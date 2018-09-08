using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameButtonController : MonoBehaviour {

    void Start() {
        GetComponent<Button>().onClick.AddListener(OnMouseUp);
    }

    void OnMouseUp() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
