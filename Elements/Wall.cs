internal class Wall : LevelElement
{
    public Wall(int x, int y) : base(new Position() { X = x, Y = y }, '#', ConsoleColor.Gray) {}

}