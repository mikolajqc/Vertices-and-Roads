FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY RV.sln ./
COPY RV.Model/RV.Model.csproj RV.Model/
COPY RV.Web/RV.Web.csproj RV.Web/

RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .

WORKDIR /src/RV.Web
RUN dotnet build -c Debug -o /app

FROM build AS publish
RUN dotnet publish -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RV.Web.dll"]
