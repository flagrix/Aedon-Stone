using UnityEngine;
using UnityEngine.UI;

public class CompteurRespawn : MonoBehaviour
{
    [SerializeField] private float timeBetweenRespawn = 10f;
    private float countdown = 0f;
    private int waveNumber = 0; // Compteur de vagues

    [SerializeField] private Text waveCountTimer;
    [SerializeField] private Text waveAnnouncement; 
    [SerializeField] private float announcementDuration = 10f; // Temps d'affichage de l'annonce


    private float announcementTimer = 0f;

    void Start()
    {
        //countdown = timeBetweenWave;
        waveAnnouncement.gameObject.SetActive(false); // Masquer l'annonce au départ
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            StartNewWave();
        }

        countdown -= Time.deltaTime;
        waveCountTimer.text = Mathf.Floor(countdown).ToString();

        // Masquer l'annonce après un certain temps
        if (waveAnnouncement.gameObject.activeSelf && Time.time >= announcementTimer)
        {
            waveAnnouncement.gameObject.SetActive(false);
        }
    }

    void StartNewWave()
    {
        //WazeAudioSource.clip = new_wave;
        //WazeAudioSource.Play();
        waveNumber++;
        //timeBetweenWave += 10f; // Augmente le temps entre les vagues
        //countdown = timeBetweenWave;

        waveAnnouncement.text = "Début Vague " + waveNumber + " !!!";
        waveAnnouncement.gameObject.SetActive(true);
        announcementTimer = Time.time + announcementDuration; // Planifier la désactivation
    }
}
