using System;
using Unity.VisualScripting;
using UnityEngine;

public class PharmacoBook : Item
{
    [SerializeField] PlayerController player;
    
    public ParticleSystem particleSystem;
    public AudioSource WazeAudioSource;
    public AudioClip pharmabook_sound;

    void Start()
    {
        particleSystem.Stop();
    }
    public override void Use()
    {
        WazeAudioSource.clip = pharmabook_sound;
        if (itemScriptableObject.cooldown < itemScriptableObject.tempecoule)
        {
            player.SetActualHealth(Convert.ToInt32(itemScriptableObject.damage)); //heal le player
            particleSystem.Play();
            Debug.Log("particuleeeeee");
            itemScriptableObject.tempecoule = 0;
        }
        else
        {
            Debug.Log("Pharmaco Book is reloading"+ itemScriptableObject.tempecoule );
        }
        Debug.Log("Pharmacobook"+ itemScriptableObject.itemType.ToString());
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
    }
    public override void NodeOverview()
    {
        return;
    }
}
