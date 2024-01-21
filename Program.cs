using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Добро пожаловать, воин!");
        Console.Write("Как к вам обращаться?: ");
        string playerName = Console.ReadLine();

        Game game = new Game(playerName);
        game.Play();
    }
}

class Game
{
    private Player player;
    private Random rnd;
    private int score;

    public Game(string playerName)
    {
        player = new Player(playerName);
        rnd = new Random();
        score = 0;
    }

    public void Play()
    {
        Console.WriteLine($"Ваше имя {player.Name}!");
        Console.WriteLine($"Вам был ниспослан меч {player.Weapon.Name} ({player.Weapon.Damage}), а также {player.Aid.Name} ({player.Aid.Healing}hp).");
        Console.WriteLine($"У вас {player.GetCurrentHealth()}hp.");

        while (player.GetCurrentHealth() > 0)
        {
            Enemy enemy = Enemy.GenerateRandomEnemy(rnd);
            Console.WriteLine($"{player.Name} Ты встретил вражину по имени {enemy.Name} ({enemy.GetCurrentHealth()}hp), и замечаешь в его руках  {enemy.Weapon.Name} ({enemy.Weapon.Damage})");

            while (enemy.GetCurrentHealth() > 0 && player.GetCurrentHealth() > 0)
            {
                Console.WriteLine("Чё сделаешь?");
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Пропустить ход (зачем?)");
                Console.WriteLine("3. Использовать аптечку");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    int playerDamage = player.Attack();
                    int enemyDamage = enemy.Attack();

                    enemy.UpdateHealth(-playerDamage);
                    player.UpdateHealth(-enemyDamage);

                    Console.WriteLine($"{player.Name} ударил противника {enemy.Name} и нанес {playerDamage} урона");
                    Console.WriteLine($"{enemy.Name} ударил вас и нанес {enemyDamage} урона");
                    Console.WriteLine($"У противника {enemy.Name} осталось {enemy.GetCurrentHealth()}hp, у вас {player.GetCurrentHealth()}hp");
                }
                else if (choice == "2")
                {
                    Console.WriteLine($"{player.Name} пропустил ход");
                    int enemyDamage = enemy.Attack();
                    player.UpdateHealth(-enemyDamage);
                    Console.WriteLine($"{enemy.Name} ударил вас и нанес {enemyDamage} урона");
                    Console.WriteLine($"У противника {enemy.Name} осталось {enemy.GetCurrentHealth()}hp, у вас {player.GetCurrentHealth()}hp");
                }
                else if (choice == "3")
                {
                    player.UseAid();
                    Console.WriteLine($"{player.Name} использовал аптечку");
                    Console.WriteLine($"У противника {enemy.Name} осталось {enemy.GetCurrentHealth()}hp, у вас {player.GetCurrentHealth()}hp");
                }
            }

            if (player.GetCurrentHealth() > 0)
            {
                Console.WriteLine($"Поздравляем! Вы победили врага {enemy.Name} и получили {enemy.Score} очков.");
                score += enemy.Score;
                Console.WriteLine($"У вас теперь {score} очков.");

                if (score >= 50)
                {
                    Console.WriteLine("Поздравляем! Вы достигли 50 очков и прошли игру!");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Игра окончена. Вы помЁрли.");
                break;
            }
        }
    }
}

class Player
{
    public string Name { get; }
    public int MaxHealth { get; }
    protected int CurrentHealth { get; set; }
    public Aid Aid { get; }
    public Weapon Weapon { get; }
    public int Score { get; private set; }

    public Player(string name)
    {
        Name = name;
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        Aid = new Aid("Средняя аптечка", 10);
        Weapon = new Weapon("Меч Феникса", 20);
        Score = 0;
    }

    public int Attack()
    {
        return Weapon.Damage;
    }

    public void UseAid()
    {
        CurrentHealth = Math.Min(MaxHealth, CurrentHealth + Aid.Healing);
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void UpdateHealth(int value)
    {
        CurrentHealth += value;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }
}

class Enemy
{
    public string Name { get; }
    public int MaxHealth { get; }
    protected int CurrentHealth { get; set; }
    public Weapon Weapon { get; }
    public int Score { get; }

    private static readonly List<string> EnemyNames = new List<string> { "Демонический Эльф", "Гоблин", "Демон", "Некромант", "Огр (похож на Шрека)" };

    public Enemy(string name, int maxHealth, Weapon weapon, int score)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        Weapon = weapon;
        Score = score;
    }

    public int Attack()
    {
        return Weapon.Damage;
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void UpdateHealth(int value)
    {
        CurrentHealth += value;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }

    public static Enemy GenerateRandomEnemy(Random rnd)
    {
        string name = EnemyNames[rnd.Next(0, EnemyNames.Count)];
        int maxHealth = rnd.Next(40, 80);

        List<Weapon> weapons = new List<Weapon>
        {
            new Weapon("Острый меч", 10),
            new Weapon("Меч Феникса", 20),
            new Weapon("Лук Богини войны", 15)
        };
        Weapon weapon = weapons[rnd.Next(weapons.Count)];

        int score = rnd.Next(5, 15);

        return new Enemy(name, maxHealth, weapon, score);
    }
}

class Aid
{
    public string Name { get; }
    public int Healing { get; }

    public Aid(string name, int healing)
    {
        Name = name;
        Healing = healing;
    }
}

class Weapon
{
    public string Name { get; }
    public int Damage { get; }

    public Weapon(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }
}

