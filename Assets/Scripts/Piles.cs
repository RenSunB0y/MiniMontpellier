using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piles

{ 
    public Dictionary<Card, int> Pile= new();
    private Dictionary<Card, int> updatePile = new();

    public Piles()
    {
        //Jeu(deck) du joueur
    }

    public Piles(List<CardSO> cards)
    {
        foreach (CardSO card in cards)
        {
            Pile.Add(new Card(card), card.amount);
        }
    }

    // M thodes
    public void AddCard(Card wantedCard)
    {
        foreach(Card card in Pile.Keys)
            if(card.SO==wantedCard.SO)
            {
                Pile[card]++;
                Debug.Log($"Carte existante augmentée : {wantedCard.Name}, quantité : {Pile[card]}");
                return;
            }
        Pile.Add(wantedCard, 1);
        Debug.Log($"Nouvelle carte ajoutée : {wantedCard.Name}, quantité : {Pile[wantedCard]}");
    }


    public void RemoveCard(Card wantedCard)
    {
        foreach(Card card in Pile.Keys)
            if(card.SO==wantedCard.SO)
            {
                Pile[card]--;
                if(Pile[card] == 0)
                    Pile.Remove(card);
                return;
            }
    }

    private void UpgradePile()
    {
        //On reset l'UpgradePile en vidant puis en copiant la Pile (--->  viter de rajouter plusieurs fois les am liorations)
        updatePile.Clear();
        updatePile = new Dictionary<Card, int>();
        foreach (KeyValuePair<Card, int> card in Pile)
            updatePile.Add(card.Key, card.Value);

        //Parcours chaque carte dans l'UpdatePile
        foreach (Card card in updatePile.Keys)
        {
            //Recherche am lioration pour chaque carte
            CheckCard(card, updatePile);
        }
    }

    //card = carte  tudi e, deck = jeu du joueur / itself = si la carte doit se "parcourir" elle m me
    private void CheckCard(Card card, Dictionary<Card, int> deckRef, bool itself = false)
    {
        Dictionary<Card, int> deck = new Dictionary<Card, int>(deckRef);
        //S curti  : si on ne doit pas l' dudier et qu'elle est pr sente (on la supprime)
        if (!itself && deck.ContainsKey(card))
        {
            deck.Remove(card);
        }

        //Upgrade sur b timent sp cifique (un seul)
        switch (card.Name)
        {
            case "Fromagerie":
                card.Gain = 3 * CountTypeInDeck(" levage");
                break;

            case "Fabrique de meuble":
                card.Gain = 3 * CountTypeInDeck("Production");
                break;

            case "March ":
                card.Gain = 2 * CountTypeInDeck("Agricole");
                break;

            default:
                break;
        }

        // Upgrade sur type de b timent (plusieurs)

        // Centre commercial
        // Si la pile(deck du joueur) contient le centre co et que la carte courante est de type co ou resto ---> Gain carte courante += 1 pi ce
        if (Pile.Keys.Any(mycard => mycard.Name == "Centre commercial") && (card.Type == "commerce" || card.Type == "caf "))
        {
            card.ChangeCardGain(card.Gain + 1);
        }

    }

    //Compte le nombre de carte du type donn  en param tre
    private int CountTypeInDeck(string type)
    {
        int res = 0;

        foreach (var card in Pile.Keys)
        {
            if (card.Type == type)
            {
                res++;
            }
        }

        return res;
    }

}
