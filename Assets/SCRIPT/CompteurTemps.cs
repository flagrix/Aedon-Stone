using UnityEngine;
using UnityEngine.UI;

public class CompteurTemps : MonoBehaviour
{

    public static CompteurTemps instance;
    [SerializeField] private float timeBetweenWave = 50f;
    private float countdown = 0f;
    private int waveNumber = 0; // Compteur de vagues

    [SerializeField] private Text waveCountTimer;
    [SerializeField] private Text waveAnnouncement; // Texte pour afficher "VAGUE X"
    [SerializeField] private float announcementDuration = 10f; // Temps d'affichage de l'annonce

    public AudioSource WazeAudioSource;  // Composant AudioSource pour les bruits de pas
    public AudioClip new_wave;

    private float announcementTimer = 0f;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        countdown = timeBetweenWave;
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
        WazeAudioSource.clip = new_wave;
        WazeAudioSource.Play();
        waveNumber++;
        timeBetweenWave += 10f; // Augmente le temps entre les vagues
        countdown = timeBetweenWave;

        waveAnnouncement.text = "Début Vague " + waveNumber + " !!!";
        waveAnnouncement.gameObject.SetActive(true);
        announcementTimer = Time.time + announcementDuration; // Planifier la désactivation
    }
}
