#docker pull consul
#docker run -d -p 8500:8500 -p 8300:8300 -p 8301:8301 -p 8302:8302 -p 8600:8600 --name=consul consul agent -client 0.0.0.0 --node=c1 -dev -ui

#docker pull fabiolb/fabio
#docker run -d -p 9999:9999 -p 9998:9998 -e registry_consul_addr=192.168.186.225:8500 -e registry_consul_serviceaddr=192.168.186.225:9998 -e proxy_addr=':9999;proto=grpc' --name=fabio fabiolb/fabio

#FROM microsoft/dotnet:3.1-aspnetcore-runtime
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
#WORKDIR /app
#COPY . .
#ENV ASPNETCORE_URLS http://*:80;https://*:81
#ENV ASPNETCORE_ENVIRONMENT Development »ò Staging »ò Production
#EXPOSE 80
#EXPOSE 81
#ENTRYPOINT ["dotnet", "Spear.DataProvider.Host.????.dll"]

#$Env:ASPNETCORE_ENVIRONMENT = "Staging"
#export ASPNETCORE_ENVIRONMENT=Staging »ò Production

#docker exec ???? ls /app/logs/
#docker exec ???? cat /app/logs/???/???.log

#docker build -t schedule .
#docker run -d --env ASPNETCORE_ENVIRONMENT=Staging --name=schedule_0 schedule

#docker build -t testdemo .
#docker run -d -p 7000:80 -p 7001:81 --env ASPNETCORE_ENVIRONMENT=Staging --name=testdemo_0 testdemo hostpublic=119.8.46.181 hostinternal=192.168.0.173 webapiport=7000 grpcport=7001

#$Env:ASPNETCORE_ENVIRONMENT = "Development"
#dotnet Spear.DispatchCenter.Host.Distribution.Actuator.dll teamflag=common5566 ispublic=true