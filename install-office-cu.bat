ProtocolHandler.exe RegisterScheme -protocol="ms-word" -handlerPath="C:\Users\%username%\AppData\Roaming\Citrix\SelfService\Word.exe"
ProtocolHandler.exe RegisterScheme -protocol="ms-excel" -handlerPath="C:\Users\%username%\AppData\Roaming\Citrix\SelfService\Excel.exe"
ProtocolHandler.exe RegisterScheme -protocol="ms-powerpoint" -handlerPath="C:\Users\%username%\AppData\Roaming\Citrix\SelfService\Powerpoint.exe"

ProtocolHandler.exe InstallUrlSchemes -schemes="ms-excel,ms-word,ms-powerpoint"