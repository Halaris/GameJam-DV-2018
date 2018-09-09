using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManagerController : MonoBehaviour {

	[SerializeField] Text titleText;
	[SerializeField] Text subTitleText;
	[SerializeField] Text nameText;
	[SerializeField] InputField nameInput;
	[SerializeField] Text creditsTitle;
	public static string item;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
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
	}

	private void Update()
	{
		if (nameInput.text != null && nameInput.text != "" && Input.GetKeyDown(KeyCode.Return)) {

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
}
