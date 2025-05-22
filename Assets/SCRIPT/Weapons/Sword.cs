using UnityEngine;

public class Sword : Item
{
    public AudioSource WazeAudioSource;
    public AudioClip sword_sound;

    void Start()
    {
        WazeAudioSource.clip = sword_sound;
    }
    public override void Use()
    {
        Debug.Log("Sword"+ itemScriptableObject.itemType.ToString());
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
    }
}
