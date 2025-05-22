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

    public HealthBar healthBar;
    public AudioSource WazeAudioSource;
    public AudioClip potion;

    public GameObject panelToShow;

    public GameObject panel_amélioration_canon;

    public GameObject turretPrefab;

    public GameObject tde;

    public GameObject balliste;

    public GameObject playerHUD;

    public GameObject playerHUD2;

    private int cout_canon = 350;
    private int cout_tde = 700;

    private int cout_baliste = 500;
    private int cout_amélioration_canon = 200;
    public int level_canon = 0;

    public int damage_augmentation_canon=20;

    public TMP_Text Level_canon;

    public TMP_Text degat_actuelle_canon;

    public TMP_Text degat_suivant_canon;

    public TMP_Text range_actuelle_canon;

    public TMP_Text range_suivante_canon;

    public TMP_Text cout_amelioration_canon_text;

    public void ShowPanel()
    {
        if (!photonView.IsMine) return;

        panelToShow.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowPanelAmeliorationcanon()
    {
        if (!photonView.IsMine) return;

        panel_amélioration_canon.SetActive(true);
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
        Level_canon.text = level_canon.ToString();
        degat_actuelle_canon.text = "50";
        degat_suivant_canon.text = (50 + (damage_augmentation_canon * (level_canon + 1))).ToString();
        range_actuelle_canon.text = "30";
        range_suivante_canon.text = "30";
        cout_amelioration_canon_text.text = cout_amélioration_canon.ToString();
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
            GameObject turret = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "canon_vrai 1"), selectedNode.transform.position + selectedNode.positionOffset, transform.rotation, 0);
            selectedNode.turret = turret;
            Debug.Log("Tourelle construite sur " + selectedNode.name);
            inv.addRune(-cout_canon);
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
            GameObject turret = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "tour_de_l_enfer"), selectedNode.transform.position, transform.rotation, 0);
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
            GameObject turret = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Baliste"), selectedNode.transform.position + selectedNode.positionOffset, transform.rotation, 0);
            selectedNode.turret = turret;
            Debug.Log("Tourelle construite sur " + selectedNode.name);
            inv.addRune(-cout_baliste);
        }

        panelToShow.SetActive(false); // Ferme le panel si tu veux
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Ameliorer_canon()
    {
        if (cout_amélioration_canon <= Inventory_global.runes)
        {
            level_canon += 1;
            selectedNode.turret.GetComponent<defense>().add_Damage(damage_augmentation_canon * level_canon);
            degat_actuelle_canon.text = (selectedNode.turret.GetComponent<defense>().Damage).ToString();
            degat_suivant_canon.text = (selectedNode.turret.GetComponent<defense>().Damage + (damage_augmentation_canon * (level_canon+1))).ToString();
            if (level_canon == 3 || level_canon == 5)
            {
                selectedNode.turret.GetComponent<defense>().add_range(5);
                range_actuelle_canon.text = (selectedNode.turret.GetComponent<defense>().range).ToString();
                if (level_canon == 2 || level_canon == 4)
                {
                    range_suivante_canon.text = (selectedNode.turret.GetComponent<defense>().range + 5).ToString();
                }
            }
            if (level_canon == 2 || level_canon == 4)
            {
                range_suivante_canon.text = (selectedNode.turret.GetComponent<defense>().range + 5).ToString();
            }
            inv.addRune(-cout_amélioration_canon);
            cout_amélioration_canon = cout_amélioration_canon + 100 * level_canon;
            Level_canon.text = level_canon.ToString();
            cout_amelioration_canon_text.text = cout_amélioration_canon.ToString();
        }
        panel_amélioration_canon.SetActive(false); // Ferme le panel si tu veux
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}

