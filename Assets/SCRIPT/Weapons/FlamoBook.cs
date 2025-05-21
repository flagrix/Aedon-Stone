using UnityEngine;

public class FlamoBook : Item
{
    public override void Use()
    {
        Debug.Log("FlamoBook"+ itemScriptableObject.itemType.ToString());
    }
}
