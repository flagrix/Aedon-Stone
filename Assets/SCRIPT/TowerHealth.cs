using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public static TowerHealth instance;
    public int health = 100;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetHealth(int i)
    {
        health += i;
        if (health <= 0)
        {
            GameOver.instance.EndGame();
        }
    }
}
