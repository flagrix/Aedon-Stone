using UnityEngine;

public class Sword : Item
{
    [SerializeField] Camera cam;
    
    public AudioSource WazeAudioSource;
    public AudioClip sword_sound;

    void Start()
    {
        WazeAudioSource.clip = sword_sound;
    }
    public override void Use()
    {
        if (itemScriptableObject.cooldown < itemScriptableObject.tempecoule)
        {
            Shoot(itemScriptableObject.damage);
            itemScriptableObject.tempecoule = 0;
        }
        else
        {
            Debug.Log("Sword is reloading"+ itemScriptableObject.tempecoule );
        }
    }
    public override void NodeOverview()
    {
        return;
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
        Debug.Log("Sword"+ itemScriptableObject.itemType.ToString());
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
    }
}
