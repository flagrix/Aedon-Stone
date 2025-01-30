using UnityEngine;
using UnityEngine.UI;

public class CompteurTemps : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenWave = 50f;
    private float countdown = 0f;
    [SerializeField]
    private Text waveCountTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0f)
        {
            countdown = timeBetweenWave;
        }
        countdown -= Time.deltaTime;
        waveCountTimer.text = Mathf.Floor(countdown).ToString();
    }
}
