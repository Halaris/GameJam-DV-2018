using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreGameManagerController : MonoBehaviour {

    public GameObject highScoreList;
    public GameObject highScoreRowPrefab;

    private string dbPath;

    void Awake()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/database.db";

        HighScoreService highScoreService = new HighScoreService(dbPath);
        List<HighScore> highScores = highScoreService.List();
        foreach (HighScore highScore in highScores)
        {
            Debug.Log("name: " + highScore.GetName() + "; score: " + highScore.GetScore());
            GameObject highScoreRow = Instantiate(highScoreRowPrefab, highScoreList.transform, false);
            Text[] highScoreFields = highScoreRow.GetComponentsInChildren<Text>();
            highScoreFields[0].text = highScore.GetName();
            highScoreFields[1].text = highScore.GetScore().ToString();
        }
    }
}
