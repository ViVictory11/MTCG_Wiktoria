using MTCG_Wiktoria.Cards;
using NUnit.Framework;

namespace MTCG_Wiktoria.Test
{
    public class CardTests
    {
        // A mock subclass to test the abstract Card class
        public class TestCard : Card
        {
            public TestCard(string name, Element element, int damage) : base(name, element, damage)
            {
            }
        }

        [Test]
        public void Card_Initialization_ShouldSetProperties()
        {
            // Arrange
            string name = "Test Card";
            Element element = Element.Fire; // Assuming Element is an enum or defined elsewhere
            int damage = 100;

            // Act
            var card = new TestCard(name, element, damage);

            // Assert
            Assert.AreEqual(name, card.Name);
            Assert.AreEqual(element, card.Element);
            Assert.AreEqual(damage, card.Damage);
        }
    }
}