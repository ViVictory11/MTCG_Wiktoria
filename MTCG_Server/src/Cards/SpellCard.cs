namespace MTCG_Wiktoria.Cards;

public class SpellCard: Card
{
    public SpellCard(string name, Element element, int damage) : base(name, element, damage)
    {
    }
    
    public int CalculateDamageAgainst(Card opponent)
    {
        if (opponent is MonsterCard monster)
        {
            return ElementEffectiveness(monster);
        }
        return Damage;
    }

    private int ElementEffectiveness(MonsterCard opponent)
    {
        if (Element == Element.Water && opponent.Element == Element.Fire)
            return Damage * 2;
        if (Element == Element.Fire && opponent.Element == Element.Normal)
            return Damage * 2;
        if (Element == Element.Normal && opponent.Element == Element.Water)
            return Damage * 2;
        if (Element == Element.Fire && opponent.Element == Element.Water)
            return Damage / 2;
        
        return Damage;
    }
}