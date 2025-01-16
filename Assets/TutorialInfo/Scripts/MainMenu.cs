using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MenuJoueur()
    {
        SceneManager.LoadScene(2);
    }

    public void MenuMode()
    {
        SceneManager.LoadScene(3);
    }
}
