using UnityEngine;
using UnityEngine.AI;

public class QwertiensBasic : MonoBehaviour
{
    public NavMeshAgent agent2;
    public float stopDistance = 8F; // Distance minimale avant d'arr�ter la poursuite

    private Vector3 lastTargetPosition;

    void Update()
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
            }
        }
    }
}
