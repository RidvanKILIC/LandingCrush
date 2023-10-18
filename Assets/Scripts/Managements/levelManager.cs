using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelManager : MonoBehaviour
{
    #region Variables
    [Header("Star Objects & Variables")]
    int starCount;
    List<GameObject> stars = new List<GameObject>();
    [Header("Lock Objects & Variables")]
    GameObject lockObject;
    bool isLocked;
    [Header("TextObject & Variables")]
    TMPro.TMP_Text text;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    /// <summary>
    /// Initiliazes objects & references
    /// </summary>
    public void Init()
    {
        PlayerInfos.readJson();

        GetComponent<Button>().onClick.AddListener(() => loadLevel());
        lockObject = transform.Find("Lock").gameObject;
        text = transform.Find("text").GetComponent<TMPro.TMP_Text>();

        ///Get stars
        GameObject starParent = transform.Find("Star").gameObject;
        if (starParent != null)
        {
            for (int i = 0; i < starParent.transform.childCount; i++)
            {
                if (i >= 3)
                    stars.Add(starParent.transform.GetChild(i).gameObject);
            }
        }

        ///

        //starCount = PlayerPrefs.GetInt(this.gameObject.name);
        //Debug.Log(this.gameObject.name + " " + PlayerPrefs.GetInt(this.gameObject.name));
        //PlayerPrefs.SetString("1", "Unlock");
        //PlayerPrefs.SetString("2", "Unlock");
        //PlayerPrefs.SetString("3", "Unlock");
        //PlayerPrefs.SetString("4", "Unlock");

        var levelInfo = PlayerInfos.levelInfoList.Find(x => x.levelName.Equals(this.gameObject.name));
        if(!string.IsNullOrEmpty(levelInfo.levelName))
        {
            starCount = levelInfo.starCount;
            isLocked = levelInfo.isLock;
        }
        else
        {
            starCount = 0;
            if (this.gameObject.name.Equals("1"))
                isLocked = false;
            else
                isLocked = true;
        }
        //if (PlayerPrefs.GetString(this.gameObject.name) == "Unlock")
        //{
        //    lockLevel = false;
        //}
        //else
        //{
        //    lockLevel = true;
        //}
        setText();
        setStar();
        lockUnlockItem();
    }
    void setText()
    {
        text.text = "LEVEL "+this.gameObject.name.Replace("_", " ").ToUpper();
    }
    /// <summary>
    /// Activates stars attached objects
    /// </summary>
    void setStar()
    {
        if (starCount > 0 && starCount < 4)
        {
            for (int i = 0; i < starCount; i++)
            {
                stars[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].SetActive(false);
            }
        }

    }
    /// <summary>
    /// Activates Deactivates lock panel
    /// </summary>
    void lockUnlockItem()
    {
        if (isLocked)
            lockObject.SetActive(true);
        else
            lockObject.SetActive(false);
    }
    /// <summary>
    /// Load the level which shares the same name with the object
    /// </summary>
    void loadLevel()
    {
        if(!isLocked)
            UnityEngine.SceneManagement.SceneManager.LoadScene(this.gameObject.name);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
