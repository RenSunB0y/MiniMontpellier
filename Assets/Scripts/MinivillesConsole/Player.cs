using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Player : MonoBehaviour
    {
        public string playerName;
        public int coins;
        public List<CardSO> Deck;  // Liste des cartes du joueur

        // Constructeur pour initialiser le joueur avec un nom et une quantité de pièces
        public Player(string name, int initialCoins)
        {
            playerName = name;
            coins = initialCoins;
            Deck = new List<CardSO>();  // Initialisation du Deck
        }

        // Méthode pour ajouter une carte au Deck
        public void AddCardToDeck(CardSO card)
        {
            Debug.Log($"Ajout de la carte {card.name} au deck du joueur {playerName}");
            Deck.Add(card);
        }
    }
}
