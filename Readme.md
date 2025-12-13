** Résumé du projet ** 

Ce projet a pour objectif de créer un backend en C# avec le framework ASP.NET Core, pour gérer plusieurs users (admin, manager et user) avec 
des permissions d'accès et d'edition différentes. Une pipeline CI, incluant des tests unitaires et d'intégration automatiques à chaque push, est mise en place via GitHub Actions. 

** Points clés **: 

-Gestion des utilisateurs: Des Users avec différents roles et permissions (Base, Manager, Admin).

-Gestion des commandes: Chaque User a une liste de commandes associées qui peuvent être ajoutées, éditées ou supprimées. 

-Authentification JWT, les endpoints protégés sont sécurisés et les informations sur l'user extraites du token. 

-Structure "Clean Architecture", avec une division en Domain, Application, Infrastructure et API pour diviser les préocupations et amincir chaque unité de code. 

-Mise en place en YAML d'une pipeline CI, avec des tests unitaires et d'intégration automatiques. Utilisation de XUnit, Moq, FluentAssertions et TestExplorer pour les tests eux-même. Coverlet pour le coverage. 

-Mise en place d'un Swagger pour la lisibilité de l'API, avec JWT fonctionel dans l'UI. 

-Seeding de la base de donnée SQLite en mode "dev" pour pouvoir tester à chaque run. 

** Prérequis **: 
-.NET 10 SDK
-SQlite
-Postman pour tester en dehors du Swagger. 

Une fois le projet clone, restoré (pour les packages) et build, il est prêt à être testé. 

La DB se seed automatiquement au lancement, et ajoute un utilisateur "Admin" permettant de réaliser toutes les opérations CRUD:
Sur la route POST/api/Auth/login
UserName: "admin"
Password: "admin". 

Le token récupéré peut être collé dans le "Authorize". 
Toutes les autres routes necessitent ce token. 

** Endpoints de l'API ** : 

/api/auth/login >>> POST >>> Récupère le JWT token en cas de login fructueux. 

/api/Order/{targetUserId} >>> POST >>> Ajoute une commande. 

/api/Order/{id} >>> PUT >>> Modifie une commande ciblée. 

/api/Order/{id} >>> DELETE >>> Supprime une commande ciblée. 

** Testing **: 

Des tests unitaires existent à chaque layer de l'architecture. 
La commande dotnet test dans le folder associé ou dans Backend.Tests run l'ensemble des tests liés. 

Un test d'intégration vérifie le fonctionnement de la chaine handler + repositories avec une DB en mémoire. 

** CI ** 

Les tests d'intégration et unitaires se lancent automatiquement à chaque push. 
En l'absence de déploiement / production, pas de pipeline CD associée. 

** Choix de SQLite ** 

Le focus du projet était sur la modélisation du User, avec des tables claires et prédéfinies (Nom, MDP, Role), lui même contenant des Orders nestées avec encore une fois des données claire et prédéfinies. 
Le layer Infrastructure permettrait de remplaçer SQLite par une autre DB lors d'un passage en production. 












