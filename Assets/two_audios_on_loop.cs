using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
{
    if (audioSource == null)
    {
        audioSource = GetComponent<AudioSource>();
    }

    StartCoroutine(PlayMusicSequence());
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

