using NUnit.Framework;
using MTCG_Wiktoria.Server;
using MTCG_Wiktoria.Cards;
using System;
using System.Collections.Generic;
using System.Linq;



[TestFixture]
public class SpellCardTests
{
    [Test]
    public void TestSpellCard_Initialization()
    {
        var card = new SpellCard("Fireball", Element.FIRE, 80);
        Assert.AreEqual("Fireball", card.Name);
        Assert.AreEqual(Element.FIRE, card.Element);
        Assert.AreEqual(80, card.Damage);
    }

    [Test]
    public void TestSpellCard_CalculateDamageAgainstMonster()
    {
        var spell = new SpellCard("Fireball", Element.FIRE, 80);
        var monster = new MonsterCard("Goblin", Element.NORMAL, 50);
        // Assuming the commented-out ElementEffectiveness method is implemented
        // Assert.AreEqual(160, spell.CalculateDamageAgainst(monster));
    }

    [Test]
    public void TestSpellCard_ElementEffectiveness()
    {
        var spell = new SpellCard("Water Blast", Element.WATER, 60);
        var fireMonster = new MonsterCard("FireDragon", Element.FIRE, 100);
        // Assuming the commented-out ElementEffectiveness method is implemented
        // Assert.AreEqual(120, spell.CalculateDamageAgainst(fireMonster));
    }
}