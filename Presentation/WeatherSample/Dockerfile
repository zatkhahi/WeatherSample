#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Presentation/WeatherSample/WeatherSample.csproj", "Presentation/WeatherSample/"]
COPY ["Infrastructure/Persistence/WeatherSample/WeatherSample.Persistence.csproj", "Infrastructure/Persistence/WeatherSample/"]
COPY ["Core/Domain/WeatherSample.Domain/WeatherSample.Domain.csproj", "Core/Domain/WeatherSample.Domain/"]
COPY ["Core/Domain/Common/Common.Domain.csproj", "Core/Domain/Common/"]
COPY ["Infrastructure/Persistence/Common/Common.Persistence.csproj", "Infrastructure/Persistence/Common/"]
COPY ["Core/Application/Core/Common.Application.csproj", "Core/Application/Core/"]
COPY ["Core/Application/WeatherSample.Application/WeatherSample.Application.csproj", "Core/Application/WeatherSample.Application/"]
COPY ["Presentation/Common.Presentation/Common.Presentation.csproj", "Presentation/Common.Presentation/"]
RUN dotnet restore "Presentation/WeatherSample/WeatherSample.csproj"
COPY . .
WORKDIR "/src/Presentation/WeatherSample"
RUN dotnet build "WeatherSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRMWebApp.dll"]