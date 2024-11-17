using System.Collections;
using UnityEngine;

namespace GameLogic
{
    public class Dice : MonoBehaviour
    {
        // Event déclenché quand le dé termine son lancer
        public delegate void DiceRolled(int result);
        public event DiceRolled OnDiceRolled;

        private Sprite[] diceSides;
        private SpriteRenderer rend;

        private int diceResult = 0;

        private void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        }

        public void Roll()
        {
            StartCoroutine(RollTheDice());
        }

        private IEnumerator RollTheDice()
        {
            int randomDiceSide = 0;

            // Animation: afficher différentes faces de manière aléatoire
            for (int i = 0; i <= 20; i++)
            {
                randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.1f);
            }

            // Résultat final
            diceResult = randomDiceSide + 1;

            // Afficher la face finale
            rend.sprite = diceSides[randomDiceSide];

            // Déclencher l'événement avec le résultat
            OnDiceRolled?.Invoke(diceResult);
        }

        public int GetDiceResult()
        {
            return diceResult; // Permet de récupérer le résultat si nécessaire
        }
    }
}
