using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour
{
    public void exitGame()
    {
        Application.Quit();
    }
    public void reloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void home()
    {
        SceneManager.LoadScene("startScene");
    }
    public void next()
    {
        int levelName = int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SceneManager.LoadScene((levelName+1).ToString());
    }
}
