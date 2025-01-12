using System;
using System.Collections.Generic;
using MTCG_Wiktoria.Cards;

namespace MTCG_Wiktoria.Battle
{
    public class RoundHandler(List<Card> player1Deck, List<Card> player2Deck, BattleLogger logger)
    {
        private Booster _player1Booster = null;
        private Booster _player2Booster = null;

        public void PlayRound()
        {
            Random rand = new Random();
            Card player1Card = player1Deck[rand.Next(player1Deck.Count)];
            Card player2Card = player2Deck[rand.Next(player2Deck.Count)];

            logger.Log($"Player 1 plays {player1Card.Name} ({player1Card.Damage} dmg, {player1Card.Element})");
            logger.Log($"Player 2 plays {player2Card.Name} ({player2Card.Damage} dmg, {player2Card.Element})");

            int damage1 = CardEffectiveness.Calculate(player1Card, player2Card, logger);
            int damage2 = CardEffectiveness.Calculate(player2Card, player1Card, logger);

            // Apply boosters
            damage1 = _player1Booster?.Apply(player1Card, player2Card, damage1) ?? damage1;
            damage2 = _player2Booster?.Apply(player2Card, player1Card, damage2) ?? damage2;

            if (damage1 > damage2)
            {
                logger.Log($"Player 1's {player1Card.Name} wins the round!");
                player1Deck.Add(player2Card);
                player2Deck.Remove(player2Card);
                logger.UpdateStreak(true); // Player 1 wins
                _player2Booster = CheckLossBooster(logger.GetPlayer2Streak(), "Player 2");
                _player1Booster = CheckWinBooster(logger.GetPlayer1Streak(), "Player 1");
            }
            else if (damage2 > damage1)
            {
                logger.Log($"Player 2's {player2Card.Name} wins the round!");
                player2Deck.Add(player1Card);
                player1Deck.Remove(player1Card);
                logger.UpdateStreak(false); // Player 2 wins
                _player1Booster = CheckLossBooster(logger.GetPlayer1Streak(), "Player 1");
                _player2Booster = CheckWinBooster(logger.GetPlayer2Streak(), "Player 2");
            }
            else
            {
                logger.Log("It's a draw! No cards are exchanged.");
                logger.ResetStreaks(); // Reset streaks on draw
            }
        }

        private Booster CheckWinBooster(int streak, string playerName)
        {
            if (streak != 3) return null;
            logger.Log($"{playerName} has won 3 rounds in a row and may pick a booster!");
            return Booster.ChooseBooster(playerName);
        }

        private Booster CheckLossBooster(int streak, string playerName)
        {
            if (streak != 4) return null;
            logger.Log($"{playerName} has lost 4 rounds in a row and receives a randomized booster!");
            return Booster.RandomBooster(playerName);
        }
    }
}
