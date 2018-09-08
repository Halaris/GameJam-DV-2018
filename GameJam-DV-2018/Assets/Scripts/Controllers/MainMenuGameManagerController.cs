using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameManagerController : MonoBehaviour {

    private string dbPath;

    void Awake ()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/database.db";
        Debug.Log("dbPath: " + dbPath);

        HighScoreService highScoreService = new HighScoreService(dbPath);
        highScoreService.CreateTable();
        if(highScoreService.List().Count < 10)
        {
            for(int i = 10; i > 0; i--)
            {
                highScoreService.Insert(i % 2 == 0 ? "GIT" : "GUD", i * 1000);
            }
        }
    }

}
