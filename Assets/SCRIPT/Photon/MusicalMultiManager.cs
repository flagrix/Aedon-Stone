using UnityEngine;
using UnityEngine;
using System.Collections;
using Photon.Pun;

public class MusicalMultiManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;
    public static PhotonView instance; // Singleton instance

    void Awake()
    {
        instance.GetComponent<AudioSource>().enabled = true;
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (instance.IsMine)
        {
            



            StartCoroutine(PlayMusicSequence());
        }
    }

    public void StopMusic()
    {
        if (instance.IsMine)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("Musique de fond arrêtée.");
            }
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
