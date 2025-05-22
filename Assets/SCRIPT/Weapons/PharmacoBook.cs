using UnityEngine;

public class PharmacoBook : Item
{
    public AudioSource WazeAudioSource;
    public AudioClip pharmabook_sound;

    void Start()
    {
        WazeAudioSource.clip = pharmabook_sound;
    }
    public override void Use()
    {
        Debug.Log("Pharmacobook"+ itemScriptableObject.itemType.ToString());
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
    }
}
