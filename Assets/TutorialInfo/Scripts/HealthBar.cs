using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public int actual_health = 100;
    public static HealthBar instance;

    [Header("Damage Flash Settings")]
    public Image damageImage; // Image rouge qui recouvre l'écran
    public float flashDuration = 0.7f; // Durée du flash en secondes
    private float flashTimer;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (actual_health <= 0)
        {
            PlayerMovement.instance.Death();
            PlayerMovement.instance.isRespawning = true;
        }

        if (flashTimer > 0)
        {
            damageImage.gameObject.SetActive(true);
            flashTimer -= Time.deltaTime;
            float alpha = flashTimer / flashDuration;
            damageImage.color = new Color(1f, 0f, 0f, alpha);
        }
        else
        {
            damageImage.gameObject.SetActive(false);
            damageImage.color = new Color(1f, 0f, 0f, 0f); // Caché
        }

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
        // Si le joueur perd de la vie, activer le flash rouge
        if (health < 0 && actual_health > 0)
        {
            TriggerDamageFlash();
        }
        if (actual_health + health <= 0)
            actual_health = 0;
        else if (actual_health + health > 100)
            actual_health = 100;
        else
            actual_health += health;
        SetHealth(actual_health);
        
    }

    private void TriggerDamageFlash()
    {
        
        flashTimer = flashDuration;
        damageImage.color = new Color(1f, 0f, 0f, 1f); // Plein rouge
    }
}
