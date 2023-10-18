using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    #region Variables
    [Header("Panels & GameObjects")]
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject warningPanel;
    [SerializeField] List<GameObject> levelItemList;
    [Header("Sliders")]
    [SerializeField] Slider themeSlider;
    [SerializeField] Slider SFXSlider;
    [Header("Referemces")]
    audioSources _audioSources;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Init();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Initiliazes References, Objects and variables
    /// </summary>
    void Init()
    {
        TinySauce.OnGameStarted("1");

        GameObject[] audioSources = GameObject.FindGameObjectsWithTag("audioSources");
        if (audioSources.Length > 1)
        {
            for (int i = audioSources.Length - 1; i > 0; i--)
            {
                Destroy(audioSources[i]);
            }
        }


        _audioSources = GameObject.FindObjectOfType<audioSources>();
        themeSlider.maxValue = 1f;
        themeSlider.value = PlayerPrefs.GetFloat("themeVolume", 1f); ;
        SFXSlider.maxValue = 1f;
        SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f); ;
        activateDeactivateWarningPanel(false);
        closeSettings();
    }
    /// <summary>
    /// Activates Setting panel
    /// </summary>
    public void openSettings()
    {
        settingsPanel.SetActive(true);
        //Animator _animator = settingsPanel.GetComponentInChildren<Animator>();
        //Debug.Log(_animator.gameObject.name);
        //_animator.Play("panelOpen");
        
    }
    /// <summary>
    /// Deactivates Settings panel
    /// </summary>
    public void closeSettings()
    {
        settingsPanel.SetActive(false);
        //Animator _animator = settingsPanel.GetComponentInChildren<Animator>();
        ////Debug.Log(_animator.gameObject.name);
        //_animator.Play("panelClose");
        //StartCoroutine(waitForCloseAnim(_animator, settingsPanel));
        ////settingsPanel.SetActive(false);
    }
    /// <summary>
    /// Gets themSlider value and assigns it to the theme audio sources volume
    /// </summary>
    public void setThemeVolume()
    {
        _audioSources.themeAs.volume = themeSlider.value;
        PlayerPrefs.SetFloat("themeVolume", themeSlider.value);
    }
    /// <summary>
    /// Gets sfxSlider value and assigns it to the theme SFX sources volume
    /// </summary>
    public void setSFXASVolume()
    {
        _audioSources.sFXAs.volume = SFXSlider.value;
        PlayerPrefs.SetFloat("sfxVolume", SFXSlider.value);
    }
    /// <summary>
    /// Before Deletes playerInfo list opensUp warning panel
    /// </summary>
    public void resetLevels()
    {
        activateDeactivateWarningPanel(true);
    }
    /// <summary>
    /// calls delete json file then deactivates warning panel
    /// </summary>
    public void submit()
    {
        PlayerInfos.deleteJson();
        foreach(var levelItem in levelItemList)
        {
            levelManager _levelManager = levelItem.GetComponent<levelManager>();
            if (_levelManager != null)
                _levelManager.Init();
        }
        activateDeactivateWarningPanel(false);
    }
    /// <summary>
    /// Deactivates warning panei
    /// </summary>
    public void reject()
    {
        activateDeactivateWarningPanel(false);
    }
    /// <summary>
    /// According to the given parameter activates deactivates warning panel
    /// </summary>
    /// <param name="state"></param>
    void activateDeactivateWarningPanel(bool state)
    {
        warningPanel.SetActive(state);
        //Animator _animator = warningPanel.GetComponentInChildren<Animator>();
        ////Debug.Log(_animator.gameObject.name);
        //if (!state)
        //{
        //    _animator.Play("panelClose");
        //    StartCoroutine(waitForCloseAnim(_animator,warningPanel));
        //}
        //else
        //{
        //    warningPanel.SetActive(state);
        //    _animator.Play("panelOpen");
            
        //}
    }
    /// <summary>
    /// Waits until given animator's animatiom to stop playing then deactites given Panel
    /// </summary>
    /// <param name="_animator"></param>
    /// <returns></returns>
    IEnumerator waitForCloseAnim(Animator _animator, GameObject _panel)
    {
        
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        _panel.SetActive(false);
    }

}
