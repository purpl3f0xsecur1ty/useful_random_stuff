-----
# Custom .zshrc
-----
Customized `.zshrc` configuration that adds IP addresses to the terminal.  
The name is STATIC so make sure you change it if you download this!  
Note that the colors will **not** look the same on your machine, they will depend on your terminal's theme!

![](https://i.imgur.com/Bi5dUZO.png)

-----
# B64_encode_cmd.ps1
-----
Just a simple PowerShell script to encode commands to use with encoded powershell commands.

Example:

![](https://i.imgur.com/ewLWGNe.png)

Also works on PS for Kali:

![](https://i.imgur.com/BMibZv1.png)

Usage:
```powershell
powershell -enc RwBlAHQALQBDAGgAaQBsAGQASQB0AGUAbQA=
```

-----
# Class1.cs
-----
C# source code for a malicious DLL that executes shellcode.

-----
# ClassLibrary1_53.exe
-----
Malicious DLL that executes a reverse Meterpreter shell over port 53.

-----
# PipeImpersonate_cradle.cs
-----
C# source code.

-----
# PipeImpersonate_cradle.exe
-----
C# executable that captures tokens from SpoolSample and then executes a PowerShell command to download and run download_cradle.ps1.

-----
# SQL.cs
-----
C# source code.

-----
# SQL.exe
-----
C# SQL client for enumerating and attacking MSSQL servers.
![](https://i.imgur.com/iRdLiY5.png)

-----
# SQL_sa.cs
-----
C# source code.

-----
# SQL_sa.exe
-----
C# SQL client for enumerating and attacking MSSQL servers, with SA impersonation for Linked Servers.

-----
# download_cradle.ps1
-----
PowerShell that will download and execute malicious code in memory **without touching the disk**. <br />
Requires a malicious DLL hosted on a download location you control.

-----
# ps_download_oneliner.ps1
-----
Powershell that will download and execute remotely hosted Powershell scripts in memory **without touching the disk**. <br />
Piping the script to IEX (Invoke-Expression) bypasses **script execution policy**.
  
  
  
  
  
👀
