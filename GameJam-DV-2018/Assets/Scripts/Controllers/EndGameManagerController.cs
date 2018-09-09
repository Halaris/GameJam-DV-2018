using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManagerController : MonoBehaviour {

	[SerializeField] Text titleText;
	[SerializeField] Text subTitleText;
	[SerializeField] Text nameText;
	[SerializeField] Text nameInput;
	[SerializeField] Text creditsTitle;

	// Use this for initialization
	void Start () {
		if (LevelGameManagerController.playerAlive)
		{
			success(LevelGameManagerController.score);
		}
		else {
			fail(LevelGameManagerController.score);
		}
	}

	private void success(long score)
	{
		titleText.text = ("GAME OVER");
		subTitleText.text = ("CONGRATULATIONS");
		nameText.text = ("YOUR SCORE WAS " + score);
		creditsTitle.text = ("WAS PRESENTED TO YOU BY");
		// nameInput.text = ("");
	}

	private void fail(long score)
	{
		titleText.text = ("GAME OVER");
		subTitleText.text = ("YOU FAIL");
		nameText.text = ("YOUR SCORE WAS " + score);
		creditsTitle.text = ("WAS PRESENTED TO YOU BY");
		// nameInput.text = ("");
	}
}
