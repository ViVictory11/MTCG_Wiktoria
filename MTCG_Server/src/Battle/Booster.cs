using MTCG_Wiktoria.Cards;
using System;
using System.Collections.Generic;

namespace MTCG_Wiktoria.Battle
{
    public class Booster(string name, Func<Card, Card, int, int> effect, int roundsDuration)
    {
        public string Name { get; private set; } = name;
        private Func<Card, Card, int, int> Effect { get; set; } = effect;
        private int RoundsRemaining { get; set; } = roundsDuration;

        public int Apply(Card attacker, Card defender, int baseDamage)
        {
            if (RoundsRemaining > 0)
            {
                RoundsRemaining--;
                return Effect(attacker, defender, baseDamage);
            }
            return baseDamage;
        }

        public static Booster ChooseBooster(string playerName)
        {
            Console.WriteLine($"{playerName}, you have won 3 rounds in a row!");
            Console.WriteLine("Choose your booster:");
            Console.WriteLine("1. Shield (Halves opponent's damage for the next round)");
            Console.WriteLine("2. Damage Boost (20% extra damage for the next 2 rounds)");
            
            string choice;
            do
            {
                Console.Write("Enter 1 or 2: ");
                choice = Console.ReadLine();
            }
            while (choice != "1" && choice != "2");

            if (choice == "1")
            {
                return new Booster(
                    "Shield",
                    (attacker, defender, damage) => damage / 2,
                    1
                );
            }
            else // choice == "2"
            {
                return new Booster(
                    "Damage Boost",
                    (attacker, defender, damage) => (int)(damage * 1.2),
                    2
                );
            }
        }

        public static Booster RandomBooster(string playerName)
        {
            Random rand = new Random();
            int choice = rand.Next(1, 5); // Random number from 1 to 4
            Console.WriteLine($"{playerName} gets a random booster: #{choice}");

            return choice switch
            {
                1 => new Booster(
                    "Block Opponent",
                    (attacker, defender, damage) => 0,
                    1
                ),
                2 => new Booster(
                    "Double Damage",
                    (attacker, defender, damage) => damage * 2,
                    2
                ),
                3 => new Booster(
                    "Halve Opponent Damage",
                    (attacker, defender, damage) => damage / 2,
                    1
                ),
                _ => new Booster(
                    "Extra Damage",
                    (attacker, defender, damage) => damage + 20,
                    2
                )
            };
        }
    }
}

