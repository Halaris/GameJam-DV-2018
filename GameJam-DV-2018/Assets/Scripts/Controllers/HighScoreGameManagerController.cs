using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreGameManagerController : MonoBehaviour {

    public GameObject highScoreList;

    private string dbPath;

    void Awake()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/database.db";

        HighScoreService highScoreService = new HighScoreService(dbPath);
        List<HighScore> highScores = highScoreService.List();

        Image[] highScoreRows = highScoreList.GetComponentsInChildren<Image>();

        int i = 0;
        foreach (HighScore highScore in highScores)
        {
            Debug.Log("name: " + highScore.GetName() + "; score: " + highScore.GetScore());
            for (; i < highScoreRows.Length && !highScoreRows[i].gameObject.CompareTag("HighScoreRow"); i++);
            if(i < highScoreRows.Length)
            {
                Text[] highScoreFields = highScoreRows[i].gameObject.GetComponentsInChildren<Text>();
                highScoreFields[0].text = highScore.GetName();
                highScoreFields[1].text = highScore.GetScore().ToString();
                i++;
            }
        }
    }
}
