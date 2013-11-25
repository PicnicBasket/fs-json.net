powershell -NoProfile -ExecutionPolicy unrestricted -File .\psake.ps1
.nuget\NuGet.exe pack .\fs-json.net.nuspec -BasePath ..
