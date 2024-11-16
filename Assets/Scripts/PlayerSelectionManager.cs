using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Ajoutez cette directive pour charger la scène

public class PlayerSelectionManager : MonoBehaviour
{
    public TMP_InputField[] playerInputFields; // Tableau des champs d'entrée pour les pseudos

    // Cette méthode sera appelée lorsque l'utilisateur valide la sélection des joueurs
    public void StartGame()
    {
        // On s'assure que PlayerNames est réinitialisé avant d'ajouter de nouveaux noms
        PlayerData.PlayerNames.Clear();

        // On ajoute les noms des joueurs si les champs ne sont pas vides
        foreach (var inputField in playerInputFields)
        {
            if (!string.IsNullOrEmpty(inputField.text))
            {
                PlayerData.PlayerNames.Add(inputField.text); // Ajouter le pseudo dans PlayerData
            }
        }

        // Charger la scène de jeu
        SceneManager.LoadScene("YvannDevScene");
    }
}
