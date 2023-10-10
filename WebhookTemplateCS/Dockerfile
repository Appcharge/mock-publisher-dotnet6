# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

ARG offers_path
ARG player_dataset_path
ARG APP_PORT
ARG FACEBOOK_APP_SECRET
ARG KEY

ENV APP_PORT=$APP_PORT

# Set the working directory in the container
WORKDIR /app

# Copy the project file and restore any necessary packages
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining files and build the project
COPY . ./
RUN KEY=$KEY FACEBOOK_APP_SECRET=$FACEBOOK_APP_SECRET OFFERS_FILE_PATH=$offers_path PLAYER_DATASET_FILE_PATH=$player_dataset_path dotnet publish -c Release -o out

# Create a new image with just the published code
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY $offers_path /app$offers_path
COPY $player_dataset_path /app$player_dataset_path

# Expose the port and start the application
EXPOSE $APP_PORT
ENTRYPOINT ["dotnet", "WebhookTemplateCS.dll"]
