using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGameManagerController : MonoBehaviour {

	[SerializeField] private Text scoreText;
	[SerializeField] private int score = 0;

	// Use this for initialization
	void Start () {
		scoreText.text = ("Score: " + score);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
