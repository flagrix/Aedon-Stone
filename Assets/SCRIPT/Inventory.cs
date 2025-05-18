using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Inventory : MonoBehaviour
{
    public int runes;
    public int potions;
    public Text runesText; //pour afficher les runes
    public Text potionsText;
    private Photon.Pun.PhotonView photonView;

    public HealthBar healthBar;
    private void Awake()
    {
        photonView = GetComponent<Photon.Pun.PhotonView>();


    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (photonView == null || !photonView.IsMine) return;

    }

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

    public void UsePotion()
    {
        if (potions > 0)
        {
            addPotion(-1);
            GetComponentInParent<PlayerController>().Heal(40);
        }
    }
}

