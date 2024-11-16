namespace GameLogic
{
    public class Dice
    {

        public int Roll(int diceCount)
        {
            int total = 0;
            for (int i = 0; i < diceCount; i++)
            {
                total += UnityEngine.Random.Range(1, 7); // Lancer de dé entre 1 et 6
            }
            return total;
        }
    }
}
