using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterBaseController
{
    private long MAX_SCORE = 10000;
	public static long totalScore;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

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
    }

    protected override void Die()
    {
		// gameObject.SetActive(false);
    }

    protected override void LoseLife()
    {
        score -= Mathf.RoundToInt(Mathf.Log10(1f + 9f * currentLifeScore / MAX_SCORE) * score);
        scoreValueText.text = score.ToString();
        currentLifeScore = 0;
        speed *= 1.3f;
        projectileSpeed *= 1.3f;
        fireRate *= 0.8f;
        mineRate *= 0.8f;
        minesLeft += 1;

        for (int x = 0; x < lifeImgs.Length; x++)
        {
            if(x > lives -1)
            {
                if(x > lives -1)
                {
                    Destroy(lifeImgs[x]);
					updateScore();
                }
            }
        }
    }

	private void updateScore()
	{
		totalScore = 0;
	}
}

