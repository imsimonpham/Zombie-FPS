using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("High Score")]
    [SerializeField] private int _highScore;
    [SerializeField] private int _currentScore;

    [Header("Gameplay")]
    [SerializeField] private float _timer;
    private bool _showDeathScreen = false;

    private void Start()
    {
        _currentScore = 0;
        UIManager.Instance.UpdateScoreText(_currentScore);
    }
    private void Update()
    {
        //Timer
        Timer();

        //High Score
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            UIManager.Instance.UpdateHighScoreText(_currentScore);
        }

        //Death Screen
        if (_showDeathScreen)
            UIManager.Instance.ShowDeathScreen();
    }

    public void IncreaseScore()
    {
        _currentScore += 1;
        UIManager.Instance.UpdateScoreText(_currentScore);
    }

    void Timer()
    {
        _timer += Time.deltaTime;
        // Convert timer to minutes and seconds
        int minutes = Mathf.FloorToInt(_timer / 60);
        int seconds = Mathf.FloorToInt(_timer % 60);

        // Display the time in "00:00" format
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        UIManager.Instance.UpdateTimerText(timeString);
    }

    public void PlayerDied() { _showDeathScreen = true; }
}
