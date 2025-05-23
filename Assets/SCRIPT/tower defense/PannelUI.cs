using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using System.IO;

public class TurretUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text levelText;
    public TMP_Text damageText;
    public TMP_Text rangeText;
    public TMP_Text upgradeCostText;
    public TMP_Text damagesuivantText;
    public TMP_Text rangesuivantText;
    private defense turretDefense;

    public Button upgradeButton;

    public Button Quitter;

    public GameObject panel_tde;
    public TMP_Text levelText_tde;
    public TMP_Text damageText_tde;
    public TMP_Text rangeText_tde;
    public TMP_Text upgradeCostText_tde;
    public TMP_Text damagesuivantText_tde;
    public TMP_Text rangesuivantText_tde;

    public Button upgradeButton_tde;

    public Button Quitter_tde;

    public GameObject panel_baliste;
    public TMP_Text levelText_baliste;
    public TMP_Text damageText_baliste;
    public TMP_Text rangeText_baliste;
    public TMP_Text upgradeCostText_baliste;
    public TMP_Text damagesuivantText_baliste;
    public TMP_Text rangesuivantText_baliste;

    public Button upgradeButton_baliste;

    public Button Quitter_baliste;

    public Button vendre_baliste;
    public Button vendre_tde;

    public Button vendre;
    public Inventory_global inv;

    void Start()
    {
        inv = FindObjectOfType<Inventory_global>();
        if (turretDefense.isCanon)
        {
            upgradeButton.onClick.AddListener(() =>
            {
                if (turretDefense != null && turretDefense.Upgrade())
                {
                    UpdateUI();
                }
            });

            Quitter.onClick.AddListener(() =>
            {
                if (turretDefense != null)
                {
                    Hide();
                }
            });
            vendre.onClick.AddListener(() =>
            {
                if (turretDefense != null)
                {
                    inv.addRune(350);
                    PhotonNetwork.Destroy(turretDefense.gameObject);
                    Hide();
                }
            });
        }
        else if (turretDefense.useLaser)
        {
            upgradeButton_tde.onClick.AddListener(() =>
            {
                if (turretDefense != null && turretDefense.Upgrade())
                {
                    UpdateUI();
                }
            });

            Quitter_tde.onClick.AddListener(() =>
            {
                if (turretDefense != null)
                {
                    Hide();
                }
            });
            vendre_tde.onClick.AddListener(() =>
            {
                if (turretDefense != null)
                {
                    inv.addRune(700);
                    PhotonNetwork.Destroy(turretDefense.gameObject);
                    Hide();
                }
            });
        }
        else if (turretDefense.isBalise)
        {
            upgradeButton_baliste.onClick.AddListener(() =>
            {
                if (turretDefense != null && turretDefense.Upgrade())
                {
                    UpdateUI();
                }
            });
            Quitter_baliste.onClick.AddListener(() =>
            {
                if (turretDefense != null)
                {
                    Hide();
                }
            });
            vendre_baliste.onClick.AddListener(() =>
            {
                if (turretDefense != null)
                {
                    inv.addRune(500);
                    PhotonNetwork.Destroy(turretDefense.gameObject);
                    Hide();
                }
            });
        }
        panel.SetActive(false);
    }


    public void Show(defense def)
    {
        turretDefense = def;
        UpdateUI();
        if (def.useLaser)
        {
            panel_tde.SetActive(true);
        }
        else if (def.isCanon)
        {
            panel.SetActive(true);
        }
        else if (def.isBalise)
        {
            panel_baliste.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void Hide()
    {
        if (turretDefense.useLaser)
        {
            panel_tde.SetActive(false);
        }
        else if (turretDefense.isCanon)
        {
            panel.SetActive(false);
        }
        else if (turretDefense.isBalise)
        {
            panel_baliste.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateUI()
    {
        if (turretDefense == null) return;
        if (turretDefense.isCanon)
        {
            levelText.text = turretDefense.level.ToString();
            damageText.text = turretDefense.Damage.ToString();
            rangeText.text = turretDefense.range.ToString();
            upgradeCostText.text = turretDefense.upgradeCost.ToString();
            rangesuivantText.text = turretDefense.Rangenext.ToString();
            damagesuivantText.text = turretDefense.Damagenext.ToString();
            Hide();
        }
        else if (turretDefense.useLaser)
        {
            levelText_tde.text = turretDefense.level.ToString();
            damageText_tde.text = turretDefense.damageOverTime.ToString();
            rangeText_tde.text = turretDefense.range.ToString();
            upgradeCostText_tde.text = turretDefense.upgradeCost_tde.ToString();
            rangesuivantText_tde.text = turretDefense.Rangenext.ToString();
            damagesuivantText_tde.text = turretDefense.damageOverTime_tde.ToString();
            Hide();
        }
        else if (turretDefense.isBalise)
        {
            levelText_baliste.text = turretDefense.level.ToString();
            damageText_baliste.text = turretDefense.Damage.ToString();
            rangeText_baliste.text = turretDefense.range.ToString();
            upgradeCostText_baliste.text = turretDefense.upgradeCost.ToString();
            rangesuivantText_baliste.text = turretDefense.Rangenext.ToString();
            damagesuivantText_baliste.text = turretDefense.Damagenext.ToString();
            Hide();
        }
    }
    




}
