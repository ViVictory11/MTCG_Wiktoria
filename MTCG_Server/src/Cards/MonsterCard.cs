namespace MTCG_Wiktoria.Cards;

public class MonsterCard: Card
{
    public Species Species { get; private set; }

    public MonsterCard(string name, Element element, Species species, int damage) : base(name, element, damage)
    {
        Species = species;
    }


    public override int CalculateDamage(Card opponent)
    {
               
        
        
        
        
    }
}