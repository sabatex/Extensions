# Sabatex.Tools (The command line project publish helper)
- NUGET Packets (Automatic publishing of debug versions to the local repository and release of versions to the NUGET repository. )

- ASP.NET Core project publish to linux server with NGINX.

## Install

To get started, copy the sabatex-tools.exe to the folder whose path is specified in the PATH environment variable

## Using
At the command prompt, go to the folder where the *.csproj file is located. And run sabatex-tools.exe, depending on the type of project, the dll will publish to NUGET and the EXE to linux host 

### Settings for publishing
- All settings must be placed in appsettings.json or usersecrets.
- NUGET Settings:
```json
{
  "SabatexSettings": {
    "NUGET": {
      "nugetAuthTokenPath": "you path\\token file name",
      "LocalDebugStorage": "local folder for NUGET debug storfge"
    }
  }
}
```

- Ubuntu settings:
```
  "SabatexSettings:NGINXPublish:TempFolder": "temp folder path or space (default user temp folder)",
  "SabatexSettings:NGINXPublish:SSLPublic": "ssl public certificate path or space",
  "SabatexSettings:NGINXPublish:SSLPrivate": "ssl private certificate path or space",
  "SabatexSettings:NGINXPublish:Service:ServiceName": "service name in ubuntu or space (service name as project name)",
  "SabatexSettings:NGINXPublish:Service:Port": "asp net application port",
  "SabatexSettings:NGINXPublish:ProjectName": "project name or space",
  "SabatexSettings:NGINXPublish:LinuxWebFolder": "folder wish sitetes as /var/www",
  "SabatexSettings:NGINXPublish:LinuxTempFolder": "temp folder in linux (/home/azureuser/temp)",
  "SabatexSettings:NGINXPublish:HostName": "you host name (contoso.com)",
  "SabatexSettings:NGINXPublish:BlazorContent": "path blazor content for standalone Blazor WASM",
  "SabatexSettings:Linux:UserHomeFolder": "/home/azureuser",
  "SabatexSettings:Linux:Service:UpdateService": "False",
  "SabatexSettings:Linux:Service:ServiceName": "you service name",
  "SabatexSettings:Linux:Service:Port": "asp application port",
  "SabatexSettings:Linux:PublishFolder": "/opt/app/sabatex-exchange",
  "SabatexSettings:Linux:FrontEnd": "False", // True - Blazor WASM standalone, false - asp.net core project
  "SabatexSettings:Linux:BitviseTlpFile": "Bitvise tlp file path",

```
