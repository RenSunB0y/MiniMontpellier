using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
     // Liste des joueurs
    public List<GameObject> players;
    private int currentPlayerIndex = 0;  // Index du joueur actuel

    // Initialisation
    private void Start()
    {
        if (players.Count == 0)
        {
            Debug.LogError("Aucun joueur n'a été ajouté à la liste des joueurs.");
            return;
        }

        // Début du tour pour le premier joueur
        StartTurn(players[currentPlayerIndex]);
    }

    // Passer au joueur suivant
    public void NextTurn()
    {
        EndTurn(players[currentPlayerIndex]);

        // Passer à l'index du joueur suivant, revenir au début si on atteint la fin de la liste
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        // Début du tour pour le joueur suivant
        StartTurn(players[currentPlayerIndex]);
    }

    // Début du tour d'un joueur
    private void StartTurn(GameObject player)
    {
        Debug.Log("C'est le tour de " + player.name);
        // Activer les contrôles du joueur
        player.GetComponent<PlayerController>().EnableControls();
    }

    // Fin du tour d'un joueur
    private void EndTurn(GameObject player)
    {
        Debug.Log(player.name + " a terminé son tour.");
        // Désactiver les contrôles du joueur
        player.GetComponent<PlayerController>().DisableControls();
    }
    
    private void Update()
    {
        // Passe au joueur suivant en appuyant sur la touche Espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextTurn();
        }
    }

}
