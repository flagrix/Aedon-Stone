using UnityEngine;

public class Arbelete : Item
{
  [SerializeField] Camera cam;
  public override void Use()
  {
    Shoot();
  }

  void Shoot()
  {
    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    ray.origin = cam.transform.position;
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
      Debug.Log("we hit " + hit.collider.gameObject.name);
      hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(itemScriptableObject.damage);
    }
  }
}
