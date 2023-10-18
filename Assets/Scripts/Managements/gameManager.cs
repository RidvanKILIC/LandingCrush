using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    #region Variables
    [Header("Balonn variables & References")]
    [SerializeField] bool startFromRandomPos = false;
    Baloon _baloon;
    bool updateHUDVariables=true;
    int currentPace=2;
    [Header("Camera Variables")]
    public bool moveCamera=false;
    CameraFollow _cameraFollow;
    [Header("Obstacle Objects & Variables")]
    [SerializeField] GameObject birdPrefab;
    [SerializeField] GameObject spawnCube;
    [SerializeField] GameObject windFx;
    GameObject target;
    [Header("Pause Game Variables")]
    public bool gamePaused = false;
    [Header("End Game Variables")]
    public bool landed = false;
    public bool onTarget = false;
    public bool validSpeed = false;
    public bool ranOutGas = false;
    public bool onLand = false;
    public bool gameStarted = false;
    public bool gameEnded = false;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] List<GameObject> starList = new List<GameObject>();
    enum Result { Success,NotOnTarget,Crashed,Default}
    struct levelGrade { public int numberOfStar;}
    levelGrade fuelConsumption=new levelGrade();
    levelGrade finalVelocity=new levelGrade();
    levelGrade targetGrade = new levelGrade();
    Result currentResult = new Result();
    [SerializeField] TMPro.TMP_Text endingText;
    //[SerializeField] GameObject fireWorks;
    //[SerializeField] GameObject explosion;
    [Header("Panels & UI Elements")]
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject gameOverPanel;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Init();
    }
    /// <summary>
    /// Initiliazes objects and references
    /// </summary>
    void Init()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        _cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
        _baloon = GameObject.FindObjectOfType<Baloon>();

        //startPanel.SetActive(true);
        //gamePanel.SetActive(false);

        //UIManager.UInstance.setFuelSliderMaxValue(_baloon.getFuel());
        //UIManager.UInstance.setSpeedText(_baloon.getSpeed().ToString());
        //UIManager.UInstance.setHeightSliderMaxValue(_baloon._baloonMovement.maxY.transform.position.y);
        //UIManager.UInstance.setHightSliderValue(_baloon.getHeight());
        UIManager.UInstance.setSpeedSliderMaxValue(_baloon._baloonMovement.maxVerticalSpeed);
        UIManager.UInstance.setHeightText(_baloon.getHeight().ToString());
        if (startFromRandomPos)
        {
            _baloon.gameObject.transform.position = getRandomPos();
        }
        InvokeRepeating("spawnObstackle", 1f, 15f);
        startGame();
    }
    public void startRoutine()
    {
        gamePanel.SetActive(true);
        startPanel.SetActive(false);
        moveCamera = true;
    }
    /// <summary>
    /// Starts the game
    /// </summary>
    public void startGame()
    {
        //Debug.Log("StartGame");
        gamePanel.SetActive(true);
        startPanel.SetActive(false);
        gameStarted = true;
        _baloon.rb.useGravity = true;
    }
    Vector3 getRandomPos()
    {
        Vector3 _pos = Vector3.zero;
        float posX = Random.Range(_baloon._baloonMovement.minX.transform.position.x, _baloon._baloonMovement.maxX.transform.position.x);
        float posZ = Random.Range(_baloon._baloonMovement.minZ.transform.position.z, _baloon._baloonMovement.maxZ.transform.position.z);

        float randomX = Random.Range(_baloon.transform.position.x - 200, _baloon.transform.position.x + 200);
        float randomZ = Random.Range(_baloon.transform.position.z - 200, _baloon.transform.position.z + 200);

        Mathf.Clamp(randomX, _baloon._baloonMovement.minX.transform.position.x, _baloon._baloonMovement.maxX.transform.position.x);
        Mathf.Clamp(randomZ, _baloon._baloonMovement.minZ.transform.position.z, _baloon._baloonMovement.maxZ.transform.position.z);

        Vector3 randomPos =new Vector3(randomX,_baloon.gameObject.transform.position.y, randomZ);
       
        _pos = new Vector3(posX, _baloon.gameObject.transform.position.y, posZ);
        return randomPos /*_pos*/ ;
    }
    // Update is called once per frame
    void Update()
    {
        if (!landed)
        {
            //UIManager.UInstance.setFuelSliderValue(_baloon.getFuel());
            //UIManager.UInstance.setHightSliderValue(_baloon.getHeight());
            UIManager.UInstance.setHeightText(_baloon.getHeight().ToString());
            UIManager.UInstance.setSpeedSliderValue(_baloon.getSpeed());
            setThePace();
        }
        else if(landed & updateHUDVariables)
        {
            SoundManager.SInstance.playThemeSound(0);
            _baloon.activateDeactivateSpeedFx(0);
            UIManager.UInstance.setHeightText("0");
            //UIManager.UInstance.setHightSliderValue(0);
            UIManager.UInstance.setSpeedSliderValue(0);
            updateHUDVariables = false;
        }
    }
    /// <summary>
    /// According to the speed of the player changes the theme music
    /// </summary>
    void setThePace()
    {
        if (!landed)
        {
            if (_baloon.getFuel() <= 0)
            {
                ranOutGas = true;
                gameOver(_baloon.getSpeed(), _baloon.getFuel());
            }
            int _changedPace = currentPace;
            if (_baloon.getSpeed() > (_baloon._baloonMovement.maxVerticalSpeed * 0.7) && _baloon.rb.velocity.y < 0)
            {
                currentPace = 1;
            }
            else if (_baloon.getSpeed() <= (_baloon._baloonMovement.maxVerticalSpeed * 0.7) && _baloon.rb.velocity.y < 0)
            {
                currentPace = 0;
            }
            if (_changedPace != currentPace)
            {
                SoundManager.SInstance.playThemeSound(currentPace);
                _baloon.activateDeactivateSpeedFx(currentPace);
            }
        }

    }
    /// <summary>
    /// End Game fuhnction decides which coroutine will be executed
    /// </summary>
    public void gameOver(float lastSpeed,float fuelRemain,string targetColor="White" )
    {
        if (!gameEnded)
        {
            
            gameEnded = true;
            // Default value
            finalVelocity.numberOfStar = 0;
            fuelConsumption.numberOfStar = 0;

            if (onTarget && validSpeed && !ranOutGas)
            {
                //Debug.Log("Properly Landed");
                getSpeedGrade(lastSpeed);
                getFuelGrade(fuelRemain);
                getTargetGrade(targetColor);
                currentResult = Result.Success;
            }
            else if (!onTarget && landed && validSpeed && !ranOutGas)
            {
                //Debug.Log("Landed wrong place!");
                currentResult = Result.NotOnTarget;
            }
            else if (!validSpeed && !ranOutGas)
            {
                //Debug.Log("Crashed!");
                currentResult = Result.Crashed;
            }
            else
            {
                //Debug.Log("Dropeed in the see");
                currentResult = Result.Default;
                SoundManager.SInstance.playSfx(SoundManager.SInstance.fail);
                endingText.text = "Try Again!";
                gameOverPanel.SetActive(true);
                Time.timeScale = 0;
            }
            if(currentResult != Result.Default)
            {
                SoundManager.SInstance.playPauseTheme(false);
                int starCount = /*finalVelocity.numberOfStar;*/ Mathf.FloorToInt((finalVelocity.numberOfStar + targetGrade.numberOfStar) / 2);
                //Debug.Log("Earned Star Count :" + starCount);
                StartCoroutine(endingRoutine(currentResult,starCount));
            }
                
        }

    }
    /// <summary>
    /// Arranges grade according to the given parameter
    /// </summary>
    /// <param name="lastSpeed"></param>
    void getSpeedGrade(float lastSpeed)
    {
        ///Decide speeed Star
        if (lastSpeed <= (_baloon._baloonMovement.maxVerticalSpeed * 0.3))
            finalVelocity.numberOfStar = 3;
        else if (lastSpeed > (_baloon._baloonMovement.maxVerticalSpeed * 0.3) && lastSpeed <= (_baloon._baloonMovement.maxVerticalSpeed * 0.7))
            finalVelocity.numberOfStar = 2;
        else
            finalVelocity.numberOfStar = 1;

        Debug.Log("Velocity: " + lastSpeed+ finalVelocity.numberOfStar + " star");
    }
    /// <summary>
    /// Arranges grade according to the given parameter
    /// </summary>
    /// <param name="remainFuel"></param>
    void getFuelGrade(float remainFuel)
    {
        if (remainFuel >= (_baloon.Maxfuel * 0.6f))
        {
            fuelConsumption.numberOfStar = 3;
            //Debug.Log("remain fuel:" + remainFuel + " %60 of fuel: " + (_baloon.Maxfuel * 0.6f));
        }   
        else if (remainFuel < (_baloon.Maxfuel * 0.6f) && remainFuel > (_baloon.Maxfuel * 0.4f))
        {
            //Debug.Log("remain fuel:" + remainFuel + " 40 of fuel: " + (_baloon.Maxfuel * 0.4f));
            fuelConsumption.numberOfStar = 2;
        }
        else
        {
            fuelConsumption.numberOfStar = 1;
            //Debug.Log("remain fuel:" + remainFuel + " 20 of fuel: " + (_baloon.Maxfuel * 0.2f));
        }
 

        //Debug.Log("Reamining fuel: " + remainFuel+ fuelConsumption.numberOfStar + "star");
    }
    /// <summary>
    /// Arranges grade according to the given parameter
    /// </summary>
    /// <param name="remainFuel"></param>
    void getTargetGrade(string targetColor)
    {
        switch (targetColor)
        {
            case "Green":
                targetGrade.numberOfStar = 3;
                break;
            case "Yellow":
                targetGrade.numberOfStar = 2;
                break;
            case "Red":
                targetGrade.numberOfStar = 1;
                break;
            default:
                targetGrade.numberOfStar = 0;
                break;
        }

    }
    /// <summary>
    /// Follows Routine which is decided by the parameter
    /// </summary>
    /// <param name="_result"></param>
    /// <returns></returns>
    IEnumerator endingRoutine(Result _result,int starCount)
    {
        
        if (_result.Equals(Result.Success))
        {

            #region Save To Level Info List
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "4")
            {
                exitButton.SetActive(false);
                nextButton.SetActive(true);
            }
            else
            {
                exitButton.SetActive(true);
                nextButton.SetActive(false);
            }
            PlayerInfos.levelInfos levelInfo = new PlayerInfos.levelInfos();
            levelInfo.levelName = "";
            levelInfo = PlayerInfos.levelInfoList.Find(x => x.levelName.Equals(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
            if (!string.IsNullOrEmpty(levelInfo.levelName))
            {

                if (levelInfo.starCount < starCount)
                {
                    PlayerInfos.levelInfos newItem = new PlayerInfos.levelInfos();
                    newItem.levelName = levelInfo.levelName;
                    newItem.starCount = starCount;
                    newItem.isLock = levelInfo.isLock;
                    PlayerInfos.levelInfoList.Remove(levelInfo);
                    PlayerInfos.levelInfoList.Add(newItem);
                }

            }
            else
            {
                PlayerInfos.levelInfos levelInfoObject = new PlayerInfos.levelInfos();
                levelInfoObject.levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                levelInfoObject.starCount = starCount;
                levelInfoObject.isLock = false;
                PlayerInfos.levelInfoList.Add(levelInfoObject);
            }

            

            int currentLevel = int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            var nextLevelInfo = PlayerInfos.levelInfoList.Find(x => x.levelName.Equals((currentLevel + 1).ToString()));
            if (!string.IsNullOrEmpty(nextLevelInfo.levelName))
            {
                if (nextLevelInfo.isLock)
                {
                    PlayerInfos.levelInfos newItem = new PlayerInfos.levelInfos();
                    newItem.levelName = nextLevelInfo.levelName;
                    newItem.starCount = nextLevelInfo.starCount;
                    newItem.isLock = false;
                    PlayerInfos.levelInfoList.Remove(nextLevelInfo);
                    PlayerInfos.levelInfoList.Add(newItem);
                }
                    
            }
            else
            {
                PlayerInfos.levelInfos levelInfoObject = new PlayerInfos.levelInfos();
                levelInfoObject.levelName = (currentLevel+1).ToString();
                levelInfoObject.isLock = false;
                levelInfoObject.starCount = 0;
                PlayerInfos.levelInfoList.Add(levelInfoObject);
            }

            PlayerInfos.saveJson();
            TinySauce.OnGameFinished(true, starCount, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            #endregion
            //if (PlayerPrefs.GetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 0) <= starCount)
            //{

            //    PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, starCount);
            //    Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " +
            //        PlayerPrefs.GetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 0));
            //}
            //int name = int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            //PlayerPrefs.SetString((name + 1).ToString(), "Unlock");
            //Debug.Log((name + 1).ToString() + " Unlocked");

            //fireWorks.SetActive(true);
            SoundManager.SInstance.playSfx(SoundManager.SInstance.sucess);
            endingText.text = "TEBRÝKLER!";
            yield return new WaitForSeconds(1f);
            gameOverPanel.SetActive(true);

            if(starCount>2)
                retryButton.SetActive(false);

            yield return new WaitForSeconds(0.2f);
            for (int i =0; i < starCount ; i++)
            {
                SoundManager.SInstance.playSfx(SoundManager.SInstance.star);
                starList[i].gameObject.GetComponent<Animator>().Play("starAnim");
                yield return new WaitForSeconds(1f);
            }
        }
        else if(_result.Equals(Result.Crashed))
        {
            checkNextLevelAvailability();
            //explosion.SetActive(true);
            SoundManager.SInstance.playSfx(SoundManager.SInstance.crash);
            endingText.text = "KAYBETTÝNÝZ!";
            yield return new WaitForSeconds(1f);
            gameOverPanel.SetActive(true);
        }
        else
        {
            checkNextLevelAvailability();
            SoundManager.SInstance.playSfx(SoundManager.SInstance.fail);
            endingText.text = "KAYBETTÝNÝZ!";
            yield return new WaitForSeconds(1f);
            gameOverPanel.SetActive(true);
        }
        yield return null;
    }
    void checkNextLevelAvailability()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "4")
        {
            exitButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else
        {
            PlayerInfos.levelInfos levelInfo = new PlayerInfos.levelInfos();
            levelInfo.levelName = "";
            int levelName = int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            levelInfo = PlayerInfos.levelInfoList.Find(x => x.levelName.Equals((levelName + 1).ToString()));
            if (!string.IsNullOrEmpty(levelInfo.levelName))
            {
                if (levelInfo.levelName == (levelName + 1).ToString() && !levelInfo.isLock)
                {
                    exitButton.SetActive(false);
                    nextButton.SetActive(true);
                }
                else
                {
                    exitButton.SetActive(true);
                    nextButton.SetActive(false);
                }
            }
            else
            {
                exitButton.SetActive(true);
                nextButton.SetActive(false);
            }
        }

 
    }
    /// <summary>
    /// Spawn obstackles, set Animations and assign targets  
    /// </summary>
    void spawnObstackle()
    {
        if(gameStarted && !_baloon.landed)
        {
            Vector3 pos = getCubesEdgePositions();
            StartCoroutine(spawnOstacleRoutine(pos));
        }

    }
    IEnumerator spawnOstacleRoutine(Vector3 pos)
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject windObj = Instantiate(windFx, new Vector3(pos.x, (pos.y - (10 * i + 1)), pos.z), Quaternion.identity);
            windObj.transform.localScale = Vector3.one;/* new Vector3(25f, 25f, 25f);*/
            //windObj.transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            //Destroy(windObj, 10f);
        }

        yield return new WaitForSeconds(6f);

        GameObject obstacle = Instantiate(birdPrefab, pos, Quaternion.identity);
        //obstacle.GetComponent<obstacle>().target = _baloon.gameObject;
        for (int i = 0; i < obstacle.transform.childCount; i++)
        {
            obstacle.transform.GetChild(i).GetComponent<Animator>().SetBool("flying", true);
            obstacle.transform.GetChild(i).GetComponent<Animator>().SetFloat("flyingDirectionX", 0);
            obstacle.transform.GetChild(i).GetComponent<Animator>().SetFloat("flyingDirectionY", 0);
            //Debug.Log("horde Spawned");
        }

    }
    /// <summary>
    /// Return random positions of the edges of the cube
    /// </summary>
    /// <returns></returns>
    Vector3 getCubesEdgePositions()
    {

        Collider collider = spawnCube.GetComponent<BoxCollider>();

        //get the extents
        var xHalfExtents = collider.bounds.extents.x;
        //get the center
        var xCenter = collider.bounds.center.x;
        //get the up border
        float xUpper = transform.position.y + (xCenter + xHalfExtents);
        //get the lower border
        float xLower = transform.position.y + (xCenter - xHalfExtents);
        //Debug.Log("Upper border: " + xUpper);
        //Debug.Log("Lower border: " + xLower);

        //get the extents
        var yHalfExtents = collider.bounds.extents.y;
        //get the center
        var yCenter = collider.bounds.center.y;
        //get the up border
        float yUpper = transform.position.y + (yCenter + yHalfExtents);
        //get the lower border
        float yLower = transform.position.y + (yCenter - yHalfExtents);
        //Debug.Log("Upper border: " + yUpper);
        //Debug.Log("Lower border: " + yLower);

        //get the extents
        var zHalfExtents = collider.bounds.extents.z;
        //get the center
        var zCenter = collider.bounds.center.z;
        //get the up border
        float zUpper = transform.position.y + (zCenter + zHalfExtents);
        //get the lower border
        float zLower = transform.position.y + (zCenter - zHalfExtents);
        //Debug.Log("Upper border: " + zUpper);
        //Debug.Log("Lower border: " + zLower);

        int randZ = Random.Range(0, 2);
        int randX = Random.Range(0, 2);
        float xValue = randX == 0 ? xUpper : xLower;
        float zValue = randZ == 0 ? zCenter : zCenter;
        return new Vector3(xValue/*Random.Range(xLower, xUpper)*/, Random.Range(yLower, _baloon.transform.position.y/*yUpper*/), zValue/*Random.Range(zLower, zUpper)*/);
    }


}
