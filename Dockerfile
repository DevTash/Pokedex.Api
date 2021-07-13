FROM mcr.microsoft.com/dotnet/sdk:5.0 as BUILD
# Would probably run restore, build, test in the CI/CD pipeline itself
# Docker would then copy files and create the actual image
# Equally this is a valid approach if you want container based build infra
COPY ./Pokedex.Api.sln .
COPY ./src ./src
COPY ./tests ./tests
RUN dotnet restore Pokedex.Api.sln
RUN dotnet build -c Release --no-restore ./src/Pokedex.Api/Pokedex.Api.csproj
RUN dotnet test --no-restore --collect:"XPlat Code Coverage" ./tests/Pokedex.Api.Tests/Pokedex.Api.Tests.csproj
RUN dotnet publish -c Release -o ./publish --no-restore --no-build ./src/Pokedex.Api/Pokedex.Api.csproj


FROM mcr.microsoft.com/dotnet/aspnet:5.0 as API
WORKDIR /api
COPY --from=BUILD ./publish /api/
ENV ASPNETCORE_URLS=http://+:5006
EXPOSE 5006
ENTRYPOINT [ "dotnet", "Pokedex.Api.dll" ]