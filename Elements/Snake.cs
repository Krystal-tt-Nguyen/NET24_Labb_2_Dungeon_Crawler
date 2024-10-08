internal class Snake : Enemy
{
    public Snake(int x, int y) : 
        base ( x, y, 's', ConsoleColor.Green, 25, "Snake", new Dice (3,4,2), new Dice (1,8,5)) {}

    public override void Update() => Move();

    private void Move()
    {
        originalPosition = new Position() { X = Position.X, Y = Position.Y };
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(' ');

        foreach (var element in LevelData.Elements)
        {
            if (element is Player player)
            {
                double distanceToPlayer = player.CalculateDistanceToPlayer(this, player);

                if (distanceToPlayer <= 2)
                {
                    if (player.Position.Y > Position.Y && player.Position.X == Position.X)
                    {
                        Position.Y--;
                    }
                    else if (player.Position.Y < Position.Y && player.Position.X == Position.X)
                    {
                        Position.Y++;
                    }
                    else if (player.Position.X > Position.X)
                    {
                        Position.X--;
                    }
                    else if (player.Position.X < Position.X)
                    {
                        Position.X++;
                    }
                }
            }
        }

        CheckPositionAvailability();
    }

}