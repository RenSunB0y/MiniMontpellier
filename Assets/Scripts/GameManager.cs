using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameLogic;
using System.Linq;
using Unity.VisualScripting;
using Mono.Cecil;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public enum TurnPhase
{
    Preparation,
    DiceCount,
    DiceRoll,
    Shop,
    End,
    EndGame
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
    private GameObject background;
    [SerializeField]
    private Image imageBG;

    [SerializeField]
    public List<CardSO> cards = new();

    [SerializeField]
    private bool isDouble = false;
    public Piles DrawPile;
    public List<Player> players; // Liste des joueurs
    [SerializeField]
    private Transform enemiesUIManager;
    [SerializeField]
    private CardSO farm;
    [SerializeField]
    private CardSO wheatField;
    // TextMeshProUGUI playerInfoText;
    public string[] playerNamesTexts;
    public List<GameObject> playersGameObject; // Liste des GameObjects des joueurs
    public List<Dice> diceObjects; // Liste des dés à lancer
    private int currentPlayerIndex = 0; // Index du joueur courant
    public TurnPhase currentPhase = TurnPhase.Preparation; // Phase du tour courant

    public GameObject currentPlayer; // GameObject du joueur actuel
    private int lastDiceResult;

    private int doubleCheck = 0;
    private int diceResult = 0;

    private bool isPaused = false;
    private bool waitingForDiceChoice = false; // Flag pour attendre l'entrée utilisateur
    private bool hasUsedReroll = false;
    private bool isInShop = false;

    [SerializeField]
    private GameObject shopPanel;

    IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(2);
        isPaused = false;
        AfterPause();
    }

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
                    playerNamesTexts[i] = PlayerData.PlayerNames[i]; // Assigner le nom du joueur à chaque TextMeshPro
                }
                else
                {
                    playerNamesTexts[i] = "Joueur " + (i + 1); // Par défaut, afficher "Joueur X" si non assigné
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
                players[i].playerName = PlayerData.PlayerNames[i];

                enemiesUIManager.GetChild(i>0 ? i-1 : i).gameObject.SetActive(true);
                enemiesUIManager.GetChild(i>0 ? i-1 : i).gameObject.name = $"EnemyPanel-{i}";
                enemiesUIManager.GetChild(i>0 ? i-1 : i).GetComponent<EnemyPanelUI>().Init();
            }
        }

        // Initialiser le premier joueur et commencer son tour
        if (playersGameObject.Count > 0)
        {
            currentPlayer = playersGameObject[currentPlayerIndex];
            foreach(Card c in currentPlayer.GetComponent<Player>().Deck.Pile.Keys)
                Debug.Log(c.Name);
            StartTurnForCurrentPlayer();
        }

        // UI Update
       // GameObject.FindGameObjectWithTag("MainHand").GetComponent<MainHandUI>().UpdateMainHand(currentPlayer.GetComponent<Player>().Deck.Pile); ///????????????????
    }

    void Update()
    {
        
    }

    // Démarrer un tour pour le joueur courant
    void StartTurnForCurrentPlayer()
    {
        if (players.Count > 0)
        {
            currentPlayer = playersGameObject[currentPlayerIndex];

            imageBG.sprite = currentPlayer.GetComponent<Player>().PlayerBackground;
            Debug.Log($"{currentPlayer.name}'s Turn Started");
            currentPhase = TurnPhase.Preparation;
            ChangePlayerColor(Color.green);
            //playerInfoText.text = $"Tour de {playerNamesTexts[currentPlayerIndex]}";

            NextPhase();
        }
        else
        {
            Debug.LogError("La liste des joueurs est vide.");
        }
    }

    // Passer à la phase suivante
    void NextPhase()
    {
        Debug.Log($"Phase actuelle : {currentPhase}");
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
                currentPhase = TurnPhase.Shop;
                break;
             case TurnPhase.Shop:
                currentPhase = TurnPhase.End;
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
                HandleDiceButtons();
            }
        }
        waitingForDiceChoice = false;

        //Creer un bouton pour lancer Un dé de la meme facon (en event passer le parametre 1 au rolldice())
        currentPhase = TurnPhase.DiceRoll;
        return;
    }

    void HandleDiceButtons()
    {
        //CREER DES BOUTONS ET LES ASSIGNER COMME CA : 1 EN ARGUMENT DE ROLLDICE POUR UN DE ET 2 POUR DEUX
    }

    void RollDice(int diceCount)
    {
        diceResult = 0;
        doubleCheck = 0;

        for (int i = 0; i < diceCount; i++)
        {
            int rnd = Random.Range(1, 7);
            diceResult += rnd;
            if (doubleCheck == 0)
                doubleCheck = diceResult;
            else if (doubleCheck == rnd)
                isDouble = true;
        }
        isPaused = true;
        StartCoroutine(PauseGame());
    }

    private void AfterPause()
    {
      
        foreach (Card card in currentPlayer.GetComponent<Player>().Deck.Pile.Keys)
        {
            if (card.Name == "Tour Radio" && !hasUsedReroll)
            {
                hasUsedReroll = true;
                ChooseDiceCount();
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
        isInShop = true;
        NextPhase();
        DisplayShop();
    }

    private void DisplayShop()
    {
        shopPanel.SetActive(true);
    }

    public void EndShopping()
    {
        shopPanel.SetActive(false);
        isInShop = false;
        NextPhase();
        EndTurn();
    }

    // Fin du tour du joueur courant
    void EndTurn()
    {
        Debug.Log($"{currentPlayer.name}'s Turn Ended");
        hasUsedReroll = false; 
    }

    void NextPlayer()
    {
        int _winConCount = 0;
        foreach(var card in currentPlayer.GetComponent<Player>().Deck.Pile.Keys)
        {
            if (card.Name == "Centre Commercial")
            {
                _winConCount++;
            }
            if (card.Name == "Gare" )
            {
                _winConCount++;
            }
            if (card.Name == "Tour radio")
            {
                _winConCount++;
            }
            if (card.Name == "Parc")
            {
                _winConCount++;
            }
        }

        if (_winConCount == 4)
        {
            Debug.Log("Le joueur a gagné !");
            currentPhase = TurnPhase.EndGame;
            EndGame();

            return;
        }

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

    private void EndGame()
    {

    }
}
