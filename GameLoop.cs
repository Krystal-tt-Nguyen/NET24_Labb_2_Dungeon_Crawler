internal class GameLoop
{
    public void PlayGame()
    {
        LevelData levelData = new LevelData();
        string filename = @"Level\Level1.txt";
        levelData.Load(filename);

        bool isPlayerAlive = true;
        int visionRange = 5;
        while (isPlayerAlive)
        {
            Console.SetCursorPosition(0, 19);
            Console.WriteLine($"{levelData.Player.Name}: Krystal \t Health: {levelData.Player.HealthPoints} \t Turns: {levelData.Player.Turns}");

            levelData.Player.Draw();
            levelData.Player.Update();

            foreach (var element in LevelData.Elements)
            {
                if (element is Wall wall && levelData.Player.CalculateDistanceToPlayer(wall, levelData.Player) <= visionRange)
                {
                    wall.Draw();
                }
                else if (element is Enemy enemy)
                {
                    enemy.Update();
                   
                    double distanceToPlayer = levelData.Player.CalculateDistanceToPlayer(enemy, levelData.Player);
                    bool isEnemyInsideVisionRange = enemy.IsInsideVisionRange(distanceToPlayer, visionRange);

                    if (isEnemyInsideVisionRange && enemy.HealthPoints >= 0)
                    {
                        enemy.Draw(); 
                    }

                    isPlayerAlive = levelData.Player.IsPlayerAlive(levelData.Player.HealthPoints);
                }
            }
        }

        if (!isPlayerAlive)
        {
            Thread.Sleep(2000);
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You died, GAME OVER!");
            Console.ResetColor();
        }
    }
}
