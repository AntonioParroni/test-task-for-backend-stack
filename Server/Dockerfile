FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY bin/Debug/net6.0/ ./
WORKDIR ./
ENTRYPOINT ["dotnet", "Server.dll"]

# docker build -t aspnetapp .
# docker run -d -p 8080:80 --name myapp aspnetapp