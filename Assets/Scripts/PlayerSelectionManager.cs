using UnityEngine;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public TMP_InputField[] playerInputFields; // Array des champs d'entrée pour les pseudos

    public void StartGame()
    {
        PlayerData.PlayerNames.Clear(); // Nettoyer les noms avant d'ajouter
        foreach (var inputField in playerInputFields)
        {
            if (!string.IsNullOrEmpty(inputField.text))
            {
                PlayerData.PlayerNames.Add(inputField.text);
            }
        }

        // Charger la scène de jeu
        UnityEngine.SceneManagement.SceneManager.LoadScene("YvanDevScene");
    }
}
