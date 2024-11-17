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

    [SerializeField]
    private bool isDouble = false;
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
    private int lastDiceResult;

    private bool waitingForDiceChoice = false; // Flag pour attendre l'entrée utilisateur
    private bool hasUsedReroll = false;


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
        else if (waitingForDiceChoice && currentPhase == TurnPhase.DiceRoll)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("Relance des dés !");
                hasUsedReroll = true; // Marque la relance comme utilisée
                waitingForDiceChoice = false;
                RollDice(1); // Relance avec le même nombre de dés (ou adapte en fonction de ton jeu)
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Aucune relance, on garde le résultat.");
                waitingForDiceChoice = false;
                ResolveDiceEffects(lastDiceResult); // Résout les effets avec le dernier résultat
            }
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
        foreach (var card in currentPlayer.GetComponent<Player>().Deck.Pile.Keys)
        {
            Debug.Log($"Carte : {card.Name}");
            if (card.Name == "Gare")
            {
                playerInfoText.text = "Choisissez le nombre de dés à lancer (1 ou 2)";
                waitingForDiceChoice = true;
                return;
            }
            
        }
        Debug.Log("Pas de carte Gare, lancer 1 dé.");
        playerInfoText.text = "Vous n'avez pas de carte Gare, vous lancerez 1 dé.";
        waitingForDiceChoice = false;
        RollDice(1);
        currentPhase = TurnPhase.DiceRoll;
        return;
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
        int doubleCheck = 0;
        int diceResult = 0;

        for (int i = 0; i < dice; i++)
        {
            int rnd = Random.Range(1, 7);
            diceResult += rnd;

            if (doubleCheck != 0)
            {
                foreach (Card card in currentPlayer.GetComponent<Player>().Deck.Pile.Keys)
                {
                    if (card.Name == "Parc" && rnd == doubleCheck)
                    {
                        isDouble = true;
                    }
                }
            }

            doubleCheck = rnd;
        }

        Debug.Log($"Résultat des dés : {diceResult}");
        playerInfoText.text = $"Résultat des dés : {diceResult}";

        // Vérifie si le joueur possède une carte "Tour Radio"
        foreach (Card card in currentPlayer.GetComponent<Player>().Deck.Pile.Keys)
        {
            if (card.Name == "Tour Radio" && !hasUsedReroll)
            {
                playerInfoText.text = "Voulez-vous relancer les dés ?";
                waitingForDiceChoice = true;
                lastDiceResult = diceResult; // Sauvegarde le résultat en cas de relance
                return; // Attend l'entrée utilisateur dans Update
            }
        }

        // Si pas de relance possible ou déjà utilisée, applique les effets des dés
        ResolveDiceEffects(diceResult);
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
        hasUsedReroll = false; // Réinitialise la relance
        // On peut réinitialiser la couleur ou effectuer d'autres actions.
        ChangePlayerColor(Color.white); // Exemple pour remettre la couleur de base
        playerInfoText.text = $"Fin du tour de {playerNamesTexts[currentPlayerIndex].text}";
    }

    void NextPlayer()
    {
        if (isDouble)
        {
            Debug.Log("Double! C'est toujours au tour du même joueur.");
            StartTurnForCurrentPlayer();
            isDouble = false;
        }
        else
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playersGameObject.Count;
            currentPlayer = playersGameObject[currentPlayerIndex];
            StartTurnForCurrentPlayer();
        }
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
