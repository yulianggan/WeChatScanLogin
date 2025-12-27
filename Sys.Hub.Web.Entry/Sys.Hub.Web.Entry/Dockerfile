FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5208

COPY . .
ENTRYPOINT ["dotnet", "Sys.Hub.Web.Entry.dll"]