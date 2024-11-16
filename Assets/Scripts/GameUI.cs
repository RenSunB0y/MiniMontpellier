// using UnityEngine;
// using GameLogic;
// using TMPro;

// public class GameUI : MonoBehaviour
// {
//     public Player currentPlayer; // Référence au joueur actuel
//     public TextMeshProUGUI playerInfoText; // Pour afficher les infos des cartes

//     void Start()
//     {
//         DisplayPlayerDeck();
//     }

//     public void DisplayPlayerDeck()
//     {
//         playerInfoText.text = $"Cartes du joueur {currentPlayer.Name}:\n";

//         foreach (CardSO card in currentPlayer.Deck)
//         {
//             playerInfoText.text += $"- {card.name} (Coût: {card.cost}, Gain: {card.gain})\n";
//         }
//     }
// }
