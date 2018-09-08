using UnityEngine;
using UnityEngine.UI;

public class ExitButtonController : MonoBehaviour {

    void Start() {
        GetComponent<Button>().onClick.AddListener(OnMouseUp);
    }

    void OnMouseUp() {
        Application.Quit();
    }
}
