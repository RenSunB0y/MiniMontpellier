using UnityEngine;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public TMP_InputField[] playerInputFields; // Tableau des champs d'entrée pour les pseudos
    public int minPlayers = 2; // Nombre minimum de joueurs
    public int maxPlayers = 4; // Nombre maximum de joueurs

    void Start()
    {
        // Assurez-vous que PlayerData contient un nombre d'éléments correct pour les joueurs
        PlayerData.PlayerNames.Clear(); // Réinitialiser les noms avant de les ajouter
    }

    public void StartGame()
    {
        // Réinitialiser la liste des joueurs
        PlayerData.PlayerNames.Clear();

        // Vérifiez chaque InputField et ajoutez les pseudos
        foreach (var inputField in playerInputFields)
        {
            // Ne rajoutez un joueur que si l'input est rempli
            if (!string.IsNullOrEmpty(inputField.text))
            {
                // Limiter le nombre de joueurs à 4
                if (PlayerData.PlayerNames.Count < maxPlayers)
                {
                    PlayerData.PlayerNames.Add(inputField.text); // Ajouter le nom du joueur à la liste
                }
            }
        }

        // Vérification de l'état après l'ajout des joueurs
        Debug.Log($"Nombre de joueurs : {PlayerData.PlayerNames.Count}");

        // Vérifiez que le nombre de joueurs est compris entre 2 et 4
        if (PlayerData.PlayerNames.Count >= minPlayers && PlayerData.PlayerNames.Count <= maxPlayers)
        {
            // Charger la scène de jeu si le nombre de joueurs est valide
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
        }
        else
        {
            // Afficher un message d'erreur si le nombre de joueurs est insuffisant ou trop élevé
            Debug.LogError($"Nombre de joueurs invalide. Vous devez avoir entre {minPlayers} et {maxPlayers} joueurs.");
        }
    }
}
