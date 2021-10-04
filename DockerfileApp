FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY Server/bin/Debug/net6.0/ ./
WORKDIR ./
ENTRYPOINT ["dotnet", "Server.dll"]
EXPOSE 80/tcp
