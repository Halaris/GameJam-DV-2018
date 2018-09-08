using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private float cameraHeight = 20.0f;
    public GameObject player;
    private float maxDistanceFromPlayer = 20f;

    private void Awake()
    {
        cameraHeight = this.transform.position.z;
    }

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if(player != null)
        {
            Vector3 playerPos = player.transform.position;
            playerPos.z = 0;
            Vector3 cameraPos = this.transform.position;
            cameraPos.z = 0;
            float distance = Vector3.Distance(playerPos, cameraPos);
            float proportionOfDistance = (distance < maxDistanceFromPlayer ? distance / maxDistanceFromPlayer : 1);
            cameraPos += (playerPos - cameraPos) * (proportionOfDistance);
            cameraPos.z = cameraHeight;
            transform.position = cameraPos;
        }
    }
}
