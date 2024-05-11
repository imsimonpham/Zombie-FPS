using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Text _highScoreText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _waveNumberText;
    [SerializeField] private Text _ammoText;

    private void Start()
    {
        _healthBar.value = _healthBar.maxValue = _playerController.GetMaxHealth();
    }

    public void UpdateHealthBar(float health)
    {
        _healthBar.value = health;
    }

    public void UpdateHighScoreText(int score)
    {
        _highScoreText.text = score.ToString();
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void UpdateTimerText(string timeText)
    {
        _timerText.text = timeText;
    }

    public void UpdateWaveNumberText(int waveNum)
    {
        _waveNumberText.text = waveNum.ToString();
    }

    public void UpdateAmmoText(int ammoCount)
    {
        _ammoText.text = ammoCount.ToString();
    }
}
