internal class LevelData
{
    private static List<LevelElement> elements = new ();
    public static List<LevelElement> Elements { get { return elements; } }
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
                        Player = new Player(x, y);
                        elements.Add(Player);
                        break;
                    case '#':
                        elements.Add(new Wall(x, y)); 
                        break;
                    case 'r':
                        elements.Add(new Rat(x, y));
                        break;
                    case 's':
                        elements.Add(new Snake(x, y));
                        break;
                }
            }
            y++;
        }
    }

}