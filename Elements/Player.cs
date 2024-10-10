internal class Player : MovingElements
{
    public int Turns { get; set; }
    public Player(int x, int y) :
        base(x, y, '@', ConsoleColor.Yellow, 100, "Player", new Dice(2, 6, 2), new Dice(2, 6, 0)) { }

    public void Update()
    {
        Move();
        Draw();
    }

    private void Move()
    {
        Console.CursorVisible = false;
        ConsoleKey keyInfo = Console.ReadKey(true).Key;

        OriginalPosition = new Position() { X = Position.X, Y = Position.Y };
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(' ');

        switch (keyInfo)
        {
            case ConsoleKey.UpArrow:
                Position.Y--;
                break;
            case ConsoleKey.DownArrow:
                Position.Y++;
                break;
            case ConsoleKey.LeftArrow:
                Position.X--;
                break;
            case ConsoleKey.RightArrow:
                Position.X++;
                break;
            case ConsoleKey.Enter:
                break;
        }

        Turns++;
        CheckPositionAvailability();
    }

    public double CalculateDistanceToPlayer(LevelElement element, Player player)
    {
        int deltaX = element.Position.X - player.Position.X;
        int deltaY = element.Position.Y - player.Position.Y;
        double distanceToPlayer = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));

        return distanceToPlayer;
    }

    public bool IsPlayerAlive(int playerHealthPoints)
    {
        if (playerHealthPoints > 0)
        {
            return true;
        }
        return false;
    }

}