# P_Scanner
st une application web .NET Core qui génère des QR codes et met à jour une base de données MongoDB lorsqu'un QR code est scanné. Il a été conçu pour un système de gestion de sièges de cinéma, où chaque siège a un QR code unique associé à un film et un utilisateur.
Elle est utiliser en remplacement d'un Capteur ( Scan) pour le projet P_Ciné

Fonctionnalités
Génération de QR codes : Les utilisateurs peuvent générer des QR codes à partir de trois variables : variable1 qui représente l'ID du siège, variable2 qui est l'ID de l'utilisateur, et variable3 qui est l'ID du film.
Mise à jour de la base de données : Lorsqu'un QR code est scanné, l'application met à jour la base de données MongoDB pour refléter les informations contenues dans le QR code.

Installation
Assurez-vous que vous avez installé .NET Core 3.1 ou plus récent et MongoDB.
Clonez ce dépôt sur votre machine locale.
Ouvrez le projet dans Visual Studio Code ou un autre éditeur de code.
Mettez à jour la chaîne de connexion MongoDB dans le fichier QRCodeController.cs avec vos propres informations de connexion.
Exécutez dotnet build pour compiler le projet.
Exécutez dotnet run pour lancer l'application.

Dépannage
Si vous rencontrez des problèmes lors de l'utilisation de l'application, vérifiez les points suivants :

Votre base de données MongoDB est-elle en cours d'exécution et accessible ?
Lien de la bdd mongodb+srv://dbUser:dbPassword@serverlessinstanceiottp.ygpm5nj.mongodb.net
Les variables de votre QR code sont-elles correctement formatées et séparées par des virgules ?
