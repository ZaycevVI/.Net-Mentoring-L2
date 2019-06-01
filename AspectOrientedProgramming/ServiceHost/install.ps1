$path="bin\Debug\ServiceHost.exe" | Resolve-Path
New-Service ImgToPdfService -BinaryPathName $path
Write-Host "Press any key to continue ..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")