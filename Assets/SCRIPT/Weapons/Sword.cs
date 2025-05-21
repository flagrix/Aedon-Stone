using UnityEngine;

public class Sword : Item
{
    public override void Use()
    {
        Debug.Log("Sword"+ itemScriptableObject.itemType.ToString());
    }
}
