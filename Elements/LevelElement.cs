abstract class LevelElement
{
    public Position Position { get; set; }
    protected char Symbol { get; set; }
    protected ConsoleColor Color { get; set; }

    protected LevelElement(Position position, char symbol, ConsoleColor color)
    {
        Position = position;
        Symbol = symbol;
        Color = color;
    }

    public void Draw()
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(Position.X,Position.Y);
        Console.Write(Symbol);
        Console.ResetColor();
    }

}