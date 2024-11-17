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
    private Transform playerDataUI;
    [SerializeField]
    private CardSO[] startPlayerDeck;
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
    [SerializeField]
    private Transform diceGroup;
    [SerializeField]
    private Transform diceButtons;
    [SerializeField]
    private Sprite[] diceFacesSprite;

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
                foreach(CardSO data in startPlayerDeck)
                {
                    players[i].Deck.AddCard(new Card(data));
                }
                players[i].playerName = PlayerData.PlayerNames[i];

                enemiesUIManager.GetChild(i>0 ? i-1 : i).gameObject.SetActive(true);
                enemiesUIManager.GetChild(i>0 ? i-1 : i).GetComponent<EnemyPanelUI>().Init(players[i]);
            }
        }

        // Initialiser le premier joueur et commencer son tour
        if (playersGameObject.Count > 0)
        {
            currentPlayer = playersGameObject[currentPlayerIndex];
            playerDataUI.GetComponent<EnemyPanelUI>().Init(currentPlayer.GetComponent<Player>());
            playerDataUI.gameObject.SetActive(true);
            foreach(Card c in currentPlayer.GetComponent<Player>().Deck.Pile.Keys)
                Debug.Log(c.Name);
            StartTurnForCurrentPlayer();

        }

        // UI Update
        UpdateUI();
    }

    void UpdateUI()
    {
        Debug.Log("Update UI");
       currentPlayer = playersGameObject[currentPlayerIndex];
        for(int i=0; i<players.Count;i++)
        {
            if(i==currentPlayerIndex)
                playerDataUI.GetComponent<EnemyPanelUI>().Init(players[i]);
            else
            {
                enemiesUIManager.GetChild(i>=3 ? 2 : i).GetComponent<EnemyPanelUI>().Init(players[i]);
            }
        }
       GameObject.FindGameObjectWithTag("MainHand").GetComponent<MainHandUI>().UpdateMainHand(currentPlayer.GetComponent<Player>().Deck.Pile);
        imageBG.sprite = currentPlayer.GetComponent<Player>().PlayerBackground;
        Debug.Log($"{currentPlayer.name}'s Turn Started");
        currentPhase = TurnPhase.Preparation;
    }

    // Démarrer un tour pour le joueur courant
    void StartTurnForCurrentPlayer()
    {
        if (players.Count > 0)
        {
            UpdateUI();
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
        waitingForDiceChoice = false;
        UpdateDiceUI(currentPlayer.GetComponent<Player>().Deck.Pile.Keys.Any(card => card.Name == "Gare") ? 2 : 1);
        //Creer un bouton pour lancer Un dé de la meme facon (en event passer le parametre 1 au rolldice())
        currentPhase = TurnPhase.DiceRoll;
        return;
    }

    void UpdateDiceUI(int diceCount)
    {
        for(int i=0; i<2; i++)
        {
            diceGroup.GetChild(i).gameObject.SetActive(i<diceCount);
        }
        UpdateButtonDiceUI(diceCount);
    }

    void UpdateButtonDiceUI(int butCount)
    {
        for(int i=0; i<2; i++)
        {
            diceButtons.GetChild(i).gameObject.SetActive(i<butCount);
        }
    }

    public void RollDice(int diceCount)
    {
        UpdateDiceUI(diceCount);
        UpdateButtonDiceUI(0);
        diceResult = 0;
        doubleCheck = 0;

        for (int i = 0; i < diceCount; i++)
        {
            int rnd = Random.Range(1, 7);
            diceGroup.GetChild(i).GetComponent<DiceRollAnim>().actualFace = diceFacesSprite[rnd-1];
            diceGroup.GetChild(i).GetComponent<Animator>().enabled = true;
            diceGroup.GetChild(i).GetComponent<Animator>().SetTrigger("RollDice");
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
        Debug.Log(diceResult);
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
        enemiesUIManager.gameObject.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void EndShopping()
    {
        shopPanel.SetActive(false);
        enemiesUIManager.gameObject.SetActive(true);
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

    private void EndGame()
    {

    }
}
