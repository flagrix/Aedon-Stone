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

    private bool isSolo = false;
    private bool isInfini = false;

    private float survivalTime = 0f;


    private void Awake()
    {
    }

    void Start()
    {

    }
    void Update()
    {
        if (!isGameOver)
        {
            survivalTime += Time.deltaTime; // Compte les secondes
        }
    }

    public void EndGame()
    {
        Réticule.gameObject.SetActive(false);
        Debug.Log("jhsbdfojqhsdbf");
        Debug.Log("go" + TableauRcords.instance.isInfini);
        Debug.Log("go" + TableauRcords.instance.isSolo);
        if (TableauRcords.instance.isInfini)
        {
            if (TableauRcords.instance.isSolo) {
                Debug.Log("solo"); 
                TableauRcords.instance.NewScoreSolo((int)survivalTime * 10); }
            else
            {
                Debug.Log("ghvsqgvo" + TableauRcords.instance.isSolo);
                TableauRcords.instance.NewScoreDuo((int)survivalTime * 10);
            }
        }
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
