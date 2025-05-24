using UnityEngine;

public class Arbelete : Item
{
  [SerializeField] Camera cam;
  
  

    public AudioSource WazeAudioSource;
    public AudioClip arbalete_sound;
    [SerializeField] private GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 20f;
    public override void Use()
  {
    if (itemScriptableObject.cooldown < itemScriptableObject.tempecoule)
    {
      Shoot();
      itemScriptableObject.tempecoule = 0;
    }
    else
    {
      Debug.Log("Arbelete reloading"+ itemScriptableObject.tempecoule );
    }
  }

  public override void NodeOverview()
  {
    return;
  }

  void Shoot()
  {
        WazeAudioSource.clip = arbalete_sound;
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    ray.origin = cam.transform.position;
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
        float distance = Vector3.Distance(transform.position, hit.transform.position);
        if (distance <= itemScriptableObject.portee)
        {
            Debug.Log("we hit " + hit.collider.gameObject.name);
            var temp = hit.collider.gameObject.GetComponent<IDamageable>();
            temp?.TakeDamage(itemScriptableObject.damage);
            Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 0.5f;
           /* Quaternion lookRotation = Quaternion.LookRotation(hit.point - spawnPosition);
            Quaternion correctedRotation = lookRotation * Quaternion.Euler(90f, 90f, 0);
            GameObject proj = Photon.Pun.PhotonNetwork.Instantiate(
              ("PhotonPrefabs/"+projectilePrefab.name), 
              projectileSpawnPoint.position, 
              correctedRotation
            );
            proj.GetComponent<Projectile>().Initialize(hit.point, projectileSpeed);*/ //projectile cassé
          

        }
    }
  }
}
