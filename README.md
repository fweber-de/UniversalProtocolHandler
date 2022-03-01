# UniversalProtocolHandler

This is a tool so simply provide URL Protocol Handler for the system or the current user to open specific apps.
One use case could be to open Microsoft-365 ms-* (ms-excel, ms-word, ...) URL Schemes with Citrix Published Apps from Teams or Sharepoint Online.

## CLI Commands

### Open

```ProtocolHandler.exe Open -path="<url from offcie online>"```
  
Opens the provided file with the defined url scheme handler
  
### RegisterScheme

```ProtocolHandler.exe RegisterScheme -protocol="<protocol>" -handlerPath="<path to the exe>" -forMachine=false```

for example ```ProtocolHandler.exe RegisterScheme -protocol="ms-powerpoint" -handlerPath="C:\Users\%username%\AppData\Roaming\Citrix\SelfService\Powerpoint.exe"```  
Registers the url scheme in the registry config

### InstallUrlSchemes

```ProtocolHandler.exe InstallUrlSchemes -schemes="<schemes, comma separated>" -forMachine=false```

for example ```ProtocolHandler.exe InstallUrlSchemes -schemes="ms-excel,ms-word,ms-powerpoint"```  
Installs the registered schemes in the registry classes