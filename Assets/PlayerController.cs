using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool canPlay = false;

    // Méthode appelée pour activer les contrôles du joueur
    public void EnableControls()
    {
        canPlay = true;
        Debug.Log(gameObject.name + " peut jouer maintenant.");
    }

    // Méthode appelée pour désactiver les contrôles du joueur
    public void DisableControls()
    {
        canPlay = false;
        Debug.Log(gameObject.name + " ne peut plus jouer.");
    }

    private void Update()
    {
        // Exemple : chaque joueur peut tourner autour de l'axe Y s'il a le droit de jouer
        if (canPlay)
        {
            transform.Rotate(0, 50 * Time.deltaTime, 0);
        }
    }
}
