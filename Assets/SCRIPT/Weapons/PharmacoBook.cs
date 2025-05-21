using UnityEngine;

public class PharmacoBook : Item
{
    public override void Use()
    {
        Debug.Log("Pharmacobook"+ itemScriptableObject.itemType.ToString());
    }
}
