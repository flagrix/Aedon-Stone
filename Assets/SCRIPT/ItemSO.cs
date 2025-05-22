using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Properties")]
    public Sprite sprite;
    public float cooldown;
    public itemType itemType;
    public float damage;
}

public enum itemType {Hammer, Axe, LongSword, PharmacoBook, Arbalete, FlameBook, Hallebarde}
