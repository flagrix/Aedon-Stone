using UnityEngine;

public class AudioListenersManager : MonoBehaviour
{
    void Awake()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            // Désactive ce AudioListener car il y en a déjà un
            GetComponent<AudioListener>().enabled = false;
        }
    }
}
