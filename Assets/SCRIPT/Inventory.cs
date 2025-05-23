using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using TMPro;
public class Inventory : MonoBehaviourPun
{
    [HideInInspector]
    public Node selectedNode;
    public int runes;
    public int potions;
    public Text runesText; //pour afficher les runes
    public Text potionsText;
    public Inventory_global inv;
    public AudioSource WazeAudioSource;
    public AudioClip potion;

    public GameObject panelToShow;

    public GameObject turretPrefab;

    public GameObject tde;

    public GameObject balliste;

    public GameObject playerHUD;

    public GameObject playerHUD2;

    private int cout_canon = 350;
    private int cout_tde = 700;

    private int cout_baliste = 500;

    public TurretUI defenseUI;


    public void ShowPanel()
    {
        if (!photonView.IsMine) return;

        panelToShow.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

      public void ShowUpgradePanel(defense defense)
    {
        defenseUI.Show(defense);
    }


    public void ShowPanelAmelioration()
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
        inv = FindObjectOfType<Inventory_global>();
        if (!photonView.IsMine && playerHUD != null)
        {
            playerHUD.SetActive(false);
        }
        if (!photonView.IsMine && playerHUD2 != null)
        {
            playerHUD2.SetActive(false);
        }
    }
    private void Update()
    {
        if (photonView == null || !photonView.IsMine) return;

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
        if (cout_canon <= Inventory_global.runes)
        {
            object[] instantiationData = new object[] { selectedNode.name };

            GameObject turret = PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "canon_vrai 1"),
                selectedNode.transform.position + selectedNode.positionOffset,
                Quaternion.identity,
                0,
                instantiationData
            );
            selectedNode.turret = turret;
            Debug.Log("Tourelle construite sur " + selectedNode.name);
            inv.addRune(-cout_canon);
            defense def = turret.GetComponent<defense>();
        }

        panelToShow.SetActive(false); // Ferme le panel si tu veux
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void BuildTde()
    {
        if (selectedNode == null) return;

        if (selectedNode.turret != null)
        {
            Debug.Log("Une tourelle existe déjà ici.");
            return;
        }
        if (cout_tde <= Inventory_global.runes)
        {
            object[] instantiationData = new object[] { selectedNode.name };
            GameObject turret = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "tour_de_l_enfer"), selectedNode.transform.position, transform.rotation, 0, instantiationData);
            selectedNode.turret = turret;
            Debug.Log("Tourelle construite sur " + selectedNode.name);
            inv.addRune(-cout_tde);
        }

        panelToShow.SetActive(false); // Ferme le panel si tu veux
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Buildballiste()
    {
        if (selectedNode == null) return;

        if (selectedNode.turret != null)
        {
            Debug.Log("Une tourelle existe déjà ici.");
            return;
        }
        if (cout_baliste <= Inventory_global.runes)
        {
            object[] instantiationData = new object[] { selectedNode.name };
            GameObject turret = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Baliste"), selectedNode.transform.position + selectedNode.positionOffset, transform.rotation, 0, instantiationData);
            selectedNode.turret = turret;
            Debug.Log("Tourelle construite sur " + selectedNode.name);
            inv.addRune(-cout_baliste);
        }

        panelToShow.SetActive(false); // Ferme le panel si tu veux
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
