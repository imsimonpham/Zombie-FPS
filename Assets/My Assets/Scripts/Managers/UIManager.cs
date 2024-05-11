using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Player")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _highScoreText;
    [SerializeField] private Text _scoreText;

    [Header("Gameplay")] 
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _waveNumberText;
    

    [Header("Death Screen")]
    [SerializeField] private Text _deathText;
    [SerializeField] private Image _deathScreenBackground;
    private float _startAlpha;
    private float _targetBacgroundAlpha = 0.9f;
    private float _targetTextAlpha = 1f;
    private float _elapsedTime = 0f;
    private float _deathScreenDuration = 7f;

    private void Start()
    {
        //Player's health
        _healthBar.value = _healthBar.maxValue = _playerController.GetMaxHealth();

        //Death Screen
        _startAlpha = _deathScreenBackground.color.a;
    }

    //Gameplay UI
    #region
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

    public void ShowDeathScreen()
    {
        if(_elapsedTime < _deathScreenDuration)
        {
            //gradually change death screen background alpha
            float newBackgroundAlpha = Mathf.Lerp(_startAlpha, _targetBacgroundAlpha, _elapsedTime / _deathScreenDuration);
            Color newColor = _deathScreenBackground.color;
            newColor.a = newBackgroundAlpha;
            _deathScreenBackground.color = newColor;

            //gradually change death text alpha
            float newTextAlpha = Mathf.Lerp(_startAlpha, _targetTextAlpha, _elapsedTime / _deathScreenDuration);
            Color newTextColor = _deathText.color;
            newTextColor.a = newTextAlpha;
            _deathText.color = newTextColor;

            //update elapsed time
            _elapsedTime += Time.deltaTime;
        }else
            Time.timeScale = 0;
    }

    #endregion

    //Menu UI
    #region
    #endregion
}
