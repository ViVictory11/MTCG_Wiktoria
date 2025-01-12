using MTCG_Wiktoria.Cards;

namespace MTCG_Wiktoria.Battle
{
    public class BattleManager
    {
        private readonly List<Card> _player1Deck;
        private readonly List<Card> _player2Deck;
        private readonly BattleLogger _logger = new BattleLogger();
        private readonly RoundHandler _roundHandler;

        
        public BattleManager(List<Card> player1Deck, List<Card> player2Deck)
        {
            this._player1Deck = player1Deck;
            this._player2Deck = player2Deck;
            this._roundHandler = new RoundHandler(player1Deck, player2Deck, _logger);
        }

        public string Start()
        {
            int roundCount = 0;

            _logger.Log("Starting the battle!");
            while (_player1Deck.Count > 0 && _player2Deck.Count > 0 && roundCount < 100)
            {
                roundCount++;
                _logger.Log($"--- Round {roundCount} ---");
                _roundHandler.PlayRound();
            }

            DetermineWinner();
            return _logger.GetLog();
        }

        private void DetermineWinner()
        {
            if (_player1Deck.Count > _player2Deck.Count)
            {
                _logger.Log("Player 1 wins the battle!");
            }
            else if (_player2Deck.Count > _player1Deck.Count)
            {
                _logger.Log("Player 2 wins the battle!");
            }
            else
            {
                _logger.Log("The battle ends in a draw!");
            }
        }
    }
}