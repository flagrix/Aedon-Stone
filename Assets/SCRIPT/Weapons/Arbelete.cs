using UnityEngine;

public class Arbelete : Item
{
  [SerializeField] Camera cam;
  
  private float tempecoule = 3;

  private void Update()
  {
    tempecoule += Time.deltaTime;
  }
  public override void Use()
  {
    if (itemScriptableObject.cooldown < tempecoule)
    {
      Shoot();
      tempecoule = 0;
    }
    else
    {
      Debug.Log("Arbelete reloading"+ tempecoule );
    }
  }

  void Shoot()
  {
    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    ray.origin = cam.transform.position;
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
      var temp = hit.collider.gameObject.GetComponent<IDamageable>();
      temp?.TakeDamage(itemScriptableObject.damage);
    }
  }
}
