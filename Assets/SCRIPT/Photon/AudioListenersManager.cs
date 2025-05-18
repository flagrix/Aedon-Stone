using UnityEngine;

public class AudioListenersManager : MonoBehaviour
{
    void Awake()
{
    AudioListener[] listeners = FindObjectsOfType<AudioListener>();
    if (listeners.Length > 1)
    {
        AudioListener listener = GetComponent<AudioListener>();
        if (listener != null)
        {
            listener.enabled = false;
        }
        else
        {
            Debug.LogWarning($"Pas d'AudioListener attaché à {gameObject.name}, impossible de le désactiver.");
        }
    }
}

}
