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

    private void OnMouseDown()
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
        if (turret != null)
        {
            Debug.Log("Impossible de construire ici, il y a déjà une tourelle.");
            return;
        }
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }


}
