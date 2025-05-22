using UnityEngine;

public class Arbelete : Item
{
  [SerializeField] Camera cam;

    public AudioSource WazeAudioSource;
    public AudioClip arbalete_sound;
    public override void Use()
  {
    Shoot();
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
      Debug.Log("we hit " + hit.collider.gameObject.name);
      var temp = hit.collider.gameObject.GetComponent<IDamageable>();
      temp?.TakeDamage(itemScriptableObject.damage);
    }
  }
}
