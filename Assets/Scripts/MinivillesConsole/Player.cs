using System.Collections.Generic;

namespace GameLogic
{
    public class Player
    {
        public List<Card> Deck { get; set; } = new List<Card>(); // Liste des cartes du joueur
        public int CoinCount { get; set; } // Exemple d'attribut pour les pièces du joueur
        public string Name { get; set; } // Exemple d'attribut pour le nom du joueur

        // Méthode pour obtenir le nombre de copies d'une carte spécifique
        public int GetCardCount(Card card)
        {
            int count = 0;
            foreach (var c in Deck)
            {
                if (c.Equals(card)) // Vérifie si la carte dans le deck est égale à celle donnée
                {
                    count++;
                }
            }
            return count;
        }
    }
}
