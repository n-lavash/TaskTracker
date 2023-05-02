#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TaskTracker.PL/TaskTracker.PL.csproj", "TaskTracker.PL/"]
COPY ["TaskTracker.BLL/TaskTracker.BLL.csproj", "TaskTracker.BLL/"]
COPY ["TaskTracker.BLL.Interfaces/TaskTracker.BLL.Interfaces.csproj", "TaskTracker.BLL.Interfaces/"]
COPY ["TaskTracker.Entity/TaskTracker.Entity.csproj", "TaskTracker.Entity/"]
COPY ["TaskTracker.DAL.Interfaces/TaskTracker.DAL.Interfaces.csproj", "TaskTracker.DAL.Interfaces/"]
COPY ["TaskTracker.DAL/TaskTracker.DAL.csproj", "TaskTracker.DAL/"]
RUN dotnet restore "TaskTracker.PL/TaskTracker.PL.csproj"
COPY . .
WORKDIR "/src/TaskTracker.PL"
RUN dotnet build "TaskTracker.PL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskTracker.PL.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskTracker.PL.dll"]