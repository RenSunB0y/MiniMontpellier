using UnityEngine;
using GameLogic;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public CardSO champsDeBle; // Assigne une carte ScriptableObject dans l'inspecteur

    void Start()
    {
        player1 = new Player("Joueur 1", 5); // Création du joueur avec 5 pièces

        // Ajoute une carte au deck du joueur
        // player1.AddCardToDeck(champsDeBle);
    }
}
