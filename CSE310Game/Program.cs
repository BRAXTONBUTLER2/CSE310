using System;

namespace MedievalAdventureGame
{
    struct Stats
    {
        public int Health;
        public int Attack;

        public Stats(int health, int attack)
        {
            Health = health;
            Attack = attack;
        }
    }

    abstract class Enemy
    {
        public string Name { get; protected set; }
        public Stats EnemyStats;

        public Enemy(string name, int health, int attack)
        {
            Name = name;
            EnemyStats = new Stats(health, attack);
        }

        public abstract void AttackPlayer(Player player);

        public bool IsAlive()
        {
            return EnemyStats.Health > 0;
        }
    }

    class Goblin : Enemy
    {
        public Goblin() : base("Goblin", 30, 5) { }

        public override void AttackPlayer(Player player)
        {
            Console.WriteLine("The Goblin slashes you with its dagger!");
            player.PlayerStats.Health -= EnemyStats.Attack;
        }
    }

    class Skeleton : Enemy
    {
        public Skeleton() : base("Skeleton", 40, 7) { }

        public override void AttackPlayer(Player player)
        {
            Console.WriteLine("The Skeleton swings its bony sword!");
            player.PlayerStats.Health -= EnemyStats.Attack;
        }
    }

    class Zombie : Enemy
    {
        public Zombie() : base("Zombie", 50, 6) { }

        public override void AttackPlayer(Player player)
        {
            Console.WriteLine("The Zombie bites you with rotten teeth!");
            player.PlayerStats.Health -= EnemyStats.Attack;
        }
    }

    class Dragon : Enemy
    {
        public Dragon() : base("Dragon", 100, 15) { }

