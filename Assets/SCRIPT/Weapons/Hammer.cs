using UnityEngine;

public class Hammer : Item
{

    public AudioSource WazeAudioSource;
    public AudioClip hammer_sound;
    public Camera cam;
    public Node temptarget;

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
        
        
    }

    public override void NodeOverview()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var temp = hit.collider.gameObject.GetComponent<Node>();
            if (temp != temptarget)
            {
                temptarget?.OverviewMousseExit();
            }
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance <= itemScriptableObject.portee)
            {
                temp?.overwiewOnMousseEnter();
                temptarget = temp;
            }
            
        }
    }

    void Build()
    {
        WazeAudioSource.clip = hammer_sound;
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
                var temp = hit.collider.gameObject.GetComponent<Node>();
                temp?.OverviewOnMouseDown();
            }
        }
        
    }
}
