internal class GameLoop
{
    public void PlayGame()
    {
        LevelData levelData = new LevelData();
        string filename = @"Level\Level1.txt";
        levelData.Load(filename);

        while (levelData.Player.isPlayerAlive)
        {
            Console.SetCursorPosition(0, 17);
            levelData.PrintPlayerStatus();

            levelData.Player.Draw();
            levelData.Player.Update();

            foreach (var element in levelData.Elements)
            {
                if (element is Wall wall && levelData.Player.CalculateDistance(wall, levelData.Player) <= 5)
                {
                    wall.Draw();
                }
                else if (element is Enemy enemy)
                {
                    enemy.Update();
                    
                    double distanceToPlayer = levelData.Player.CalculateDistance(enemy, levelData.Player);
                    bool isEnemyVisible = enemy.IsVisible(distanceToPlayer);

                    if (isEnemyVisible)
                    {
                        enemy.Draw(); 
                    }
                }
            }

            if (levelData.Player.HealthPoints <= 0)
            {
                Console.Clear();
                Console.WriteLine("You died! GAME OVER");
            }
        }
    }
}
