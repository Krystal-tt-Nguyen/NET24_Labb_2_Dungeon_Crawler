internal class Dice
{
    private Random Random = new Random();
    private int NumberOfDice { get; set; }
    private int SidesPerDice { get; set; }
    private int Modifyer { get; set; }

    public Dice(int numberOfDice, int sidesPerDice, int modifier)
    {
        NumberOfDice = numberOfDice;
        SidesPerDice = sidesPerDice;
        Modifyer = modifier;
    }

    public int Throw()
    {
        int totalPoints = 0;

        for (int i = 0; i < NumberOfDice; i++)
        {
            totalPoints += Random.Next(1, SidesPerDice + 1);
        }

        return totalPoints + Modifyer;
    }

    public override string ToString() => $"{NumberOfDice}d{SidesPerDice}+{Modifyer}";

}