using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using RK.Namespace.Security;
public static class PlayerInfos
{
    #region variables
    public static List<levelInfos> levelInfoList = new List<levelInfos>();
    [System.Serializable]
    public struct levelInfos
    {
        public string levelName;
        public bool isLock;
        public int starCount;
    }
    #endregion

    /// <summary>
    /// Covert levelinfolist content to a json string and saves it to the given path
    /// </summary>
    public static void saveJson()
    {
        var json = JsonConvert.SerializeObject(levelInfoList);
        json = Cryptography.EnCrypt(json, "^1662QUef%9b");
        File.WriteAllText(Application.persistentDataPath + "levelInfo.json", json);
        readJson();
    }
    /// <summary>
    /// Reads the json file from the given path if exist if not creates new one and turn the read file into a levelInfos object then assign it to the levelinfo list
    /// </summary>
    public static void readJson()
    {
        var json="";
        if (File.Exists(Application.persistentDataPath + "levelInfo.json"))
        {
            levelInfoList.Clear();
            json = File.ReadAllText(Application.persistentDataPath + "levelInfo.json");
            json=Cryptography.DeCrypt(json, "^1662QUef%9b");
            levelInfoList = JsonConvert.DeserializeObject<List<levelInfos>>(json);
        }
        else
        {
            saveJson();
        }
    }
    /// <summary>
    /// Deletes json file and calls saveJson to create new one
    /// </summary>
    public static void deleteJson()
    {
        File.Delete(Application.persistentDataPath + "levelInfo.json");
        if(!File.Exists(Application.persistentDataPath + "levelInfo.json"))
        {
            levelInfoList.Clear();
            saveJson();
        }
    }
}
