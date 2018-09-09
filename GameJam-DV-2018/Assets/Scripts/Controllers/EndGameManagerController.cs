using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManagerController : MonoBehaviour {

	[SerializeField] Text titleText;
	[SerializeField] Text subTitleText;
	[SerializeField] Text nameText;
	[SerializeField] InputField nameInput;
	[SerializeField] Text creditsTitle;

	// Use this for initialization
	void Start () {
		titleText.text = ("GAME OVER");
		nameText.text = ("YOUR SCORE WAS " + LevelGameManagerController.score);
		creditsTitle.text = ("WAS PRESENTED TO YOU BY");
		nameInput.text = "";
		if (LevelGameManagerController.playerAlive)
		{
			success();
		}
		else {
			fail();
		}
        nameInput.onValidateInput += delegate (string input, int charIndex, char addedChar) {
            return ValidateForcesUppercase(addedChar);
        };
    }

    private char ValidateForcesUppercase(char charToValidate)
    {
        return charToValidate.ToString().ToUpper()[0];
    }

    private void Update()
	{
		if (nameInput.text != null && nameInput.text != "" && (Input.GetButtonDown("Submit"))) {
			persistScore(nameInput.text, LevelGameManagerController.score);
			Destroy(GameObject.Find("LevelGameManagerController"));
			SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Single);
		}
	}

	private void success()
	{
		subTitleText.text = ("CONGRATULATIONS");
	}

	private void fail()
	{
		subTitleText.text = ("YOU FAIL");
	}

	private void persistScore(string name, long value) {
		string dbPath = "URI=file:" + Application.persistentDataPath + "/database.db";
		Debug.Log("dbPath: " + dbPath);
		HighScoreService highScoreService = new HighScoreService(dbPath);
		highScoreService.Insert(name, value);
	}
}
