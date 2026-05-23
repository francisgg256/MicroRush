using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorInicio : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
