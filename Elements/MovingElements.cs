﻿abstract class MovingElements : LevelElement
{
    public int HealthPoints { get; set; }
    public string Name { get; set; }
    private Dice AttackDice { get; set; }
    private Dice DefenceDice { get; set; }
    protected Position OriginalPosition { get; set; }


    private List<(string message, ConsoleColor color)> battleMessages = new();

    protected MovingElements(int x, int y, char symbol, ConsoleColor color, int healthPoints, string name, Dice attackDice, Dice defenceDice) :
        base(new Position() { X = x, Y = y }, symbol, color)
    {
        HealthPoints = healthPoints;
        Name = name;
        AttackDice = attackDice;
        DefenceDice = defenceDice;
    }


    protected void CheckPositionAvailability()
    {
        bool isPositionAvailable = true;

        foreach (var element in LevelData.Elements)
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
            Position = OriginalPosition;
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

    private void Battle(MovingElements attacker, MovingElements defender)
    {
        PerformAttack(attacker, defender);

        if (defender is Enemy enemy && defender.HealthPoints <= 0)
        {
            RemoveEnemy(enemy);
        }
        else
        {
            PerformAttack(defender, attacker);
        }

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

    private void RemoveEnemy(MovingElements deadEnemy)
    {
        Console.SetCursorPosition(deadEnemy.Position.X, deadEnemy.Position.Y);
        Console.WriteLine(' ');
        LevelData.Elements.Remove(deadEnemy);
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
            Console.SetCursorPosition(0, 1 + i);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 1 + i);

            Console.ForegroundColor = message.color;
            Console.WriteLine(message.message);
            Console.ResetColor();
            i++;
        }
        
        battleMessages.Clear();
    }

}