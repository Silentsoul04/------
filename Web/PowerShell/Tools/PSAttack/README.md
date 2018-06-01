<p align="center">
  <img src="https://raw.githubusercontent.com/jaredhaight/PSAttack/master/PSAttackLogoBox.png" width=400>
</p>
<p align="center">
  <i>A portable console aimed at making pentesting with PowerShell a little easier.</i>
<p>

#### What is it
PS>Attack combines some of the best projects in the infosec powershell community into a self contained custom PowerShell console. It's designed to make it easy to use PowerShell offensively and to evade antivirus and Incident Response teams. It does this with in a couple of ways.

1. It features powerful tab-completion covering commands, parameters and file paths.
2. A custom command "get-attack" is included that helps you find the attack that you're looking for.
3. It doesn't rely on powershell.exe. Instead it calls powershell directly through the .NET framework. This makes it harder for enterprieses to block.
4. The modules that are bundled with the exe are obfuscated with @danielbohannon's [Invoke-Obfuscation](https://github.com/danielbohannon/Invoke-Obfuscation) and then encrypted. When PS>Attack starts, they are decrypted into memory. The plaintext payloads never touch disk, making it difficult for most antivirus engines to catch them.

PS>Attack contains over 100 commands for Privilege Escalation, Recon and Data Exfilitration. It does this by including the following modules and commands:

* [PowerSploit](https://github.com/PowerShellMafia/PowerSploit)
  - Invoke-Mimikatz
  - Get-GPPPassword
  - Invoke-NinjaCopy
  - Invoke-Shellcode
  - Invoke-WMICommand
  - VolumeShadowCopyTools
  - PowerUp
  - PowerView
* [Nishang](https://github.com/samratashok/nishang)
  - Gupt-Backdoor
  - Do-Exfiltration
  - DNS-TXT-Pwnage
  - Get-Infromation
  - Get-WLAN-Keys
  - Invoke-PsUACme
* [Powercat](https://github.com/besimorhino/powercat)
* [Inveigh](https://github.com/Kevin-Robertson/Inveigh)
* [Invoke-TheHash](https://github.com/Kevin-Robertson/Invoke-TheHash)

It also comes bundled with `get-attack`, a command that allows you to search through the included commands and find the attack that you're looking for.

![Get-Attack](http://i.imgur.com/XKUEvkl.png)

You can find a list of commands included in PS>Attack [here](https://docs.google.com/spreadsheets/d/10Axl5VE08FJGrAh0NjQ_JEskxDfRvHIgUANdnTH3z3Y/edit?usp=sharing)


#### How to use it
PS>Attack is available as a pre-compiled binary on the [releases tab](https://www.github.com/jaredhaight/PSAttack/releases/). No setup or install is required, you can just download it and run.

Another option is to use the [PS>Attack Build Tool](https://www.github.com/jaredhaight/PSAttackBuildTool). The build tool handles downloading PS>Attack, updating the modules to the latest versions, encrypting them with a unique key and then compiling the whole thing. The end result is a custom version of PS>Attack that has all the latest tools and a custom file signature thanks to the unique key.

Of course, you can also just clone the repo and compile the code yourself. You can use Visual Studio Community Edition to work with it and compie it.

#### Contact Info
If you have any questions or suggestions for PS>Attack, feel free to submit an issue or you can reachout on [twitter](https://www.twitter.com/jaredhaight) or via email: jh `at` psattack.com

#### Gr33tz
PS>Attack was inspired by and benefits from a lot of incredible people in the PowerShell community. Particularly [mattifiestation](https://twitter.com/mattifestation) of PowerSploit and [sixdub](https://twitter.com/sixdub), [engima0x3](https://twitter.com/enigma0x3) and [harmj0y](https://twitter.com/HarmJ0y) of Empire. Besides writing the modules and commands that give PS>Attack it's punch, their various projects have inspired a lot of my approach to this project as well as my decision to try and contribute something back to the community.

A huge thank you to [Ben0xA](https://twitter.com/ben0xa), who's [PoshSecFramework](https://github.com/PoshSec/PoshSecFramework) was used to figure out a lot of things about how to build a powershell console.

Thanks to [danielbohannon](https://twitter.com/danielbohannon) for writing the masterpiece that is [Invoke-Obfuscation](https://github.com/danielbohannon/Invoke-Obfuscation). I'm glad someone is crazy enough to do the research in obfuscating PowerShell.