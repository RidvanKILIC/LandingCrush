using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Variables
    [Header("Audio Sources")]
    AudioSource themeAS;
    AudioSource SFXS;
    [SerializeField] AudioSource BaloonS;
    [Header("Audio Clips")]
    [SerializeField] AudioClip slowTheme;
    [SerializeField] AudioClip fastTheme;
    [Header("SFXs")]
    public AudioClip flame;
    public AudioClip crash;
    public AudioClip sucess;
    public AudioClip fail;
    public AudioClip star;
    public AudioClip avoidDanger;
    public AudioClip danger;
    public AudioClip hitObstacle;
    [Header("References")]
    audioSources _audioSources;
    ButtonController _buttonController;
    private static SoundManager instance;
    public static SoundManager SInstance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Sound instance is null");
            }
            return instance;
        }
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        
        _audioSources = GameObject.FindObjectOfType<audioSources>();
        _buttonController = GameObject.FindObjectOfType<ButtonController>();
        themeAS = _audioSources.themeAs;
        SFXS = _audioSources.sFXAs;
        setMainMenuSettings();
        //playPauseTheme(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void setMainMenuSettings()
    {

        _buttonController.setThemeSlideValue(themeAS.volume);
        _buttonController.setSFXlideValue(SFXS.volume);
    }
    /// <summary>
    /// Sets theme audio source's volume with the given value
    /// </summary>
    /// <param name="_value"></param>
    public void adjustThemeValume(float _value)
    {
        themeAS.volume = _value;

    }
    /// <summary>
    /// Sets theme SFX source's volume with the given value
    /// </summary>
    /// <param name="_value"></param>
    public void adjustSFXValume(float _value)
    {
        SFXS.volume = _value;

    }
    /// <summary>
    /// Plays theme sound according to the parameter 0 for sloaw 1 for fast one
    /// </summary>
    /// <param name="_sound"></param>
    public void playThemeSound(int _sound)
    {
       switch (_sound)
       {
            case 0:
                themeAS.pitch = 1f;
                break;

            case 1:
                themeAS.pitch = 1f;
                break;
            default:
                themeAS.pitch = 1f;
                break;


        }
        if (!themeAS.isPlaying)
        {
            themeAS.clip = slowTheme;
            themeAS.Play();
        }

    }
    /// <summary>
    /// Plays Sound SFXa
    /// </summary>
    /// <param name="sfx"></param>
    public void playSfx(AudioClip sfx)
    {
        if (sfx != null)
        {
            if (sfx.Equals(danger) && (!SFXS.isPlaying))
            {
                SFXS.loop = true;
                SFXS.clip = danger;
                SFXS.Play();
            }
            else if (!sfx.Equals(danger))
            {
                SFXS.loop = false;
                SFXS.PlayOneShot(sfx);
            }
        }
        else
        {
            SFXS.Pause();
        }
    }
    /// <summary>
    /// Plays given Audio clip using BaloonS if parameter audio clip is null stops playing
    /// </summary>
    /// <param name="_clip"></param>
    public void playBaloonSound(AudioClip _clip = null)
    {
        if (_clip != null)
        {
            BaloonS.clip = _clip;
            BaloonS.Play();
        }
        else
        {
            BaloonS.Stop();
        }


    }
    /// <summary>
    /// if true resumes else pauses theme as
    /// </summary>
    /// <param name="state"></param>
    public void playPauseTheme(bool state)
    {
        if (!state)
            themeAS.Pause();
        else if(!themeAS.isPlaying)
            themeAS.UnPause();
    } 
}
