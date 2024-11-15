using System.Collections.Generic;
using UnityEngine;

public enum TurnPhase
{
    Preparation,
    DiceRoll,
    End
}

public class PlayerTurn : MonoBehaviour
{
    public List<GameObject> player; // Liste des GameObjects des joueurs
    private int currentPlayerIndex = 0; // Index du joueur courant
    public TurnPhase currentPhase = TurnPhase.Preparation; // Phase du tour courant

    private GameObject currentPlayer; // GameObject du joueur actuel

    void Start()
    {
        if (player.Count > 0)
        {
            currentPlayer = player[currentPlayerIndex];
            StartTurnForCurrentPlayer();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Exemple pour avancer à la phase suivante
        {
            NextPhase();
        }
    }

    // Démarrer un tour pour le joueur courant
    void StartTurnForCurrentPlayer()
    {
        currentPhase = TurnPhase.Preparation;
        Debug.Log($"{currentPlayer.name}'s Turn Started");
        // Si tu veux par exemple changer la couleur du joueur actif :
        ChangePlayerColor(Color.green);
    }

    // Passer à la phase suivante
    void NextPhase()
    {
        switch (currentPhase)
        {
            case TurnPhase.Preparation:
                currentPhase = TurnPhase.DiceRoll;
                RollDice();
                break;
            case TurnPhase.DiceRoll:
                currentPhase = TurnPhase.End;
                EndTurn();
                break;
            case TurnPhase.End:
                NextPlayer();
                break;
        }
    }

    // Gérer le lancement des dés
    void RollDice()
    {
        int diceResult = Random.Range(1, 13); // Lancer de un ou deux dés (valeur entre 1 et 12)
        HandleDiceOutcome(diceResult);
    }

    // Appliquer l'effet du lancer de dés
    void HandleDiceOutcome(int rollResult)
    {
        switch (rollResult)
        {
            case 1:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 2:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 3:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 4:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 5:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 6:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 7:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 8:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 9:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 10:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 11:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            case 12:
                Debug.Log($"Dice rolled: {rollResult}");
                break;
            default:
                Debug.Log("Unknown dice result: No effect triggered.");
                break;
        }
    }

    // Fin du tour du joueur courant
    void EndTurn()
    {
        Debug.Log($"{currentPlayer.name}'s Turn Ended");
        // On peut réinitialiser la couleur ou effectuer d'autres actions.
        ChangePlayerColor(Color.white); // Exemple pour remettre la couleur de base
    }

    // Passer au joueur suivant
    void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % player.Count;
        currentPlayer = player[currentPlayerIndex];
        StartTurnForCurrentPlayer();
    }

    // Changer la couleur du joueur courant pour indiquer qu'il est actif
    void ChangePlayerColor(Color color)
    {
        Renderer playerRenderer = currentPlayer.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }
}
