using UnityEngine;

public class AnimatorQwertien : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPos;
    private Transform transform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != lastPos)
        {
            animator.SetBool("mv", true);
        }
        else 
        {
            animator.SetBool("mv", false);
        }
        lastPos = transform.position;
    }
}
