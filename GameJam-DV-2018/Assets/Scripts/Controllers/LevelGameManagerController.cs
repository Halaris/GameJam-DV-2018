using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGameManagerController : MonoBehaviour {
    public float minSize = 1f;
    public float maxSize = 6f;
    public float sensitivity = 4f;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform[] spawners;
    private float dificulty =( 1f / 3f);


    void Update ()
    {
        float size = Camera.main.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * (-sensitivity);
        size = Mathf.Clamp(size, minSize, maxSize);
        Camera.main.orthographicSize = size;
        if(player.score >= (player.MAX_SCORE * dificulty))
        {
            
            foreach (Transform spawn in spawners)
            {
                if(!spawn.gameObject.active)
                {
                    spawn.gameObject.active = true;
                    dificulty += 1f / 3f;
                    break;
                }
            }
        }
    }
}
