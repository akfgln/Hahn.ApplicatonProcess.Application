#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:5.0-buster AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Hahn.ApplicatonProcess.February2021.Web/Hahn.ApplicatonProcess.February2021.Web.csproj", "Hahn.ApplicatonProcess.February2021.Web/"]
RUN dotnet restore "Hahn.ApplicatonProcess.February2021.Web/Hahn.ApplicatonProcess.February2021.Web.csproj"
COPY . .
WORKDIR "/src/Hahn.ApplicatonProcess.February2021.Web"
RUN dotnet build "Hahn.ApplicatonProcess.February2021.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hahn.ApplicatonProcess.February2021.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hahn.ApplicatonProcess.February2021.Web.dll"]