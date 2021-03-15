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
