using UnityEngine;

public class Hammer : Item
{

    public AudioSource WazeAudioSource;
    public AudioClip hammer_sound;

    void Start()
    {
        //WazeAudioSource.clip = hammer_sound;
        itemScriptableObject.tempecoule = itemScriptableObject.cooldown+1;
    }
    public override void Use()
    { 
        Debug.Log("Hammer"+ itemScriptableObject.itemType.ToString());
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
    }
}
