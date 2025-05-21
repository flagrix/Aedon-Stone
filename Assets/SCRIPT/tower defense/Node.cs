using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class Node : MonoBehaviour {

    public Color hoverColor;

    public GameObject turret;
    private Color startColor;
    private Renderer rend;

    private BuildManager buildManager;

    public GameObject panelToShow;
    public GameObject Reticule;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    private void OnMouseDown()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Reticule.SetActive(false);
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
