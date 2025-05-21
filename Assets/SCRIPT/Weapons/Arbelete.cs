using UnityEngine;

public class Arbelete : Item
{
  public override void Use()
  {
    Debug.Log("Arbelete"+ itemScriptableObject.itemType.ToString());
  }
}
