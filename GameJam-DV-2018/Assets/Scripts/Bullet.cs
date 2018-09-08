using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    public string enemyTag;

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if(collision.gameObject.tag == enemyTag)
        {
            CharacterBaseController enemyChar = collision.gameObject.GetComponent<CharacterBaseController>();
            Debug.Log(enemyChar);
            if(enemyChar != null)
            {
                enemyChar.GetDamaged(damage);
            }
        }
		Destroy(gameObject, 0);
	}

}
