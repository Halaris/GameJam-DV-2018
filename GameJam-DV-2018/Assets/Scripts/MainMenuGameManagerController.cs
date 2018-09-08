using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameManagerController : MonoBehaviour {

    private string dbPath;

    // Use this for initialization
    void Awake ()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/database.db";
        Debug.Log("dbPath: " + dbPath);

        HighScoreService highScoreService = new HighScoreService(dbPath);
        highScoreService.CreateTable();
    }

}
