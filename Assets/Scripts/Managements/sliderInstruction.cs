using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderInstruction : MonoBehaviour
{
    [SerializeField] GameObject insAnim;
    bool animPlaying=false;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("instructionPlayed", 0);
        if (PlayerPrefs.GetInt("instructionPlayed",0) == 0)
        {
            animPlaying = true;
            insAnim.SetActive(true);
            StartCoroutine(instructionAnimRoutine());
        }
        else
        {
            insAnim.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && animPlaying)
        {
            clickedToObject();
        }
    }
    void clickedToObject()
    {
        Debug.Log("Clicked Object");
        Debug.Log("Anim Stop");
        PlayerPrefs.SetInt("instructionPlayed", 1);
        insAnim.SetActive(false);
        StopCoroutine(instructionAnimRoutine());
        animPlaying = false;
        this.gameObject.SetActive(false);
    }
    IEnumerator instructionAnimRoutine()
    {
        yield return new WaitForSeconds(5f);
        insAnim.SetActive(false);
        PlayerPrefs.SetInt("instructionPlayed", 1);
        animPlaying = false;
        this.gameObject.SetActive(false);
    }
}
