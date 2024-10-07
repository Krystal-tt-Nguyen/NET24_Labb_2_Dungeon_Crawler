internal class LevelData
{
    private List<LevelElement> elements = new ();
    public IReadOnlyList<LevelElement> Elements => elements.AsReadOnly();
    public Player Player { get; private set; }


    public void Load(string filename)
    {
        using StreamReader reader = new StreamReader(filename);
        
        int y = 0;

        while (!reader.EndOfStream)
        {
            string currentLine = reader.ReadLine();

            for (int x = 0; x < currentLine.Length; x++)
            {
                char currentChar = currentLine[x];

                switch (currentChar)
                {
                    case '@':
                        Player = new Player(this, x, y);
                        elements.Add(Player);
                        break;
                    case '#':
                        elements.Add(new Wall(/*this*/ x, y));  // behövs ev. till VisionRange
                        break;
                    case 'r':
                        elements.Add(new Rat(this, x, y));
                        break;
                    case 's':
                        elements.Add(new Snake(this, x, y));
                        break;
                }
            }
            y++;
        }
    }

    public void RemoveEnemy(LevelElement enemy)
    {
        // "Göm", rita över enemy som dött
        Console.SetCursorPosition(enemy.Position.X, enemy.Position.Y);
        Console.WriteLine(' ');

        // Ta bort enemy från listan Elements, annars ritas de ut igen när enemy.Update() körs i gameloop
        elements.Remove(enemy);
    }

    public void PrintPlayerStatus()
    {
        Console.WriteLine($"\n\n{Player.Name}: Krystal \t Health: {Player.HealthPoints} \t Turns: {Player.Turns}");
    }

}


