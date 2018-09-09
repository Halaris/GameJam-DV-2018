using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterBaseController
{
    public long MAX_SCORE = 1000;

    [SerializeField] private GameObject[] lifeImgs;

    protected override void CharacterUpdate()
    {
        //Obtain directional movement defined on the inputs
        float verticalSpeed = Input.GetAxis("Vertical");
        float horizontalSpeed = Input.GetAxis("Horizontal");
        //Update speed based on the directional movement and the defined movement speed
        rig.velocity = new Vector2(horizontalSpeed, verticalSpeed) * speed;
        // shoot a projectile
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            DropMine();
        }
        if (score >= MAX_SCORE)
        {
            LevelGameManagerController.score = score;
            LevelGameManagerController.playerAlive = true;
            SceneManager.LoadScene("EndGame", LoadSceneMode.Single);
        }
    }

    protected override void Die()
    {
        LevelGameManagerController.score = score;
        LevelGameManagerController.playerAlive = false;
        SceneManager.LoadScene("EndGame", LoadSceneMode.Single);
    }
    public void updateLife()
    {
        for (int x = 0; x < lifeImgs.Length; x++)
        {
			lifeImgs[x].SetActive(!(x > lives - 1));
        }
    }
    protected override void LoseLife()
    {
        score -= Mathf.RoundToInt(Mathf.Log10(1f + 9f * currentLifeScore / MAX_SCORE) * score);
        scoreValueText.text = score.ToString();
        currentLifeScore = 0;
        speed *= 1.1f;
        projectileSpeed *= 1.2f;
        fireRate *= 0.8f;
        mineRate *= 0.8f;
        minesLeft += 1;
        updateLife();
    }
}

