$text = Read-Host -Prompt 'Command to base64 encode '
$bytes = [System.Text.Encoding]::Unicode.GetBytes($text)
$EncodedText = [Convert]::ToBase64String($bytes)
Write-Host "Encoded string: "
Write-Host $EncodedText