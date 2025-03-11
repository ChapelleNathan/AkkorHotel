# Akkor hotel
pour installer l'application cloner le projet comme ceci 
```bash
git clone git@github.com:ChapelleNathan/AkkorHotel.git
```

## Configuration

- Dans dupliquez le fichier .env et remplissez le crédential pour l'accès à la base de donnée.
- Dans le dossier Backend, Duppliquez le fichier appsettings.json.example et mettez y le Token pour la création de jwt et remplissez votre connection string avec vos credential de base de donnée

##Lancement du projet
pour lancer le projet mettez vous à la racine du projet et lancez
```bash
docker compose up -d
```

##Lancement des test backend
Mettez vous dans le fichier BackendTest et lancez 
```bash
dotnet test
```

##Lancement des test Frontend
Mettez vous dans le fichier Frontend et lancez
```bash
npm run cy
```
et lancez les test end to end
