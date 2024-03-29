FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# install node.js
RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get install -y nodejs && npm install -g npm

# copy csproj and restore as distinct layers
COPY . .

# Fix dotnet restore
# RUN curl -o /usr/local/share/ca-certificates/verisign.crt -SsL https://crt.sh/?d=1039083 && update-ca-certificates

# Build client
RUN cd src/Sample.App/ClientApp && npm install && npm run build

# RUN dotnet restore
# copy everything else and build app
RUN cd src/Sample.App && dotnet restore && dotnet publish -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Sample.App.dll"]