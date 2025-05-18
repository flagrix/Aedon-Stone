using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public int actual_health = 100;

    [Header("Damage Flash Settings")]
    public Image damageImage; // Image rouge qui recouvre l'écran
    public float flashDuration = 0.7f; // Durée du flash en secondes
    private float flashTimer;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Mort si plus de vie
        if (actual_health <= 0 && playerMovement != null)
        {
            playerMovement.Death();
            playerMovement.isRespawning = true;
        }

        // Flash rouge
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

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetActualHealth(int delta)
    {
        if (delta < 0 && actual_health > 0)
        {
            TriggerDamageFlash();
        }

        actual_health = Mathf.Clamp(actual_health + delta, 0, 100);
        SetHealth(actual_health);
    }

  
    private void TriggerDamageFlash()
    {
        flashTimer = flashDuration;
        damageImage.color = new Color(1f, 0f, 0f, 1f);
    }
}