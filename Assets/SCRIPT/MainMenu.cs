using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public AudioClip[] musique;
    public AudioSource audiosource;
    private static MainMenu instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Garde la musique entre les scènes
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
            Debug.LogWarning("Le tableau 'musique' est vide ou non assigné dans l'inspecteur.");
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
        SceneManager.LoadScene("SampleScene");
    }

    public void TwoPlayer()
    {
        SceneManager.LoadScene("Loadingscene");
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
        else if (!audiosource.isPlaying) // Reprend la musique si elle a été arrêtée
        {
            audiosource.Play();
        }
    }
}
