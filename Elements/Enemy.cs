abstract class Enemy : MovingElements
{
    protected Enemy(LevelData levelData,int x, int y, char symbol, ConsoleColor color, int healthPoints, string name, Dice attackDice, Dice defenceDice): 
        base (levelData, x, y, symbol, color, healthPoints, name, attackDice, defenceDice) {}

    public override void Update() { }

}