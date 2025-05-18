using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    public override void Use()
    {
        Debug.Log("using gun" + itemInfo.itemName);
        Shoot();
    }

    void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("Aucune caméra principale trouvée ! Assigne une caméra au script SingleShotGun.");
            }
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(0.5f, 0.5f) * new Vector2(Screen.width, Screen.height));

       // Ray ray = cam.ScreenPointToRay(new Vector3(0.5f,0.5f));
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("we hit " + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunsInfos)itemInfo).damage);
        }
    }
}
