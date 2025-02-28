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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3) 
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
