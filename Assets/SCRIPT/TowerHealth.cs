using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    public static TowerHealth instance;
    public int health = 1000;
    public Slider slider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetHealth(int i)
    {
        health = i;
        if (health <= 0)
        {
            //GameOver.instance.EndGame();
        }
        
    }

    public void SetActualHealth(int i)
    {
        if (health + i <= 0)
            health = 0;
        else if (health + i > 100)
            health = 100;
        else
            health += i;
        SetHealth(health);
    }


}
