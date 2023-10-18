using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSources : MonoBehaviour
{
    public AudioSource themeAs;
    public AudioSource sFXAs;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        themeAs = transform.Find("ThemeAS").GetComponent<AudioSource>();
        sFXAs = transform.Find("SFXAS").GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void destroyObject()
    {
        Destroy(this.gameObject);
    }
}
