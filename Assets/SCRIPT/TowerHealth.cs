using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TowerHealth : MonoBehaviourPunCallbacks
{
    public static TowerHealth instance;
    public int health = 2500;
    public Slider slider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        slider.maxValue = 2500;
        slider.value = health;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
                //photonView.RPC("SetActualHealthRPC", RpcTarget.AllBuffered, -200);


        }
    }

    [PunRPC]
    public void SetActualHealthRPC(int i)
    {
        if (health + i <= 0)
            health = 0;
        else if (health + i > 2500)
            health = 2500;
        else
            health += i;
        Debug.Log(health);
        SetHealth(health);
    }

    public void SetHealth(int i)
    {
        health = i;
        slider.value = i;
        if (health <= 0)
        {
            GameOver localGameOver = FindObjectOfType<GameOver>();
            if (localGameOver != null && !localGameOver.isGameOver)
            {
                localGameOver.EndGame();
            }

        }

    }
}