        public override void AttackPlayer(Player player)
        {
            // Show the fire breathing ASCII art
            Console.WriteLine(@"
(  )   /\   _                 (     
\ |  (  \ ( \.(               )                      _____
\  \ \  `  `   ) \             (  ___                 / _   \
(_`    \+   . x  ( .\            \/   \____-----------/ (o)   \_
- .-               \+  ;          (  O                           \____
                          )        \_____________  `              \  /
(__                +- .( -'.- <. - _  VVVVVVV VV V\                 \/
(_____            ._._: <_ - <- _  (--  _AAAAAAA__A_/                  |
  .    /./.+-  . .- /  +--  - .     \______________//_              \_______
  (__ ' /x  / x _/ (                                  \___'          \     /
 , x / ( '  . / .  /                                      |           \   /
    /  /  _/ /    +                                      /              \/
   '  (__/                                             /                  \");

            Console.WriteLine("🔥 The Dragon breathes fire at you!");
            player.PlayerStats.Health -= EnemyStats.Attack;
        }
    }

    class Player
    {
        public Stats PlayerStats;
        public int MaxHealth = 100;
        public int Potions = 0;
        public int AttackBoosts = 0;

        public Player(int health, int attack)
        {
            PlayerStats = new Stats(health, attack);
        }

        public bool IsAlive()
        {
            return PlayerStats.Health > 0;
        }

        public void AttackEnemy(Enemy enemy)
        {
            Console.WriteLine($"You attack the {enemy.Name} for {PlayerStats.Attack} damage!");
            enemy.EnemyStats.Health -= PlayerStats.Attack;
        }

        public void HealAfterBattle(int amount)
        {
            int oldHealth = PlayerStats.Health;
            PlayerStats.Health += amount;

            if (PlayerStats.Health > MaxHealth)
                PlayerStats.Health = MaxHealth;

            Console.WriteLine($"🩹 You healed for {PlayerStats.Health - oldHealth} HP. Current Health: {PlayerStats.Health}");
        }

        public void UsePotion()
        {
            if (Potions > 0)
            {
                Potions--;
                int healAmount = 30;
                PlayerStats.Health += healAmount;
                if (PlayerStats.Health > MaxHealth)
                    PlayerStats.Health = MaxHealth;
                Console.WriteLine($"🍗 You used a potion and healed {healAmount} HP! Current Health: {PlayerStats.Health}");
            }
            else
            {
                Console.WriteLine("❌ You have no potions left!");
            }
        }

        public void ApplyItemDrop()
        {
            Random rand = new Random();
            int drop = rand.Next(1, 4); // 1, 2, or 3

            if (drop == 1)
            {
                Potions++;
                Console.WriteLine("🎁 You found a Healing Potion! (Now have: " + Potions + ")");
            }
            else if (drop == 2)
            {
                PlayerStats.Attack += 5;
                AttackBoosts++;
                Console.WriteLine("🎁 You found an Attack Boost! Attack increased by 5!");
            }
            else
            {
                Console.WriteLine("🎁 The enemy dropped nothing useful this time.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"
🏰 Welcome to the Medieval Adventure!
-------------------------------------------------------------
| ____    ____              __   _                        __  |
||_   \  /   _|            |  ] (_)                      [  | |
|  |   \/   |  .---.   .--.| |  __  .---.  _   __  ,--.   | | |
|  | |\  /| | / /__\\/ /'`\'' | [  |/ /__\\[ \ [  ]`'_\ :  | | |
| _| |_\/_| |_| \__.,| \__/  |  | || \__., \ \/ / // | |, | | |
||_____||_____|'.__.' '.__.;__][___]'.__.'  \__/  \'-;__/[___]|
|  _                  _                                       |
| / |_               / |_                                     |
|`| |-'.---.  _   __`| |-'   .--./) ,--.   _ .--..--.  .---.  |
| | | / /__\\[ \ [  ]| |    / /'`\;`'_\ : [ `.-. .-. |/ /__\\ |
| | |,| \__., > '  < | |,   \ \._//// | |, | | | | | || \__., |
| \__/ '.__.'[__]`\_]\__/   .',__` \'-;__/[___||__||__]'.__.' |
|                          ( ( __))                           |
-------------------------------------------------------------
");

            Console.WriteLine("Your quest: Defeat any two monsters before facing the mighty Dragon!");
            Console.WriteLine("Enemies may drop helpful items. Use potions wisely and prepare for the final fight...\n");

            Player player = new Player(100, 10);
            int defeatedEnemies = 0;
            bool dragonUnlocked = false;

            while (player.IsAlive())
            {
                Console.WriteLine("\nChoose your path:");
                Console.WriteLine("1) Fight a Goblin");
                Console.WriteLine("2) Fight a Skeleton");
                Console.WriteLine("3) Fight a Zombie");

                if (defeatedEnemies >= 2)
                {
                    if (!dragonUnlocked)
                    {
                        Console.WriteLine("🔥 The path to the Dragon is now open!");
                        dragonUnlocked = true;
                    }
                    Console.WriteLine("4) Face the Dragon (Final Boss)");
                }
                else
                {
                    Console.WriteLine("4) [LOCKED] Defeat 2 enemies to face the Dragon");
                }

                Console.WriteLine("5) Quit");

                string choice = Console.ReadLine();
                Enemy enemy = null;

                switch (choice)
                {
                    case "1": enemy = new Goblin(); break;
                    case "2": enemy = new Skeleton(); break;
                    case "3": enemy = new Zombie(); break;
                    case "4":
                        if (defeatedEnemies >= 2)
                        {
                            enemy = new Dragon();
                        }
                        else
                        {
                            Console.WriteLine("❌ You must defeat at least 2 enemies before facing the Dragon!");
                            continue;
                        }
                        break;
                    case "5":
                        Console.WriteLine("Goodbye, brave adventurer!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        continue;
                }

                Console.WriteLine($"\nYou encounter a {enemy.Name}!");

                while (enemy.IsAlive() && player.IsAlive())
                {
                    Console.WriteLine($"\nYour Health: {player.PlayerStats.Health} | Potions: {player.Potions}");
                    Console.WriteLine($"{enemy.Name}'s Health: {enemy.EnemyStats.Health}");
                    Console.WriteLine("Choose an action:");
                    Console.WriteLine("1) Attack");
                    Console.WriteLine("2) Use Potion");
                    Console.WriteLine("3) Run Away");

                    string action = Console.ReadLine();

                    if (action == "1")
                    {
                        player.AttackEnemy(enemy);

                        if (!enemy.IsAlive())
                        {
                            Console.WriteLine($"✅ You defeated the {enemy.Name}!");
                            if (!(enemy is Dragon))
                            {
                                defeatedEnemies++;
                                player.HealAfterBattle(15);
                                player.ApplyItemDrop();
                            }
                            else
                            {
                                // Show the final boss defeat ASCII art
                                Console.WriteLine(@"
,===:'.,            `-._                           
Art by                       `:.`---.__         `-._                     
 John VanderZwaag              `:.     `--.         `.                   
                                 \.        `.         `.                 
                         (,,(,    \.         `.   ____,-`.,               
                      (,'     `/   \.   ,--.___`.'                         
                  ,  ,'  ,--.  `,   \.;'         `                         
                   `{D, {    \  :    \;                                   
                     V,,'    /  /    //                                   
                     j;;    /  ,' ,-//.    ,---.      ,                   
                     \;'   /  ,' /  _  \  /  _  \   ,'/                   
                           \   `'  / \  `'  / \  `.' /                    
                            `.___,'   `.__,'   `.__,' 
");
                                Console.WriteLine("\n🎉 Congratulations! You have defeated the Dragon and won the game!");
                                return;
                            }
                            break;
                        }

                        enemy.AttackPlayer(player);

                        if (!player.IsAlive())
                        {
                            Console.WriteLine("💀 You have been defeated! Game Over.");
                            return;
                        }
                    }
                    else if (action == "2")
                    {
                        player.UsePotion();
                    }
                    else if (action == "3")
                    {
                        Console.WriteLine("You ran away safely!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid action. Try again.");
                    }
                }
            }

            Console.WriteLine("Thank you for playing!");
        }
    }
}
