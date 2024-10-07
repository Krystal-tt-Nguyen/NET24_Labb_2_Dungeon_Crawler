using System;
using System.Xml.Linq;

internal class GameLoop
{
    public void PlayGame()
    {
        LevelData levelData = new LevelData();
        string filename = @"Level\Level1.txt";
        levelData.Load(filename);

        foreach (var element in levelData.Elements)
        {
            element.Draw();
        }

        while (levelData.Player.isPlayerAlive)
        {
            levelData.Player.Update();

            foreach (var element in levelData.Elements)
            {
                if (element is Rat rat)
                {
                    rat.Update();
                }
                else if (element is Snake snake)
                {
                    snake.Update();
                }
            }
            
            levelData.PrintPlayerStatus();

            if (levelData.Player.HealthPoints <= 0)
            {
                Console.Clear();
                Console.WriteLine("You died! GAME OVER");
            }
        }
    }
}


//public void PlayGame()
//{
//    LevelData levelData = new LevelData();
//    string filename = @"Level\Level1.txt";
//    levelData.Load(filename);

//    foreach (var element in levelData.Elements)
//    {
//        element.Draw();
//    }

//    while (levelData.Player.isPlayerAlive)
//    {
//        levelData.Player.Update();

//        foreach (var element in levelData.Elements)
//        {
//            if (element is Rat rat)
//            {
//                rat.Update();
//            }
//            else if (element is Snake snake)
//            {
//                snake.Update();
//            }
//        }

//        levelData.PrintPlayerStatus();

//        if (levelData.Player.HealthPoints <= 0)
//        {
//            Console.Clear();
//            Console.WriteLine("You died! GAME OVER");
//        }
//    }
//}
//}