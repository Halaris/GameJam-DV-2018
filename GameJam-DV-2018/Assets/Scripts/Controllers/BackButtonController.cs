using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButtonController : MonoBehaviour
{

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnMouseUp);
    }

    void OnMouseUp()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }
}
