# 构建阶段
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 复制项目文件
COPY *.sln .
COPY Sys.Hub.Web.Entry/*.csproj ./Sys.Hub.Web.Entry/
COPY Sys.Hub.Application/*.csproj ./Sys.Hub.Application/
COPY Sys.Hub.Core/*.csproj ./Sys.Hub.Core/
COPY Sys.Hub.Common/*.csproj ./Sys.Hub.Common/
COPY Sys.Hub.Web.Core/*.csproj ./Sys.Hub.Web.Core/
COPY Sys.Hub.EntityFramework.Core/*.csproj ./Sys.Hub.EntityFramework.Core/
COPY Sys.Hub.Database.Migrations/*.csproj ./Sys.Hub.Database.Migrations/

# 还原依赖
RUN dotnet restore

# 复制所有源代码
COPY . .

# 发布应用
WORKDIR /src/Sys.Hub.Web.Entry
RUN dotnet publish -c Release -o /app/publish

# 运行阶段
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# 复制发布文件
COPY --from=build /app/publish .

# 设置环境变量
ENV ASPNETCORE_URLS=http://+:5208
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5208

ENTRYPOINT ["dotnet", "Sys.Hub.Web.Entry.dll"]
