using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Properties")]
    public Sprite sprite;
    public float cooldown;
    public itemType itemType;
    public float damage;
    public float portee;
    public float tempecoule = 9999;
}

public enum itemType {Hammer, Axe, LongSword, PharmacoBook, Arbalete, FlameBook, Hallebarde}
