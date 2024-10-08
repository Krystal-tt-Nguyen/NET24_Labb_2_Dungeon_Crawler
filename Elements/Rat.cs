internal class Rat : Enemy
{
    private Random numberGenerator = new Random();

    public Rat(int x, int y) : 
        base(x, y, 'r', ConsoleColor.Red, 10, "Rat", new Dice (1,6,3), new Dice(1,6,1) ) {}

    public override void Update() => Move();

    private void Move()
    {
        originalPosition = new Position() { X = Position.X, Y = Position.Y };
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(' ');

        int direction = numberGenerator.Next(0, 4);
        switch (direction)
        {
            case 0:
                Position.Y--;
                break;
            case 1:
                Position.Y++;
                break;
            case 2:
                Position.X--;
                break;
            case 3:
                Position.X++;
                break;
        }

        CheckPositionAvailability();
    }

}