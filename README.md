# UniversalProtocolHandler

This is a tool to simply provide URL Protocol Handlers for the system or the current user to open specific apps.
One use case could be to open Microsoft-365 ms-* (ms-excel, ms-word, ...) URL Schemes with Citrix Published Apps from Teams or Sharepoint Online.

## CLI Commands

### Open

```ProtocolHandler.exe Open -path="<url from office online>"```
  
Opens the provided file with the defined url scheme handler
  
### RegisterScheme

```ProtocolHandler.exe RegisterScheme -protocol="<protocol>" -handlerPath="<path to the exe>" -forMachine=<bool> -install=<bool>```

for example ```ProtocolHandler.exe RegisterScheme -protocol="ms-powerpoint" -handlerPath="C:\Users\%username%\AppData\Roaming\Citrix\SelfService\Powerpoint.exe" -install```  
Registers the url scheme in the registry config and also installs it for use

### InstallUrlSchemes

```ProtocolHandler.exe InstallUrlSchemes -schemes="<schemes, comma separated>" -forMachine=false```

for example ```ProtocolHandler.exe InstallUrlSchemes -schemes="ms-excel,ms-word,ms-powerpoint"```  
Installs the registered schemes in the registry classes

## Supported Schemes

Currently supports fixed schemes/protocol discovery via regex. There is an active Issue to refactor this to support variable schemes provided by the user.

* MS-WORD
* MS-EXCEL
* MS-POWERPOINT
* MECMRC
