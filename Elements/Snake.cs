internal class Snake : Enemy
{
    public Snake(LevelData levelData, int x, int y) : 
        base (levelData, x, y, 's', ConsoleColor.Green, 25, "Snake", new Dice (3,4,2), new Dice (1,8,5)) {}

    public override void Update()
    {
        Move();
        Draw();
    }

    protected override void Move()
    {
        base.Move();

        foreach (var element in levelData.Elements)
        {
            if (element is Player player)
            {
                int deltaX = Position.X - player.Position.X;
                int deltaY = Position.Y - player.Position.Y;
                double distanceToPlayer = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

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