using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreButtonController : MonoBehaviour
{

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnMouseUp);
    }

    void OnMouseUp()
    {
        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Single);
    }
}
