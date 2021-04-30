FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
COPY . .  
EXPOSE 80
EXPOSE 81
ENTRYPOINT ["dotnet", "Spear.Demo4GRPC.Host.Server.dll"]