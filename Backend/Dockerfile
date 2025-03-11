# Utiliser l'image SDK pour la compilation et la restauration des packages
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Définir le répertoire de travail dans le conteneur
WORKDIR /app

# Copier les fichiers .csproj et restaurer les dépendances
COPY *.csproj ./
RUN dotnet restore

# Copier le reste des fichiers de l'application
COPY . .

# Construire l'application en mode Release
RUN dotnet publish -c Release -o /out

# Utiliser une image runtime pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Définir le répertoire de travail dans le conteneur
WORKDIR /app

# Copier les fichiers publiés depuis l'étape de build
COPY --from=build /out .

# Exposer le port 8080
EXPOSE 8080

# Démarrer l'application
CMD ["dotnet", "Backend.dll"]