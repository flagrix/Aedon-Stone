using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public int actual_health = 100;
    public static HealthBar instance;
    private void Awake()
    {
        instance = this;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetActualHealth(int health)
    {
        if (actual_health + health <= 0)
            actual_health = 0;
        else if (actual_health + health > 100)
            actual_health = 100;
        else
            actual_health += health;
        SetHealth(actual_health);
    }
}
