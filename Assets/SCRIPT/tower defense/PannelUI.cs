using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text  levelText;
    public TMP_Text  damageText;
    public TMP_Text  rangeText;
    public TMP_Text  upgradeCostText;
    public TMP_Text  damagesuivantText;
    public TMP_Text  rangesuivantText;
    private defense turretDefense;

    public Button upgradeButton;

    public Button Quitter;

    public GameObject panel_tde;
    public TMP_Text  levelText_tde;
    public TMP_Text  damageText_tde;
    public TMP_Text  rangeText_tde;
    public TMP_Text  upgradeCostText_tde;
    public TMP_Text  damagesuivantText_tde;
    public TMP_Text  rangesuivantText_tde;

    public Button upgradeButton_tde;

    public Button Quitter_tde;

    void Start()
    {
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void Hide()
    {
        panel.SetActive(false);
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
    }



}
