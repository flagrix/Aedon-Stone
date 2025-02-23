using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public AudioSource GameOverSoundSource;  
    public AudioClip GameOverSound;

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
        Debug.Log("jhsbdfojqhsdbf");
        TableauRcords.instance.NewScoreSolo(100);
        isGameOver = true;
        PlayerMovement.instance.enabled = false;
        Camera.main.transform.SetParent(null);
        GameOverSoundSource.clip = GameOverSound;
        GameOverSoundSource.Play();
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
        isGameOver = false;
        GameOverUI.SetActive(false);
    }

    public void RejouerButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOverUI.SetActive(false);
        isGameOver = false;
    }
}
