using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float damage = 50f;
    private Transform target;

    public float explosionRadius = 0f;
    public float speed = 70f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        if (explosionRadius > 0)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }


    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }


    void Damage(Transform enemy)
    {
        ennemy d = enemy.GetComponent<ennemy>();
        if (d != null)
        {
            d.TakeDammage((int)damage);
        }
        else
        {
            Debug.LogError("Pas de script Enemy sur l'ennemi.");
        }
         

    }
}
    
