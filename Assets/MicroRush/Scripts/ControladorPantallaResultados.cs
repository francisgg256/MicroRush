using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorResultados : MonoBehaviour
{
    public void OnBotonMenu()
    {
        SceneManager.LoadScene("MenuNuevo");
    }

    public void OnBotonRanking()
    {
        SceneManager.LoadScene("RankingNuevo");
    }
}