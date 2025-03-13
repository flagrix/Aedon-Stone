using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public static MusicManager instance; // Singleton instance

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Garde le MusicManager entre les scènes
        }
        else
        {
            Destroy(gameObject); // Évite les doublons
        }
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        StartCoroutine(PlayMusicSequence());
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Musique de fond arrêtée.");
        }
    }



    IEnumerator PlayMusicSequence()
    {
        AudioClip music1 = Resources.Load<AudioClip>("Aedon_battle_theme_1");
        AudioClip music2 = Resources.Load<AudioClip>("Aedon_battle_theme_2");

        audioSource.clip = music1;
        audioSource.Play();
        yield return new WaitForSeconds(music1.length);

        audioSource.clip = music2;
        audioSource.Play();
    }
}

