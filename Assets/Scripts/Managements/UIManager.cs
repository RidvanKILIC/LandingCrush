using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("HUD Text References")]
    [SerializeField] TMPro.TMP_Text speedText;
    [SerializeField] TMPro.TMP_Text heightText;
    [Header("HUD Slider References")]
    [SerializeField] Slider fuelSlider;
    [SerializeField] Slider heightSlider;
    [SerializeField] Slider speedSlider;
    [SerializeField] Image fuelSliderImage;
    [SerializeField] Gradient gradientFuelColor;
    [SerializeField] Gradient gradientSpeedColor;
    private static UIManager instance;
    public static UIManager UInstance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("UI instance is null");
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Sets max value of fuel slider with the given float variable
    /// </summary>
    /// <param name="_value"></param>
    public void setFuelSliderMaxValue(float _value)
    {
        fuelSlider.maxValue = _value;
        fuelSlider.value = _value;
        fuelSliderImage.color = gradientFuelColor.Evaluate(fuelSlider.normalizedValue);
        //Debug.Log("Fuel Max Value: " + fuelSlider.maxValue);
    }
    /// <summary>
    /// Sets value of fuel slider with the given float variable
    /// </summary>
    /// <param name="_value"></param>
    public void setFuelSliderValue(float _value)
    {
        fuelSlider.value = _value;
        fuelSliderImage.color = gradientFuelColor.Evaluate(fuelSlider.normalizedValue);
        //Debug.Log("Fuel Current Value: " + fuelSlider.value);
    }
    /// <summary>
    /// Sets max value of speed slider with the given float variable
    /// </summary>
    /// <param name="_value"></param>
    public void setSpeedSliderMaxValue(float _value)
    {
        speedSlider.maxValue = _value;
        speedSlider.value = _value;

        //speedSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = gradientFuelColor.Evaluate(speedSlider.normalizedValue);
        //Debug.Log("Speed Max Value: " + speedSlider.maxValue);
    }
    /// <summary>
    /// Sets value of speed slider with the given float variable
    /// </summary>
    /// <param name="_value"></param>
    public void setSpeedSliderValue(float _value)
    {
        speedSlider.value = _value;
        //speedSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = gradientFuelColor.Evaluate(speedSlider.normalizedValue);
        //Debug.Log("Speed Current Value: " + speedSlider.value);
    }
    /// <summary>
    /// Sets max value of height slider with the given float variable
    /// </summary>
    /// <param name="_value"></param>
    public void setHeightSliderMaxValue(float _value)
    {
        heightSlider.maxValue = _value;
        heightSlider.value = _value;
        setSliderHandlePosition();

        //fuelSliderImage.color = gradientFuelColor.Evaluate(fuelSlider.normalizedValue);
        //Debug.Log("Height Max Value: " + heightSlider.maxValue);
    }
    /// <summary>
    /// Sets value of height slider with the given float variable
    /// </summary>
    /// <param name="_value"></param>
    public void setHightSliderValue(float _value)
    {
        heightSlider.value = _value;
        setSliderHandlePosition();
        //fuelSliderImage.color = gradientFuelColor.Evaluate(fuelSlider.normalizedValue);
        //Debug.Log("Height Current Value: " + heightSlider.value);
    }
    /// <summary>
    /// Adjust given slider's handle Image Pos
    /// </summary>
    /// <param name="_slider"></param>
    public void setSliderHandlePosition() 
    {
        //float height = heightSlider.transform.Find("Fill Area/Fill").GetComponent<RectTransform>().rect.height;
        //Vector3 tempV = heightSlider.transform.Find("Handle").GetComponent<RectTransform>().anchoredPosition;
        //tempV.y = -height / 2;
        //tempV.y += height * heightSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().fillAmount;
        //heightSlider.transform.Find("Handle").GetComponent<RectTransform>().anchoredPosition = tempV;
    }
    /// <summary>
    /// Sets speedText's text value with the given string
    /// </summary>
    /// <param name="_text"></param>
    public void setSpeedText(string _text)
    {
        speedText.text = _text+"km/s";
        setSpeedColor(_text);
    }
    /// <summary>
    /// Sets heightText's text value with the given string
    /// </summary>
    /// <param name="_text"></param>
    public void setHeightText(string _text)
    {
        heightText.text = _text+"m";
    }
    /// <summary>
    /// Sets speed text color as green, yellow and red 
    /// </summary>
    /// <param name="_text"></param>
    void setSpeedColor(string _text)
    {
        int speed = int.Parse(_text);
        if(speed <= 30)
        {
            speedText.faceColor = Color.green;
        }
        else if(speed <= 50)
        {
            speedText.faceColor = Color.yellow;
        }
        else
        {
            speedText.faceColor = Color.red;
        }
        
    }
}
