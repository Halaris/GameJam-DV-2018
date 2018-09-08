using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterBaseController
{
    private long MAX_SCORE = 10000;

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
    }

    protected override void Die()
    {
        score -= Mathf.FloorToInt(Mathf.Log10(1 + 9 * currentLifeScore / MAX_SCORE) * score);
        scoreValueText.text = score.ToString();
        currentLifeScore = 0;
        Destroy(gameObject, 0);
    }

    protected override void LoseLife()
    {
		if (lives >= 0) {
			Destroy(lifeImgs[lives]);
		}
    }
}

