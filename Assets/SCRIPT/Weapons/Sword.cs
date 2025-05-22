using UnityEngine;

public class Sword : Item
{
    [SerializeField] Camera cam;
    private float tempecoule = 3000;
    
    private void Update()
    {
        tempecoule += Time.deltaTime;
    }
    public override void Use()
    {
        if (itemScriptableObject.cooldown < tempecoule)
        {
            Shoot(itemScriptableObject.damage);
            tempecoule = 0;
        }
        else
        {
            Debug.Log("Sword is reloading"+ tempecoule );
        }
    }

    public void Shoot(float damage)
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance <= itemScriptableObject.portee)
            {
                var temp = hit.collider.gameObject.GetComponent<IDamageable>();
                temp?.TakeDamage(itemScriptableObject.damage);
            }
            else
            {
                Debug.Log("Trop loin pour interagir.");
            }
            
            
        }
    }
}
