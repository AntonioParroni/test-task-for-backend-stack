FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY bin/Release/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "Server.dll"]