using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class Node : MonoBehaviour
{
    public Vector3 positionOffset;

    public Color hoverColor;

    public GameObject turret;
    private Color startColor;
    private Renderer rend;

    private BuildManager buildManager;
    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

   /* private void OnMouseDown()
    {
        if (turret != null)
        {
            defense def = turret.GetComponent<defense>();
            foreach (var inventory in FindObjectsOfType<Inventory>())
            {
                if (inventory.photonView.IsMine)
                {
                    inventory.ShowUpgradePanel(def);
                    break;
                }
            }
        }
        else
        {
            foreach (var player in FindObjectsOfType<Inventory>())
            {
                if (player.photonView.IsMine)
                {
                    player.selectedNode = this;
                    player.ShowPanel(); // Affiche son propre panel
                    break;
                }
            }
        }
        
    }*/

    public void OverviewOnMouseDown()
    {
        if (turret != null)
        {
            defense def = turret.GetComponent<defense>();
            foreach (var inventory in FindObjectsOfType<Inventory>())
            {
                if (inventory.photonView.IsMine)
                {
                    inventory.ShowUpgradePanel(def);
                    break;
                }
            }
        }
        else
        {
            foreach (var player in FindObjectsOfType<Inventory>())
            {
                if (player.photonView.IsMine)
                {
                    player.selectedNode = this;
                    player.ShowPanel(); // Affiche son propre panel
                    break;
                }
            }
        }
    }

  /*  public void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }*/

    public void overwiewOnMousseEnter()
    {
        rend.material.color = hoverColor;
    }

  /* public void OnMouseExit()
    {
        rend.material.color = startColor;
    }*/

    public void OverviewMousseExit()
    {
        rend.material.color = startColor;
    }


}
