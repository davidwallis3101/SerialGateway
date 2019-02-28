dotnet build SerialGateway.sln -r ubuntu.16.04-x64
pscp -agent -r "C:\Users\Davidw\Source\Repos\SerialGateway\SerialGateway\bin\Debug\*" davidw@192.168.0.181:/home/davidw/Debug
