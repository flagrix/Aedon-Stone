using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class HealQwertien : MonoBehaviour
{
    public NavMeshAgent agent2;
    public float stopDistance = 8F; // Distance minimale avant d'arr�ter la poursuite

    private Vector3 lastTargetPosition;
    public Slider healthBar;
    public int attackDamage = 8;
    public float attackCooldown = 5f;
    public float HealCooldown = 2f;
    private float lastAttackTime;
    private float lastHealTime;
    public static HealQwertien instance;

    private int Health = 150;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private Renderer rend;
    private Color originalColor;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>(); 
        if (rend != null)
            originalColor = rend.material.color;
    }

    private IEnumerator FlashHealEffect()
    {
        if (rend != null)
        {
            rend.material.color = Color.yellow;
            yield return new WaitForSeconds(0.2f); // dur�e du flash
            rend.material.color = originalColor;
        }
    }

 /*   void Update()
    {
        if (PlayerMovement.instance != null)
        {
            // R�cup�rer la position actuelle du joueur
            Vector3 targetPos = PlayerMovement.instance.GetPlayerPosition();

            // V�rifier la distance entre l'agent et le joueur
            float distanceToPlayer = Vector3.Distance(agent2.transform.position, targetPos);

            // Si la distance est sup�rieure � la distance minimale ou que la cible a chang�, recalculer le chemin
            if (distanceToPlayer > stopDistance)
            {
                if (targetPos != lastTargetPosition)
                {
                    agent2.SetDestination(targetPos);
                    lastTargetPosition = targetPos;
                }
            }
            else
            {
                // Arr�ter le mouvement si l'agent est dans la port�e minimale
                agent2.ResetPath();
                //if (!GameOver.instance.isGameOver)
            }
            if (Time.time - lastHealTime >= HealCooldown)
            {
                lastHealTime = Time.time;
                StartCoroutine(FlashHealEffect());
            }
        }
    }*/

}

