using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piles : MonoBehaviour
{
    Dictionary<Cards, int> Pile;
    Dictionary<Cards, int> UpdatePile;

    public Piles()
    {
        //Jeu(deck) du joueur
        Pile = new Dictionary<Cards, int>(); //vide
    }

    //Constructeur pour jeu de carte (toutes)
    public Piles(List<CardSO> cards)
    {
        foreach (CardSO card in cards)
        {
            Pile.Add(new Cards(card), card.amount);
        }
    }

    // Méthodes
    public void AddCard(Cards wantedCard)
    {
        if (Pile.ContainsKey(wantedCard))
            Pile[wantedCard]++;
        else
            Pile.Add(wantedCard, 1);

        UpgradePile();
    }

    public void RemoveCard(Cards wantedCard)
    {
        if (Pile.ContainsKey(wantedCard))
            Pile[wantedCard]++;
        else
            Pile.Add(wantedCard, 1);

        UpgradePile();
    }

    private void UpgradePile()
    {
        //On reset l'UpgradePile en vidant puis en copiant la Pile (---> éviter de rajouter plusieurs fois les améliorations)
        UpdatePile.Clear();
        UpdatePile = new Dictionary<Cards, int>();
        foreach(KeyValuePair<Cards,int> card in Pile)
            UpdatePile.Add(card.Key, card.Value);

        //Parcours chaque carte dans l'UpdatePile
        foreach (Cards card in UpdatePile.Keys)
        {
            //Recherche amélioration pour chaque carte
            CheckCard(card,UpdatePile);
        }
    }

    //card = carte étudiée, deck = jeu du joueur / itself = si la carte doit se "parcourir" elle même
    private void CheckCard(Cards card, Dictionary<Cards, int> deck, bool itself = false)
    {
        //Sécurtié : si on ne doit pas l'édudier et qu'elle est présente (on la supprime)
        if (!itself && deck.ContainsKey(card))
        {
            deck.Remove(card);
        }

        //Upgrade sur bâtiment spécifique (un seul)
        switch (card.Name)
        {
            case "Fromagerie":
                card.Gain = 3 * CountTypeInDeck("Élevage");
                break;

            case "Fabrique de meuble":
                card.Gain = 3 * CountTypeInDeck("Production");
                break;

            case "Marché":
                card.Gain = 2 * CountTypeInDeck("Agricole");
                break;

            default:
                break;
        }

        // Upgrade sur type de bâtiment (plusieurs)

        // Centre commercial
        // Si la pile(deck du joueur) contient le centre co et que la carte courante est de type co ou resto ---> Gain carte courante += 1 pièce
        if (Pile.Keys.Any(mycard => mycard.Name == "Centre commercial") && (card.Type == "commerce" || card.Type == "café"))
        {
            card.ChangeCardGain(card.Gain + 1);
        }

    }

    //Compte le nombre de carte du type donné en paramètre
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
