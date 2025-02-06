using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public int runes;
    public int potions;
    public Text runesText; //pour afficher les runes
    public Text potionsText;
    public static Inventory instance;

    private void Awake()
    {
        instance = this; //Pour pouvoir faire appel a inventory de partout
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            addRune(1);
            addPotion(1);
            HealthBar.instance.SetActualHealth(20);
        }
        if (Input.GetMouseButtonDown(1))
        {
            addRune(-1);
            addPotion(-1);
            HealthBar.instance.SetActualHealth(-20);
        }}

    public void addRune(int newRune)
    {
        if (runes + newRune < 0)
            runes = 0;
        else
        {
            runes += newRune;
            runesText.text = runes.ToString();
        }


    }
    public void addPotion(int newpotion)
    {
        if (potions + newpotion < 0)
            potions = 0;
        else
        {
            potions += newpotion;
            potionsText.text = potions.ToString();
        }


    }
}

