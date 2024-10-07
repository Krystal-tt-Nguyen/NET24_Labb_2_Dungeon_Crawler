abstract class MovingElements : LevelElement
{
    public int HealthPoints { get; set; }
    public string Name { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    protected readonly LevelData levelData;
    protected Position originalPosition;
    public bool isPlayerAlive = true;
    
    public List<(string message, ConsoleColor color)> battleStatuses = new();
    public List<int> statusesToRemove = new();
    
    protected MovingElements (LevelData levelData, int x, int y, char symbol, ConsoleColor color, int healthPoints, string name, Dice attackDice, Dice defenceDice): 
        base (new Position() { X = x, Y = y }, symbol, color) 
    {
        HealthPoints = healthPoints;
        Name = name;
        AttackDice = attackDice;
        DefenceDice = defenceDice;
        this.levelData = levelData;
    }


    public abstract void Update();

    protected virtual void Move()
    {
        originalPosition = new Position() { X = Position.X, Y = Position.Y };

        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(' ');

        // Logik för olika förflyttningsmönster finns i respektive klass.
    }

    protected void CheckPositionAvailability ()
    {
        bool isPositionAvailable = true;

        // Kolla om ny position är tillgänglig (ska ej kunna gå på varandra). Uppdatera Position
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

        // Om ny position är ockuperad, återgå till OriginalPosition
        if (!isPositionAvailable)
        {
            Position = originalPosition;
        }
    }

    protected void CheckElementsToBattle(LevelElement element)
    {
        MovingElements attacker = this;                     // this = den som anropar metoden
        MovingElements defender = (MovingElements)element;  // elementet = det element som har samma position 

        // Om Player anropar metoden: Player utför attack på Enemy 
        if (attacker is Player && (defender is Rat || defender is Snake))
        {
            Battle(attacker, defender);
        }
        // Om Enemy anropar metoden: Enemy utför attack på Player
        else if ((attacker is Rat || attacker is Snake) && defender is Player)
        {
            Battle(attacker, defender);
        }
    }

    protected void Battle (MovingElements attacker, MovingElements defender)
    {
        PerformAttack(attacker, defender);

        // Om Players HealthPoints <= 0, avsluta spelet
        if (defender is Player && defender.HealthPoints <= 0)
        {
            isPlayerAlive = false;
        }
        // Om Enemys HealthPoints <= 0, ta bort från spelplanen
        else if (defender is Enemy enemy && defender.HealthPoints <= 0)
        {
            levelData.RemoveEnemy(enemy);
        }
        // Annars, gör motattack 
        else
        {
            PerformAttack(defender, attacker);
        }

        ClearStatusMessage();
        PrintStatusMessage();
    }

    protected void PerformAttack(MovingElements attacker, MovingElements defender)
    {
        int attackPoints = attacker.AttackDice.Throw();
        int defencePoints = defender.DefenceDice.Throw();
        int damage = attackPoints - defencePoints;

        // Om damage > 0, dra av motsvarande från HealthPoints.
        // Annars om damage < 0, inget händer - defenders tur att göra motattack
        if (attackPoints > defencePoints)
        {
            defender.HealthPoints -= damage;
        }

        // OBS! Måste spara båda meddelande, (ATK-DEF, DEF-ATK), i en lista.
        // Tidigare misstag: sparade status-texten direkt i battleStatuses -> endast det senaste statusen skrevs ut i konsollfönstret (status för DEF-ATK skrev över status för ATK-DEF)
        battleStatuses.Add(CheckBattleStatusMessage(attacker, defender, attackPoints, defencePoints, damage));
    }

    protected (string, ConsoleColor) CheckBattleStatusMessage(MovingElements attacker, MovingElements defender, int attackPoints, int defencePoints, int damage)
    {
        // Använd tuple (datatyp som kan spara värden av olika datatyper) för att spara två värden knutna till varandra: vad som ska skrivas ut (message) och färg (color).
        string message = $"{attacker.Name} ATK ({attacker.AttackDice} => {attackPoints}) attacked {defender.Name} DEF ({defender.DefenceDice} => {defencePoints}), ";
        ConsoleColor messageColor = ConsoleColor.Gray;

        switch (defender)
        {
            case Enemy: // attacker = player
                if (defender.HealthPoints <= 0) { message += "killing it instantly."; }
                else if (damage <= 0) { message += "but did not manage to make any damage."; messageColor = ConsoleColor.Green; }
                else if (damage > 0 && damage <= 3) { message += $"slightly wounding it."; messageColor = ConsoleColor.Yellow; }
                else if (damage > 3 && damage <= 7) { message += $"moderately wounding it."; messageColor = ConsoleColor.Red; }
                else if (damage > 7) { message += $"severely wounding it."; messageColor = ConsoleColor.DarkRed; }
                break;
            case Player: // attacker = enemy
                if (defender.HealthPoints <= 0) { message += "killing YOU instantly. GAME OVER!"; }
                else if (damage <= 0) { message += "but did not manage to make any damage."; messageColor = ConsoleColor.Green; }
                else if (damage > 0 && damage <= 3) { message += $"slightly wounding you."; messageColor = ConsoleColor.Yellow; }
                else if (damage > 3 && damage <= 7) { message += $"moderately wounding you."; messageColor = ConsoleColor.Red; }
                else if (damage > 7) { message += $"severely wounding you."; messageColor = ConsoleColor.DarkRed; }
                break;
        }
        return (message, messageColor);
    }

    protected void PrintStatusMessage()
    {
        int i = 0;

        // Varje element i List<> battleStatuses består av en tuple med två egenskaper: message och color.
        foreach (var status in battleStatuses)
        {
            Console.SetCursorPosition(0, 20 + i);
            Console.ForegroundColor = status.color;
            Console.WriteLine(status.message);
            Console.ResetColor();
            i++;

            // Spara positionen för raden där meddelandet skrevs
            statusesToRemove.Add(Console.CursorTop - 1);
        }

        // Rensa listan, annars skrivs alla status ut på rad. Vill endast visa de två senaste!
        battleStatuses.Clear();
    }

    protected void ClearStatusMessage()
    {
        // Använd nästlade if-satser, ej for-loop som kör i < 2.
        // Detta för att i vissa omgångar skrivs endast en status ut, ex. när enemy dör - de gör ingen motattack
        // --> felmeddelande i forloop: kan inte loopa 2ggr när listan innehåller ett element
        // --> använd Bufferwidth = bredden på konsolfönstret.
        if (statusesToRemove.Count > 0)
        {
            // Rensa den senaste raden
            Console.SetCursorPosition(0, statusesToRemove[statusesToRemove.Count - 1]);
            Console.Write("".PadRight(Console.BufferWidth));

            // Om det finns minst två rader, rensa den näst sista också
            if (statusesToRemove.Count > 1)
            {
                Console.SetCursorPosition(0, statusesToRemove[statusesToRemove.Count - 2]);
                Console.Write(" ".PadRight(Console.BufferWidth));
            }
        }

        // Gå tillbaka till raden där första statusmeddelandet skrevs ut
        Console.SetCursorPosition(0, 20);
    }

}

