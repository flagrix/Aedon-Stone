using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameOver : MonoBehaviour
{

    public AudioSource GameOverSoundSource;  
    public AudioClip GameOverSound;

    public GameObject GameOverUI;
    public bool isGameOver = false;
    public CanvasGroup gameOverCanvas;
    [SerializeField] private GameObject Réticule;

    private void Awake()
    {
    }

    void Start()
    {
        
    }
    void Update()
    {
        Debug.Log("Update appelé");
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("ok");
            TriggerGameOver();
            EndGame();
        }
    }

    public void EndGame()
    {
        Réticule.gameObject.SetActive(false);
        Debug.Log("jhsbdfojqhsdbf");
        //TableauRcords.instance.NewScoreSolo(100);
        isGameOver = true;
        //MusicManager.instance.StopMusic();
        //PlayerMovement.instance.enabled = false;
        //Camera.main.transform.SetParent(null);
        foreach (var player in FindObjectsOfType<PlayerManager>())
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.isGameOver = true;
                break;
            }
        }
        GameOverSoundSource.clip = GameOverSound;
        GameOverSoundSource.Play();
        StartCoroutine(FadeInGameOverUI());


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void TriggerGameOver()
    {

        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Désactiver les contrôles pour le joueur local
        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            if (player.photonView.IsMine)
            {
                player.SetGameOverState(true);
            }
        }
    }


    IEnumerator FadeInGameOverUI()
    {
        GameOverUI.SetActive(true);
        gameOverCanvas.interactable = true;
        gameOverCanvas.blocksRaycasts = true;
        Réticule.SetActive(false);

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime;  
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
