using UnityEngine;

public class Hammer : Item
{

    public AudioSource WazeAudioSource;
    public AudioClip hammer_sound;
    private Camera cam;

    void Start()
    {
        //WazeAudioSource.clip = hammer_sound;
        itemScriptableObject.tempecoule = itemScriptableObject.cooldown+1;
    }
    public override void Use()
    { 
        if (itemScriptableObject.cooldown < itemScriptableObject.tempecoule)
        {
            Build();
            itemScriptableObject.tempecoule = 0;
        }
        else
        {
            Debug.Log("Hammer"+ itemScriptableObject.itemType.ToString());
        }
        
    }

    public void NodeOverview()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var temp = hit.collider.gameObject.GetComponent<Node>();
        }
    }

    void Build()
    {
        if (!WazeAudioSource.isPlaying)
        {
            WazeAudioSource.Play();
        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("we hit " + hit.collider.gameObject.name);
            var temp = hit.collider.gameObject.GetComponent<Node>();
            
        }
        
    }
}
