using UnityEngine;
using UnityEngine.UIElements;

public class FlamoBook : Item
{
    public AudioSource WazeAudioSource;
    public AudioClip flamobook_sound;
    [SerializeField] Camera cam;
    public float radiusExplosion;
    public GameObject explosionParticlesPrefab;

    void Start()
    {
        
    }
    public override void Use()
    {
        WazeAudioSource.clip = flamobook_sound;
        if (itemScriptableObject.cooldown < itemScriptableObject.tempecoule)
        {
            Debug.Log("FlamoBook" + itemScriptableObject.itemType.ToString());
            if (!WazeAudioSource.isPlaying)
            {
                WazeAudioSource.Play();
            }

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin = cam.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Explode(hit.point);
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance <= itemScriptableObject.portee)
                {
                    Debug.Log("we hit " + hit.collider.gameObject.name);
                    var temp = hit.collider.gameObject.GetComponent<IDamageable>();
                    temp?.TakeDamage(itemScriptableObject.damage);
                }
            }
            itemScriptableObject.tempecoule = 0;
        }
    }
    public override void NodeOverview()
    {
        return;
    }
    void Explode(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radiusExplosion);
        foreach (Collider collider in colliders)
        {
                var temp =collider.gameObject.GetComponent<IDamageable>(); 
                temp?.TakeDamage(itemScriptableObject.damage);
        }
        Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);

        GameObject particlesGO = Instantiate(explosionParticlesPrefab, pos, flatRotation);
        var particleSystem = particlesGO.GetComponent<ParticleSystem>();
        var shape = particleSystem.shape;
        shape.radius = radiusExplosion;

        particleSystem.Play();
        Destroy(particlesGO, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
    }
}
