﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGameManagerController : MonoBehaviour {
    public float minSize = 1f;
    public float maxSize = 6f;
    public float sensitivity = 4f;
	public static long score = 0;
	public static bool playerAlive = true;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform pauseScreen;
    [SerializeField] private Transform[] spawners;
    private float dificulty =( 1f / 3f);
    public static bool pause = false;

	private void Start()
	{
		DontDestroyOnLoad(this);
	}

	void Update ()
    {
        if(Input.GetButtonDown("Jump"))
        {
            LevelGameManagerController.pause = !LevelGameManagerController.pause;
            if (LevelGameManagerController.pause)
            {
                Time.timeScale = 0;
                pauseScreen.gameObject.SetActive(LevelGameManagerController.pause);
            } else
            {
                pauseScreen.gameObject.SetActive(LevelGameManagerController.pause);
                Time.timeScale = 1;
            }
            
        } else if (LevelGameManagerController.pause && Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        } else
        {
            float size = Camera.main.orthographicSize;
            size += Input.GetAxis("Mouse ScrollWheel") * (-sensitivity);
            size = Mathf.Clamp(size, minSize, maxSize);
            Camera.main.orthographicSize = size;
            if (player.score >= (player.MAX_SCORE * dificulty))
            {

                foreach (Transform spawn in spawners)
                {
                    
                     spawn.GetComponent<EnemySpawnerController>().maxQuantityEnemy++;
                }
                dificulty += 1f / 3f;
            }
        }
        
    }
}
