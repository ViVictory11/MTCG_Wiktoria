namespace MTCG_Wiktoria.Cards;

public abstract class Card
{
    protected Card(string name, Element element, int damage)
    {
        Name = name;
        Element = element;
        Damage = damage;
    }

    public string Name{ get; private set; }
    public Element Element { get; private set; }
    public int Damage { get; private set; }
    
    
}