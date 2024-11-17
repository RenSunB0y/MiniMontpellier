﻿using JetBrains.Annotations;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
    public class Player : MonoBehaviour
    {
        public string playerName;
        public int coins =0;
        public int coinsPerdus = 0;
        
        public Piles Deck; // Liste des cartes du joueur

        private void Start()
        {

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
                            playerActuel.Loss(card.Gain);
                            Gain(playerActuel.coinsPerdus);
                            break;
                        case 5: Gain(card.Gain); break;
                        case 9:
                            if (card.Color == "Red")
                            {
                                playerActuel.Loss(card.Gain);
                                Gain(playerActuel.coinsPerdus);
                            }
                            else
                            { Gain(card.Gain); }
                            break;
                        case 10:
                            if (card.Color == "Red")
                            {
                                playerActuel.Loss(card.Gain);
                                Gain(playerActuel.coinsPerdus);
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
                        if (card.Name == "Stade")
                        {
                            foreach (var _player in GameManager.Instance.players)
                            {

                                if (_player != this)
                                {
                                    _player.Loss(card.Gain);
                                    Gain(_player.coinsPerdus); 
                                }
                            }
                        }
                        else if (card.Name == "Chaîne de télévision")
                        {
                            Player _pickedPlayer = null;
                            _pickedPlayer.Loss(card.Gain);
                            Gain(card.Gain);
                        }
                        else if (card.Name == "Centre d'affaires")
                        {
                            Player _pickedPlayer= null;
                            Card _pickedCard = null;
                            Card _GivenCard = null;

                            _pickedPlayer.Deck.AddCard(_GivenCard);
                            Deck.RemoveCard(_GivenCard);
                            Deck.AddCard(_pickedCard);
                            _pickedPlayer.Deck.RemoveCard(_pickedCard);
                        }
                        
                    }
                }
            }
        }

        public void Gain(int gain)
        {
            coins += gain;
        }

        public void Loss(int loss)
        {
            coinsPerdus = 0;
            if (coins < loss)
            {
                coinsPerdus = coins;
                coins = 0;
            }
            else
            {
                coins -= loss;
                coinsPerdus = loss;
            }
        }

    }
}
