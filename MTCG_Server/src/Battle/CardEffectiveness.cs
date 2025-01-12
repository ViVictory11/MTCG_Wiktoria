using MTCG_Wiktoria.Cards;

namespace MTCG_Wiktoria.Battle
{
    public static class CardEffectiveness
    {
        // Define the effectiveness matrix
        private static readonly Dictionary<(Element, Element), double> EffectivenessMatrix = new()
        {
            // FIRE interactions
            { (Element.FIRE, Element.FIRE), 1.0 },
            { (Element.FIRE, Element.WATER), 0.5 },
            { (Element.FIRE, Element.NORMAL), 2.0 },
            { (Element.FIRE, Element.EARTH), 2.0 },
            { (Element.FIRE, Element.LIGHTNING), 0.5 },

            // WATER interactions
            { (Element.WATER, Element.FIRE), 2.0 },
            { (Element.WATER, Element.WATER), 1.0 },
            { (Element.WATER, Element.NORMAL), 0.5 },
            { (Element.WATER, Element.EARTH), 0.5 },
            { (Element.WATER, Element.LIGHTNING), 2.0 },

            // NORMAL interactions
            { (Element.NORMAL, Element.FIRE), 0.5 },
            { (Element.NORMAL, Element.WATER), 2.0 },
            { (Element.NORMAL, Element.NORMAL), 1.0 },
            { (Element.NORMAL, Element.EARTH), 2.0 },
            { (Element.NORMAL, Element.LIGHTNING), 0.5 },

            // EARTH interactions
            { (Element.EARTH, Element.FIRE), 0.5 },
            { (Element.EARTH, Element.WATER), 0.5 },
            { (Element.EARTH, Element.NORMAL), 2.0 },
            { (Element.EARTH, Element.EARTH), 1.0 },
            { (Element.EARTH, Element.LIGHTNING), 2.0 },

            // LIGHTNING interactions
            { (Element.LIGHTNING, Element.FIRE), 2.0 },
            { (Element.LIGHTNING, Element.WATER), 0.5 },
            { (Element.LIGHTNING, Element.NORMAL), 2.0 },
            { (Element.LIGHTNING, Element.EARTH), 0.5 },
            { (Element.LIGHTNING, Element.LIGHTNING), 1.0 }
        };


        public static int Calculate(Card attacker, Card defender, BattleLogger logger)
        {
            // Handle special monster interactions
            if (attacker is MonsterCard && defender is MonsterCard)
            {
                if (SpecialMonsterInteractions(attacker, defender, logger, out var specialDamage))
                {
                    return specialDamage;
                }
            }

            // Handle special spell interactions with monsters
            if (attacker is SpellCard && defender is MonsterCard)
            {
                if (SpecialSpellInteractions(attacker, defender, logger, out var specialDamage))
                {
                    return specialDamage;
                }
            }

            // Default to element effectiveness
            return ElementEffectiveness(attacker, defender, logger);
        }

        private static bool SpecialMonsterInteractions(Card attacker, Card defender, BattleLogger logger, out int damage)
        {
            damage = 0;

            // Goblin vs. Dragon
            if (attacker.Name.Contains("Goblin") && defender.Name.Contains("Dragon"))
            {
                logger.Log("Goblin is too afraid to attack Dragon!");
                return true;
            }

            // Wizard vs. Orc
            if (attacker.Name.Contains("Wizzard") && defender.Name.Contains("Ork"))
            {
                logger.Log("Wizzard controls Ork, no damage dealt!");
                return true;
            }

            // FireElf vs. Dragon
            if (attacker.Name.Contains("FireElf") && defender.Name.Contains("Dragon"))
            {
                logger.Log("FireElf evades the Dragon's attack!");
                return true;
            }

            return false;
        }

        private static bool SpecialSpellInteractions(Card attacker, Card defender, BattleLogger logger, out int damage)
        {
            damage = 0;

            // Kraken immune to spells
            if (defender.Name.Contains("Kraken"))
            {
                logger.Log("Kraken is immune to spells!");
                return true;
            }

            // Knight vs. WaterSpell
            if (defender.Name.Contains("Knight") && attacker.Element == Element.WATER)
            {
                logger.Log("Knight drowns under a Water Spell!");
                damage = int.MaxValue; // Instant defeat
                return true;
            }

            return false;
        }

        private static int ElementEffectiveness(Card attacker, Card defender, BattleLogger logger)
        {
            var key = (attacker.Element, defender.Element);
            double multiplier = EffectivenessMatrix[key];

            if (multiplier > 1)
            {
                logger.Log($"{attacker.Name}'s attack is super effective!");
            }
            else if (multiplier < 1)
            {
                logger.Log($"{attacker.Name}'s attack is not very effective.");
            }
            else
            {
                logger.Log($"{attacker.Name}'s attack is neutral.");
            }

            return (int)(attacker.Damage * multiplier);
        }

    }
}
