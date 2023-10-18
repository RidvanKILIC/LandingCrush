using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    #region Variables
    //[Header("Buttons")]
    //[SerializeField] Button brakeButton;
    [Header("Panels")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject mainPanel;
    [Header("Sliders")]
    [SerializeField] Slider themeSlider;
    [SerializeField] Slider SFXSlider;
    [Header("References")]
    Baloon _baloon;
    gameManager _gameManager;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    /// <summary>
    /// Initiliazes variables and references
    /// </summary>
    void Init()
    {
        _gameManager = GameObject.FindObjectOfType<gameManager>();
        _baloon = GameObject.FindObjectOfType<Baloon>();
        pausePanel.SetActive(false);
        mainPanel.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            onBrakeDown();
        else if (Input.GetKeyUp(KeyCode.Space))
            onBrakeUp();
    }
    /// <summary>
    /// Calls BaloonMovement's brake function
    /// </summary>
    public void onBrakeDown()
    {
        if (!_baloon.stuckWithObstacle)
        {
            _baloon._baloonMovement.brake();

        }

    }
    /// <summary>
    /// Calls BaloonMovement's release function
    /// </summary>
    public void onBrakeUp()
    {
        _baloon._baloonMovement.relase();

    }
    /// <summary>
    /// Activates pause panel sets time scale to 0
    /// </summary>
    public void pause()
    {
        pausePanel.SetActive(true);
        _gameManager.gamePaused = true;
        Time.timeScale = 0;
    }
    /// <summary>
    /// Deactivates pause panel set time scale to 1
    /// </summary>
    public void resune()
    {
        pausePanel.SetActive(false);
        _gameManager.gamePaused = false;
        Time.timeScale = 1;
    }
    /// <summary>
    /// Calls SoundManager's adjustThemeValue function with the themeSlider's value
    /// </summary>
    public void themeASSliderChange()
    {
        SoundManager.SInstance.adjustThemeValume(themeSlider.value);
        PlayerPrefs.SetFloat("themeVolume", themeSlider.value);
    }
    /// <summary>
    /// Calls SoundManager's adjustSFXValume function with the SFXSlider's value
    /// </summary>
    public void sfxSliderChange()
    {
        SoundManager.SInstance.adjustSFXValume(SFXSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", SFXSlider.value);

    }
    /// <summary>
    /// Assign given float value to theme slider value
    /// </summary>
    /// <param name="_value"></param>
    public void setSFXlideValue(float _value)
    {
        SFXSlider.value = _value;
    }
    /// <summary>
    /// Assign given float value to sfx slider value
    /// </summary>
    /// <param name="_value"></param>
    public void setThemeSlideValue(float _value)
    {
        themeSlider.value = _value;
    }
}
