using UnityEngine;

public class AudioListenersManager : MonoBehaviour
{
    void Awake()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            // D�sactive ce AudioListener car il y en a d�j� un
            GetComponent<AudioListener>().enabled = false;
        }
    }
}
