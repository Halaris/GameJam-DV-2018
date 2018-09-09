using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public int damage;
    public string enemyTag;
	public string sourceTag;
		
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!(collision.gameObject.tag == sourceTag) && !(collision.gameObject.tag == gameObject.tag))
		{
			if (collision.gameObject.tag == enemyTag)
			{
				CharacterBaseController enemyChar = collision.gameObject.GetComponent<CharacterBaseController>();
				if (enemyChar != null)
				{
					enemyChar.GetDamaged(damage);
				}
			}
            if(collision.gameObject.layer == 10 || collision.gameObject.layer == 8 || collision.gameObject.layer == 11)
            {
				Destroy(gameObject);
            }
			    
		}	
	}
}
