using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverUI;
    public static GameOver instance;
    public bool isGameOver = false;
    public CanvasGroup gameOverCanvas;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        PlayerMovement.instance.enabled = false;
        Camera.main.transform.SetParent(null);
        StartCoroutine(FadeInGameOverUI());


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator FadeInGameOverUI()
    {
        GameOverUI.SetActive(true);
        gameOverCanvas.interactable = true;
        gameOverCanvas.blocksRaycasts = true;

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            gameOverCanvas.alpha = alpha;
            yield return null;
        }
    }

        public void QuitterButton()
    {
        Application.Quit();
        GameOverUI.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
        GameOverUI.SetActive(false);
    }

    public void RejouerButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOverUI.SetActive(false);
        isGameOver = false;
    }
}
