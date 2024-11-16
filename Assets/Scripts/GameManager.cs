using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameLogic;

public enum TurnPhase
{
    Preparation,
    DiceCount,
    DiceRoll,
    End
}

public class GameManager : MonoBehaviour
{
    public List<Player> players; // Liste des joueurs
    public CardSO ferme; // Référence au ScriptableObject pour Farm
    public CardSO boulangerie; // Référence au ScriptableObject pour Bakery
    public TextMeshProUGUI playerInfoText;
    public TextMeshProUGUI[] playerNamesTexts;
    public List<GameObject> player; // Liste des GameObjects des joueurs
    private int currentPlayerIndex = 0; // Index du joueur courant
    public TurnPhase currentPhase = TurnPhase.Preparation; // Phase du tour courant

    private GameObject currentPlayer; // GameObject du joueur actuel
    private bool waitingForDiceChoice = false; // Flag pour attendre l'entrée utilisateur

    void Start()
    {
        // S'assurer que PlayerData.PlayerNames contient les noms des joueurs
        if (PlayerData.PlayerNames.Count > 0)
        {
            // Mise à jour des noms des joueurs dans l'UI (TextMeshPro)
            for (int i = 0; i < playerNamesTexts.Length; i++)
            {
                if (i < PlayerData.PlayerNames.Count)
                {
                    playerNamesTexts[i].text = PlayerData.PlayerNames[i]; // Assigner le nom du joueur à chaque TextMeshPro
                }
                else
                {
                    playerNamesTexts[i].text = "Joueur " + (i + 1); // Par défaut, afficher "Joueur X" si non assigné
                }
            }

            // Initialisation des GameObjects des joueurs
            player = new List<GameObject>();
            for (int i = 0; i < PlayerData.PlayerNames.Count; i++)
            {
                player.Add(GameObject.Find("Player" + (i + 1))); // Trouver les GameObjects des joueurs actifs
            }

            // Ajouter des cartes initiales aux joueurs sélectionnés
            for (int i = 0; i < PlayerData.PlayerNames.Count; i++)
            {
                players[i].AddCardToDeck(ferme);
                players[i].AddCardToDeck(boulangerie);
            }
        }

        // Initialiser le premier joueur et commencer son tour
        if (player.Count > 0)
        {
            currentPlayer = player[currentPlayerIndex];
            StartTurnForCurrentPlayer();
        }
    }

    void Update()
    {
        if (waitingForDiceChoice && currentPhase == TurnPhase.DiceCount)
        {
            HandleDiceInput();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) // Exemple pour avancer à la phase suivante
        {
            NextPhase();
        }
    }

    // Démarrer un tour pour le joueur courant
    void StartTurnForCurrentPlayer()
    {
        if (players.Count > 0)
        {
            currentPlayer = player[currentPlayerIndex];
            Debug.Log($"{currentPlayer.name}'s Turn Started");
            currentPhase = TurnPhase.Preparation;
            ChangePlayerColor(Color.green);
            playerInfoText.text = $"Tour de {playerNamesTexts[currentPlayerIndex].text}";
        }
        else
        {
            Debug.LogError("La liste des joueurs est vide.");
        }
    }

    // Passer à la phase suivante
    void NextPhase()
    {
        switch (currentPhase)
        {
            case TurnPhase.Preparation:
                currentPhase = TurnPhase.DiceCount;
                ChooseDiceCount();
                break;
            case TurnPhase.DiceCount:
                // Cette phase sera avancée après le choix des dés
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

    void ChooseDiceCount()
    {
        waitingForDiceChoice = true;
        playerInfoText.text = "Voulez-vous lancer 1 ou 2 dés ?\nAppuyez sur [1] ou [2]";
    }

    void HandleDiceInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            waitingForDiceChoice = false;
            Debug.Log("1 dé choisi");
            RollDice(1);
            currentPhase = TurnPhase.DiceRoll; // Avance à la phase suivante
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            waitingForDiceChoice = false;
            Debug.Log("2 dés choisis");
            RollDice(2);
            currentPhase = TurnPhase.DiceRoll; // Avance à la phase suivante
        }
    }

    void RollDice(int dice)
    {
        int diceResult = 0;
        for (int i = 0; i < dice; i++)
        {
            diceResult += Random.Range(1, 7);
        }
        Debug.Log($"Résultat des dés : {diceResult}");
        ResolveDiceEffects(diceResult);
        playerInfoText.text = $"Résultat des dés : {diceResult}";
    }

    // Appliquer l'effet du lancer de dés
    void ResolveDiceEffects(int diceResult)
    {
        // Récupérer le joueur courant à partir de la liste des joueurs
        Player player = players[currentPlayerIndex];

        if (player.Deck == null || player.Deck.Count == 0)
        {
            Debug.LogWarning("Le deck du joueur est vide ou non initialisé.");
            return;
        }

        // Parcourir les cartes dans le deck du joueur
        foreach (var card in player.Deck)
        {
            foreach (int diceValue in card.dice)
            {
                if (diceValue == diceResult)
                {
                    Debug.Log($"Activation de la carte : {card.name}, Effet : {card.effect}");
                    // Ici, ajoute la logique d'application de l'effet de la carte
                    break; // Une fois activée, on peut sortir de la boucle interne
                }
            }
        }
    }

    // Fin du tour du joueur courant
    void EndTurn()
    {
        Debug.Log($"{currentPlayer.name}'s Turn Ended");

        // On peut réinitialiser la couleur ou effectuer d'autres actions.
        ChangePlayerColor(Color.white); // Exemple pour remettre la couleur de base
        playerInfoText.text = $"Fin du tour de {playerNamesTexts[currentPlayerIndex].text}";
    }

    void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % player.Count;
        currentPlayer = player[currentPlayerIndex];
        StartTurnForCurrentPlayer();
    }

    void ChangePlayerColor(Color color)
    {
        Renderer playerRenderer = currentPlayer.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }
}
