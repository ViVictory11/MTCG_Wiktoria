public class BattleLogger
{
    private readonly List<string> _log = new List<string>();
    private int _player1Streak = 0;
    private int _player2Streak = 0;

    public void Log(string message)
    {
        _log.Add(message);
    }

    public string GetLog()
    {
        return string.Join("\n", _log);
    }

    public void UpdateStreak(bool player1Won)
    {
        if (player1Won)
        {
            _player1Streak++;
            _player2Streak = 0;
        }
        else
        {
            _player2Streak++;
            _player1Streak = 0;
        }
    }

    public int GetPlayer1Streak() => _player1Streak;
    public int GetPlayer2Streak() => _player2Streak;

    public void ResetStreaks()
    {
        _player1Streak = 0;
        _player2Streak = 0;
    }
}