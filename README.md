# MiniMontpellier

## Description

MiniMontpellier est une reprise du jeu de plateau Minivilles où les joueurs lancent des dés, utilisent des cartes et gèrent des ressources pour gagner. 
Le jeu se déroule en tours, chaque joueur ayant l'opportunité de jouer et d'interagir avec différentes cartes et mécaniques.
Le but est de prendre des décisions stratégiques basées sur le résultat des dés et les cartes que vous avez dans votre deck.

## Installation

1. Clonez ce dépôt sur votre machine locale :
   ```bash
   git clone https://github.com/RenSunB0y/Minivilles.git
   ```
2. Ouvrez le projet dans Unity.
3. Assurez-vous que toutes les dépendances sont installées.
4. Lancez le jeu directement dans Unity depuis la scene "MainMenu".

## Comment jouer

### 1. Préparation
Au début de chaque partie, le jeu commence avec un certain nombre de joueurs. Chaque joueur a un deck de cartes, qui contient des cartes spéciales (exemple : "Boulangerie", "Ferme", "Café"). Les joueurs commenceront leur tour en choisissant le nombre de dés qu'ils veulent lancer.

### 2. Lancer les dés
Au début de chaque tour, un joueur choisit combien de dés il souhaite lancer (1 ou 2). Après le lancement, le jeu affiche le résultat des dés, et certaines cartes dans le deck du joueur peuvent influencer ce résultat. Certaines cartes permettent même de relancer les dés pour tenter d'obtenir un meilleur résultat.

### 3. Relancer les dés
Si le joueur possède une carte lui permettant (exemple : "Tour Radio"), il a la possibilité de relancer ses dés. Si le joueur choisit de ne pas relancer, les effets des dés seront appliqués.

### 4. Effets des dés
Le résultat des dés peut affecter les ressources du joueur ou déclencher des actions spéciales en fonction des cartes qu'il possède dans son deck. Par exemple, une carte peut multiplier un certain résultat de dé ou offrir des ressources supplémentaires.

### 5. Fin du tour
Une fois que le joueur a effectué toutes les actions possibles en fonction des dés et des cartes, son tour est terminé. Le joueur suivant commence son tour. Si un joueur obtient un résultat particulier (par exemple, une combinaison gagnante), la partie se termine et le gagnant est déterminé.

### 6. Gagner la partie
La partie se termine lorsqu'un joueur remplit les conditions de victoire. Ces conditions sont définies par les cartes qu'il possède dans son deck. Le premier joueur à avoir acheté toutes les cartes monuments ("Gare", "Parc d'attraction", "Tour Radio" et "Centre commercial") remporte la partie.

## Technologies utilisées

* **Unity** : Moteur de jeu utilisé pour le développement.
* **C#** : Langage de programmation utilisé pour la logique du jeu.
