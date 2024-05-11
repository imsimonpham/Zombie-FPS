using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] Button _normalDiffBtn;
    [SerializeField] Button _hardDiffBtn;
    [SerializeField] Button _saveBtn;
    [SerializeField] private float _timeBetweenWaves;
    [SerializeField] private int _zombiesPerWaves;
    [SerializeField] private float _timeBetweenWavesNormal;
    [SerializeField] private int _zombiesPerWavesNormal;
    [SerializeField] private float _timeBetweenWavesHard;
    [SerializeField] private int _zombiesPerWavesHard;

    [Header("Controls")]
    [SerializeField] Slider _lookSensitivitySlider;
    [SerializeField] Slider _mouseSmoothingSlider;
    [SerializeField] private float _lookSensitivity;
    [SerializeField] private float _mouseSmoothing;
    private float _defaultLookSensitivity = 10f;
    private float _defaultMouseSmoothing = 10f;


    private void Start()
    {
        //set min and max values for control sliders
        _lookSensitivitySlider.minValue = 1f;
        _lookSensitivitySlider.maxValue = 20f;
        _mouseSmoothingSlider.minValue = 1f;
        _mouseSmoothingSlider.maxValue = 20f;

        //load setting       
        LoadSettings();

        //click events
        _normalDiffBtn.onClick.AddListener(SetNormalDifficulty);
        _hardDiffBtn.onClick.AddListener(SetHardDifficulty);
        _saveBtn.onClick.AddListener(SaveSettings); //this has to be placed after other click events
    }

    public void LoadSettings()
    {
        //controls
        if (PlayerPrefs.HasKey("LookSensitivity"))
        {
            _lookSensitivity = PlayerPrefs.GetFloat("LookSensitivity");
            _lookSensitivitySlider.value = _lookSensitivity;
        }
        else
            _lookSensitivitySlider.value = _lookSensitivity = _defaultLookSensitivity;


        if (PlayerPrefs.HasKey("MouseResponse"))
        {
            _mouseSmoothing = PlayerPrefs.GetFloat("MouseResponse");
            _mouseSmoothingSlider.value = _mouseSmoothing;;
        }          
        else
            _mouseSmoothingSlider.value = _mouseSmoothing = _defaultMouseSmoothing;


        //difficulty
        if (PlayerPrefs.HasKey("TimeBetweenWaves"))
            _timeBetweenWaves = PlayerPrefs.GetFloat("TimeBetweenWaves");
        else
            _timeBetweenWaves = _timeBetweenWavesNormal;


        if (PlayerPrefs.HasKey("ZombiesPerWave"))
            _zombiesPerWaves = PlayerPrefs.GetInt("ZombiesPerWave");
        else
            _zombiesPerWaves = _zombiesPerWavesNormal;


        Debug.Log("Settings Loaded");
    }

    public void SaveSettings()
    {
        //difficulty
        PlayerPrefs.SetFloat("TimeBetweenWaves", _timeBetweenWaves);
        PlayerPrefs.SetInt("ZombiesPerWave", _zombiesPerWaves);

        //controls
        _lookSensitivity = _lookSensitivitySlider.value;
        _mouseSmoothing = _mouseSmoothingSlider.value;
        PlayerPrefs.SetFloat("LookSensitivity", _lookSensitivity);
        PlayerPrefs.SetFloat("MouseResponse", _mouseSmoothing);

        //save settings
        PlayerPrefs.Save();
        Debug.Log("Settings Saved");
    }

    private void SetNormalDifficulty()
    {
        _timeBetweenWaves = _timeBetweenWavesNormal;
        _zombiesPerWaves = _zombiesPerWavesNormal;
    }

    private void SetHardDifficulty()
    {
        _timeBetweenWaves = _timeBetweenWavesHard;
        _zombiesPerWaves = _zombiesPerWavesHard;
    }
}
