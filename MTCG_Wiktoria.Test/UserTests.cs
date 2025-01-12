using NUnit.Framework;
using MTCG_Wiktoria.Users;

namespace MTCG.Tests
{
    public class UserTests
    {
        [Test]
        public void UserInitialization_ValidInputs_ShouldSetProperties()
        {
            var user = new User("test_user", "password123");

            Assert.AreEqual("test_user", user.Username);
            Assert.AreEqual("password123", user.Password);
            Assert.AreEqual(20, user.Coins);
            Assert.AreEqual(100, user.ELO);
        }

        [Test]
        public void AddCoins_ShouldIncreaseCoins()
        {
            var user = new User("test_user", "password123");
            user.AddCoins(10);

            Assert.AreEqual(30, user.Coins);
        }

        [Test]
        public void DeductCoins_SufficientBalance_ShouldReduceCoins()
        {
            var user = new User("test_user", "password123");
            user.DeductCoins(5);

            Assert.AreEqual(15, user.Coins);
        }

        [Test]
        public void DeductCoins_InsufficientBalance_ShouldNotReduceCoins()
        {
            var user = new User("test_user", "password123");
            user.DeductCoins(25);

            Assert.AreEqual(20, user.Coins);
        }
    }
}