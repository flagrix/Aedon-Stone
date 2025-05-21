using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class Inventory : MonoBehaviourPun
{
    [HideInInspector]
    public Node selectedNode;
    public int runes;
    public int potions;
    public Text runesText; //pour afficher les runes
    public Text potionsText;

    public HealthBar healthBar;
    public AudioSource WazeAudioSource;
    public AudioClip potion;

    public GameObject panelToShow;

    public GameObject turretPrefab;

    public void ShowPanel()
    {
        if (!photonView.IsMine) return;

        panelToShow.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Awake()
    {

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
            WazeAudioSource.clip = potion;
            WazeAudioSource.Play();
        }
    }
    
     public void BuildTurret()
{
    if (selectedNode == null) return;

    if (selectedNode.turret != null)
    {
        Debug.Log("Une tourelle existe déjà ici.");
        return;
    }
    GameObject turret = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "canon_vrai 1"), selectedNode.transform.position + selectedNode.positionOffset, transform.rotation, 0);
    
    selectedNode.turret = turret;

    Debug.Log("Tourelle construite sur " + selectedNode.name);
    
    panelToShow.SetActive(false); // Ferme le panel si tu veux
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}


}

