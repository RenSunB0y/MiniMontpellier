using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameLogic;
using System.Linq;
using Unity.VisualScripting;
using Mono.Cecil;

public enum TurnPhase
{
    Preparation,
    DiceCount,
    DiceRoll,
    End
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    private List<CardSO> cards = new();

    public Piles DrawPile;
    public List<Player> players; // Liste des joueurs
    [SerializeField]
    private CardSO farm;
    [SerializeField]
    private CardSO wheatField;
    public TextMeshProUGUI playerInfoText;
    public TextMeshProUGUI[] playerNamesTexts;
    public List<GameObject> playersGameObject; // Liste des GameObjects des joueurs
    private int currentPlayerIndex = 0; // Index du joueur courant
    public TurnPhase currentPhase = TurnPhase.Preparation; // Phase du tour courant

    private GameObject currentPlayer; // GameObject du joueur actuel
    private bool waitingForDiceChoice = false; // Flag pour attendre l'entrée utilisateur

    void Start()
    {
        DrawPile = new Piles(cards); // Initialisation du Draw


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
            playersGameObject = new List<GameObject>();
            for (int i = 0; i < PlayerData.PlayerNames.Count; i++)
            {
                players[i].Deck = new Piles(); // Initialisation du Deck
                Debug.Log($"{PlayerData.PlayerNames[i]} a été initialisé.");
                playersGameObject.Add(GameObject.Find("Player" + (i + 1))); // Trouver les GameObjects des joueurs actifs
                players[i].Deck.AddCard(new Card(farm));
                players[i].Deck.AddCard(new Card(wheatField));
            }
        }

        // Initialiser le premier joueur et commencer son tour
        if (playersGameObject.Count > 0)
        {
            currentPlayer = playersGameObject[currentPlayerIndex];
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
            currentPlayer = playersGameObject[currentPlayerIndex];
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


        foreach (GameObject _player in playersGameObject)
        // Parcourir les cartes dans le deck du joueur
        {
            // montre les cartes du deck du joueur
            Debug.Log($"Deck du joueur{currentPlayerIndex + 1} {_player.GetComponent<Player>().Deck.Pile.Count}");

            if (_player.GetComponent<Player>().Deck == null || _player.GetComponent<Player>().Deck.Pile.Count == 0)
            {
                Debug.LogWarning("Le deck du joueur est vide ou non initialisé.");
                return;
            }

            foreach (var card in _player.GetComponent<Player>().Deck.Pile.Keys)
            {
                if (card.Dice.Contains(diceResult))
                {
                    for (int i = 0; i < _player.GetComponent<Player>().Deck.Pile[card]; i++)
                    {
                        _player.GetComponent<Player>().CheckEffects(player, diceResult, card);
                    }
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
        currentPlayerIndex = (currentPlayerIndex + 1) % playersGameObject.Count;
        currentPlayer = playersGameObject[currentPlayerIndex];
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
