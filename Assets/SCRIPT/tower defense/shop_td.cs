using UnityEngine;

public class shop_td
{

    private BuildManager buildManager;

    public void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void PurchaseStandardTurret()
    {
        Debug.Log("canon posé");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }
}
