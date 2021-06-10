FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "CodingChainWorker.WebApi/CodingChainWorker.WebApi.csproj"
RUN dotnet build "CodingChainWorker.WebApi/CodingChainWorker.WebApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "CodingChainWorker.WebApi/CodingChainWorker.WebApi.csproj" -c Release -o /app/publish \
    && cp init.sh /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
ENV NODE_VERSION=16.3.0 APPLICATION_USER=app_user APPLICATION_GROUP=app_users ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=https://+:443;http://+:80

RUN apt-get update -yq \
    && apt-get install curl gnupg acl wget -yq \
    && apt-get update -yq \
    && groupadd -g 1010 $APPLICATION_GROUP \
    && useradd --create-home -g $APPLICATION_GROUP $APPLICATION_USER

RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update -yq\
    && apt-get install apt-transport-https -yq \
    && apt-get update -yq \
    && apt-get install  dotnet-sdk-5.0 -yq

COPY --from=publish  app/publish  /home/$APPLICATION_USER
RUN rm /bin/sh && ln -s /bin/bash /bin/sh && chown  -R  $APPLICATION_USER:$APPLICATION_GROUP /home/$APPLICATION_USER

USER $APPLICATION_USER
WORKDIR /home/$APPLICATION_USER
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.38.0/install.sh | bash \
    && source /home/$APPLICATION_USER/.nvm/nvm.sh \
    && nvm install $NODE_VERSION \
    && nvm alias default $NODE_VERSION \
    && nvm use default

EXPOSE 443 80
ENV NODE_PATH /home/$APPLICATION_USER/.nvm/v$NODE_VERSION/lib/node_modules
ENV PATH /home/$APPLICATION_USER/.nvm/versions/node/v$NODE_VERSION/bin:$PATH

RUN ls -l
ENTRYPOINT ["/bin/bash","init.sh"]
