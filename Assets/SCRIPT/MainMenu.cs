using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
public class MainMenu : MonoBehaviourPunCallbacks
{
    public AudioClip[] musique;
    public AudioSource audiosource;
    private static MainMenu instance;
    public GameObject? echapMenu=null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Garde la musique entre les sc�nes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

    }
    private void Start()
    {
        if (musique != null && musique.Length > 0)
        {
            audiosource.clip = musique[0];
            audiosource.loop = true;
            audiosource.Play();
        }
        else
        {
            Debug.LogWarning("Le tableau 'musique' est vide ou non assign� dans l'inspecteur.");
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("ChoixJoueurs");
    }

    public void MenuJoueur()
    {
        SceneManager.LoadScene("ChoixMode");
    }

    public void OnePlayer()
    {
        TableauRcords.instance.isSolo = true;
        SceneManager.LoadScene("Loadingscene");
    }

    public void TwoPlayer()
    {
        SceneManager.LoadScene("Loadingscene");
    }
    public void LeaveGame()
    {
        Application.Quit();
    }
    public void BackStart()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main Menu");
        PhotonNetwork.Disconnect();
    }
    public void retour()
    {
        if (echapMenu != null)
        {
            echapMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene" || scene.name == "Jeu multi") 
        {
            if (audiosource.isPlaying)
            {
                audiosource.Stop(); 
            }
        }
        else if (!audiosource.isPlaying) // Reprend la musique si elle a �t� arr�t�e
        {
            audiosource.Play();
        }
    }
}
