using UnityEngine;

public class Hammer : Item
{
    public override void Use()
    {
        Debug.Log("Hammer"+ itemScriptableObject.itemType.ToString());
    }
}
