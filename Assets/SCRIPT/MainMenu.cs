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
        audiosource.clip = musique[0];
        audiosource.loop = true;
        audiosource.Play();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("ChoixJoueurs");
    }

    public void MenuJoueur()
    {
        SceneManager.LoadScene("ChoixMode");
    }

    public void MenuMode()
    {
        
        SceneManager.LoadScene("SampleScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene") 
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
