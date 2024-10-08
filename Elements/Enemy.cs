abstract class Enemy : MovingElements
{
    protected Enemy(int x, int y, char symbol, ConsoleColor color, int healthPoints, string name, Dice attackDice, Dice defenceDice): 
        base (x, y, symbol, color, healthPoints, name, attackDice, defenceDice) {}

    public abstract void Update();

    public bool IsInsideVisionRange(double distanceToPlayer, int visionRange)
    {
        if (distanceToPlayer <= visionRange)
        {
           return true;
        }
        return false;
    }

}