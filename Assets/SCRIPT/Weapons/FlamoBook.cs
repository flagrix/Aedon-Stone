using UnityEngine;

public class FlamoBook : Item
{
    public AudioSource WazeAudioSource;
    public AudioClip flamobook_sound;

    void Start()
    {
        WazeAudioSource.clip = flamobook_sound;
    }
    public override void Use()
    {
        Debug.Log("FlamoBook"+ itemScriptableObject.itemType.ToString());
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
    }
}
