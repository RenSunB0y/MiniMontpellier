﻿using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
    public class Player : MonoBehaviour
    {
        public string playerName;
        public int coins;
        [HideInInspector]
        public Piles Deck; // Liste des cartes du joueur

        private void Start()
        {
            Deck = new Piles(); // Initialisation du Deck
        }

        public void CheckEffects(Player playerActuel, int diceResult, Card card)
        {
            { 
                if (!playerActuel == this)
                {
                    switch (diceResult)
                    {
                        case 1: Gain(card.Gain); break;
                        case 2: Gain(card.Gain); break;
                        case 3:
                            playerActuel.Pay(card.Cost);
                            Gain(card.Gain); // ++++++ Limiter la perte jusque 0 et pas en dessous plus n'ajouter que ce qui a été débité
                            break;
                        case 5: Gain(card.Gain); break;
                        case 9:
                            if (card.Color == "Red")
                            {
                                playerActuel.Pay(card.Cost);
                                Gain(card.Gain);
                            }
                            else
                            { Gain(card.Gain); }
                            break;
                        case 10:
                            if (card.Color == "Red")
                            {
                                playerActuel.Pay(card.Cost);
                                Gain(card.Gain);
                            }
                            else
                            { Gain(card.Gain); } 
                            break;

                    }
                }

                else if (playerActuel == this)
                { 
                    if (diceResult != 6)
                    {
                        Gain(card.Gain);
                    }
                    else
                    {
                        //Trucs avec cartes speciales
                    }
                }
            }
        }

        public void Gain(int gain)
        {
            coins += gain;
        }

        public void Pay(int payment)
        { 
            coins -= payment;
        }

    }
}
