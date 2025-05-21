using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemSO itemScriptableObject;
    public GameObject ItemGameobjects;

    public abstract void Use();

}
