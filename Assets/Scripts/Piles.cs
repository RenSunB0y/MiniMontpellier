using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piles
{

    public Dictionary<Card, int> Pile;
    private Dictionary<Card, int> updatePile;

    public List<Card> playerDeck;

    public Piles()
    {
        //Jeu(deck) du joueur
        Pile = new Dictionary<Card, int>(); //vide
    }

    public Piles(List<CardSO> cards)
    {
        foreach (CardSO card in cards)
        {
            Pile.Add(card.Build(), card.amount);
        }
    }

    // M thodes
    public void AddCard(Card wantedCard)
    {
        if (Pile.ContainsKey(wantedCard))
            Pile[wantedCard]++;
        else
            Pile.Add(wantedCard, 1);

        UpgradePile();
    }

    public void RemoveCard(Card wantedCard)
    {
        if (Pile.ContainsKey(wantedCard))
        {
            if (Pile[wantedCard] > 0)
            {
                Pile[wantedCard]--;
            }
            else
            {
                Pile.Remove(wantedCard);
            }

            UpgradePile();
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
    private void CheckCard(Card card, Dictionary<Card, int> deck, bool itself = false)
    {
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
