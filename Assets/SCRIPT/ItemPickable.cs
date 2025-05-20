using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPickable : MonoBehaviour, IPickable
{
    public ItemSO itemScriptableObject;

    public void PickItem()
    {
        Destroy(gameObject);
    }
}
