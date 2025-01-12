namespace MTCG_Wiktoria.Cards
{
    public class SpellCard(string name, Element element, int damage) : Card(name, element, damage)
    {
        /*public int CalculateDamageAgainst(Card opponent)
        {
            if (opponent is MonsterCard monster)
            {
                return ElementEffectiveness(monster);
            }
            if (opponent is SpellCard spell)
            {
                return ElementEffectiveness(spell);
            }
            return Damage;
        }*/

        /*private int ElementEffectiveness(Card opponent)
        {
            if ((Element == Element.WATER && opponent.Element == Element.FIRE) ||
                (Element == Element.FIRE && opponent.Element == Element.NORMAL) ||
                (Element == Element.NORMAL && opponent.Element == Element.WATER) ||
                (Element == Element.EARTH && opponent.Element == Element.LIGHTNING) ||
                (Element == Element.LIGHTNING && opponent.Element == Element.WATER) ||
                (Element == Element.EARTH && opponent.Element == Element.FIRE))
            {
                return Damage * 2; // Super effective
            }

            if ((Element == Element.FIRE && opponent.Element == Element.WATER) ||
                (Element == Element.NORMAL && opponent.Element == Element.FIRE) ||
                (Element == Element.WATER && opponent.Element == Element.NORMAL) ||
                (Element == Element.LIGHTNING && opponent.Element == Element.EARTH) ||
                (Element == Element.WATER && opponent.Element == Element.LIGHTNING) ||
                (Element == Element.FIRE && opponent.Element == Element.EARTH))
            {
                return Damage / 2; // Not effective
            }

            // Neutral effectiveness
            return Damage;
        }*/
    }
}