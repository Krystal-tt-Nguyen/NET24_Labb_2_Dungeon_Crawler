abstract class MovingElements : LevelElement
{
    public int HealthPoints { get; set; }
    public string Name { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    protected readonly LevelData levelData;
    protected Position originalPosition;
    public bool isPlayerAlive = true;
   
    private List<(string message, ConsoleColor color)> battleMessages = new();
    private List<int> messagesToRemove = new();
    
    protected MovingElements (LevelData levelData, int x, int y, char symbol, ConsoleColor color, int healthPoints, string name, Dice attackDice, Dice defenceDice): 
        base (new Position() { X = x, Y = y }, symbol, color) 
    {
        HealthPoints = healthPoints;
        Name = name;
        AttackDice = attackDice;
        DefenceDice = defenceDice;
        this.levelData = levelData;
    }

    protected void CheckPositionAvailability ()
    {
        bool isPositionAvailable = true;

        foreach (var element in levelData.Elements)
        {
            if (element is Wall && (Position.X == element.Position.X && Position.Y == element.Position.Y))
            {
                isPositionAvailable = false;
                break;
            }
            else if (element is MovingElements && (element != this) && (Position.X == element.Position.X && Position.Y == element.Position.Y))
            {
                isPositionAvailable = false;
                CheckElementsToBattle(element);
                break;
            }
        }

        if (!isPositionAvailable)
        {
            Position = originalPosition;
        }
    }

    private void CheckElementsToBattle(LevelElement element)
    {
        MovingElements attacker = this;
        MovingElements defender = (MovingElements)element;

        if (attacker is Player && (defender is Rat || defender is Snake))
        {
            Battle(attacker, defender);
        }
        else if ((attacker is Rat || attacker is Snake) && defender is Player)
        {
            Battle(attacker, defender);
        }
    }

    private void Battle (MovingElements attacker, MovingElements defender)
    {
        PerformAttack(attacker, defender);

        if (defender is Player && defender.HealthPoints <= 0)
        {
            isPlayerAlive = false;
        }
        else if (defender is Enemy enemy && defender.HealthPoints <= 0)
        {
            levelData.RemoveEnemy(enemy);
        }
        else
        {
            PerformAttack(defender, attacker);
        }

        ClearMessage();
        PrintMessage();
    }

    private void PerformAttack(MovingElements attacker, MovingElements defender)
    {
        int attackPoints = attacker.AttackDice.Throw();
        int defencePoints = defender.DefenceDice.Throw();
        int damage = attackPoints - defencePoints;

        if (attackPoints > defencePoints)
        {
            defender.HealthPoints -= damage;
        }

        battleMessages.Add(BattleMessage(attacker, defender, attackPoints, defencePoints, damage));
    }

    private (string, ConsoleColor) BattleMessage(MovingElements attacker, MovingElements defender, int attackPoints, int defencePoints, int damage)
    {
        string message = $"{attacker.Name} ATK ({attacker.AttackDice} => {attackPoints}) attacked {defender.Name} DEF ({defender.DefenceDice} => {defencePoints}), ";
        ConsoleColor messageColor = ConsoleColor.Gray;

        switch (defender)
        {
            case Enemy:
                if (defender.HealthPoints <= 0) { message += "killing it instantly."; }
                else if (damage <= 0) { message += "but did not manage to make any damage."; messageColor = ConsoleColor.Green; }
                else if (damage > 0 && damage <= 3) { message += $"slightly wounding it."; messageColor = ConsoleColor.Yellow; }
                else if (damage > 3 && damage <= 7) { message += $"moderately wounding it."; messageColor = ConsoleColor.Red; }
                else if (damage > 7) { message += $"severely wounding it."; messageColor = ConsoleColor.DarkRed; }
                break;
            case Player:
                if (defender.HealthPoints <= 0) { message += "killing YOU instantly. GAME OVER!"; }
                else if (damage <= 0) { message += "but did not manage to make any damage."; messageColor = ConsoleColor.Green; }
                else if (damage > 0 && damage <= 3) { message += $"slightly wounding you."; messageColor = ConsoleColor.Yellow; }
                else if (damage > 3 && damage <= 7) { message += $"moderately wounding you."; messageColor = ConsoleColor.Red; }
                else if (damage > 7) { message += $"severely wounding you."; messageColor = ConsoleColor.DarkRed; }
                break;
        }
        return (message, messageColor);
    }

    private void PrintMessage()
    {
        int i = 0;
        foreach (var message in battleMessages)
        {
            Console.SetCursorPosition(0, 20 + i);
            Console.ForegroundColor = message.color;
            Console.WriteLine(message.message);
            Console.ResetColor();
            i++;

            messagesToRemove.Add(Console.CursorTop - 1);
        }

        battleMessages.Clear();
    }

    private void ClearMessage()
    {
        for (int i = 0; i < messagesToRemove.Count; i++)
        {
            Console.SetCursorPosition(0, messagesToRemove[messagesToRemove.Count - 1 - i]);
            Console.Write(" ".PadRight(Console.BufferWidth));
        }
        Console.SetCursorPosition(0, 20);
    }

    public double CalculateDistance(LevelElement element, Player player)
    {
        int deltaX = element.Position.X - player.Position.X;
        int deltaY = element.Position.Y - player.Position.Y;
        double distanceToPlayer = Math.Sqrt( (deltaX * deltaX) + (deltaY * deltaY));
       
        return distanceToPlayer;
    }

}