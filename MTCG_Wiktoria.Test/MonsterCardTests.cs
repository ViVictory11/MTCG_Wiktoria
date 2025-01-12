using NUnit.Framework;
using MTCG_Wiktoria.Cards;
using System;
using System.Collections.Generic;
using System.Linq;


public class MonsterCardTests
{
    [Test]
    public void TestMonsterCard_Initialization()
    {
        var card = new MonsterCard("Dragon", Element.FIRE, 100);
        Assert.AreEqual("Dragon", card.Name);
        Assert.AreEqual(Element.FIRE, card.Element);
        Assert.AreEqual(100, card.Damage);
    }

    [Test]
    public void TestMonsterCard_Equality()
    {
        var card1 = new MonsterCard("Dragon", Element.FIRE, 100);
        var card2 = new MonsterCard("Dragon", Element.FIRE, 100);
        Assert.AreEqual(card1.Name, card2.Name);
    }

    [Test]
    public void TestMonsterCard_DamageComparison()
    {
        var strongCard = new MonsterCard("StrongMonster", Element.EARTH, 200);
        var weakCard = new MonsterCard("WeakMonster", Element.NORMAL, 50);
        Assert.Greater(strongCard.Damage, weakCard.Damage);
    }
}