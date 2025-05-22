using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class PlayerItemInventory : MonoBehaviourPunCallbacks
{
    [Header("General")]
    public List<itemType> inventoryList;
    public int selectedItem = 0;
    public float playerReach;
    [SerializeField] GameObject throwItem_GameObject;
    [SerializeField] PhotonView PV;
    
    
    [Space(20)]
    [Header("Keys")]
    [SerializeField]KeyCode throwItemKey;

    [SerializeField] private KeyCode pickItemKey;
    
    [Space(20)]
    [Header("Item gameobjects")]
    [SerializeField] Item Hammer_item;
    [SerializeField] Item Axe_item;
    [SerializeField] Item LongSword_item;
    [SerializeField] Item PharmacoBook_item;
    [SerializeField] Item Arbalete_item;
    [SerializeField] Item FlameBook_item;
    [SerializeField] Item Hallebarde_item;
    
    [Space(20)]
    [Header("Item Prefabs")]
    [SerializeField] GameObject HammerItemPrefab;
    [SerializeField] GameObject AxeItemPrefab;
    [SerializeField] GameObject LongSwordItemPrefab;
    [SerializeField] GameObject PharmacoBookItemPrefab;
    [SerializeField] GameObject ArbaleteItemPrefab;
    [SerializeField] GameObject FlameBookItemPrefab;
    [SerializeField] GameObject HallebardeItemPrefab;   
    
    [Space(20)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[4];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[4];
    [SerializeField] Sprite emptySlotSprite;
    
    [SerializeField] Camera cam;
    [SerializeField] GameObject pickUpItem_gameobject;
    
    
    private Dictionary<itemType, Item> itemSetActive = new Dictionary<itemType, Item>() { } ;
    private Dictionary<itemType, GameObject> itemInstanciate = new Dictionary<itemType, GameObject>() { } ;
    private Dictionary<itemType, string> stringprefab = new Dictionary<itemType, string>() { } ;

    void Awake()
    {
       
    }
    void Start()
    {
        PV = GetComponent<PhotonView>();
        itemSetActive.Add(itemType.Hammer, Hammer_item);
        itemSetActive.Add(itemType.Axe, Axe_item);
        itemSetActive.Add(itemType.LongSword, LongSword_item);
        itemSetActive.Add(itemType.PharmacoBook, PharmacoBook_item);
        itemSetActive.Add(itemType.Arbalete, Arbalete_item);
        itemSetActive.Add(itemType.FlameBook, FlameBook_item);
        itemSetActive.Add(itemType.Hallebarde, Hallebarde_item);
        //
        itemInstanciate.Add(itemType.Hammer, HammerItemPrefab);
        itemInstanciate.Add(itemType.Axe, AxeItemPrefab);
        itemInstanciate.Add(itemType.LongSword, LongSwordItemPrefab);
        itemInstanciate.Add(itemType.PharmacoBook, PharmacoBookItemPrefab);
        itemInstanciate.Add(itemType.Arbalete, ArbaleteItemPrefab);
        itemInstanciate.Add(itemType.FlameBook, FlameBookItemPrefab);
        itemInstanciate.Add(itemType.Hallebarde, HallebardeItemPrefab);
        //
        stringprefab.Add(itemType.Arbalete, "PhotonPrefabs/arbalete");
        stringprefab.Add(itemType.FlameBook, "PhotonPrefabs/PharmacoBook (1)");
        stringprefab.Add(itemType.Hallebarde, "PhotonPrefabs/hallebarde (1)");
        stringprefab.Add(itemType.Axe, "PhotonPrefabs/MetallAx2 (1)");
        stringprefab.Add(itemType.PharmacoBook, "PhotonPrefabs/PharmacoBook (1)");
        stringprefab.Add(itemType.LongSword, "PhotonPrefabs/LongSword (1)");
        
        if (PV.IsMine)
        {
            selectedItem = 0;
            NewItemSelected();
        }

        selectedItem = 0;
    }
    
    void Update()
    {
        if(!PV.IsMine) return;
    
        //Item Pickup
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, playerReach) && inventoryList.Count < 4)
        {
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if (item != null)
            {
                pickUpItem_gameobject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    // Récupérer les informations de l'item avant de le détruire
                    ItemPickable itemPickable = hitInfo.collider.GetComponent<ItemPickable>();
                    if (itemPickable != null)
                    {
                        itemType pickedItemType = itemPickable.itemScriptableObject.itemType;
                    
                        // Ajouter à l'inventaire
                        inventoryList.Add(pickedItemType);
                    
                        // Appeler la méthode PickItem
                        item.PickItem();
                    
                        // Détruire l'objet via Photon (synchronisé sur tous les clients)
                        PhotonView itemPhotonView = hitInfo.collider.GetComponent<PhotonView>();
                        if (itemPhotonView != null)
                        {
                            PhotonNetwork.Destroy(itemPhotonView.gameObject);
                        }
                        else
                        {
                            // Si l'objet n'a pas de PhotonView, destruction locale uniquement
                            Debug.LogWarning("L'item n'a pas de PhotonView, destruction locale seulement");
                            Destroy(hitInfo.collider.gameObject);
                        }
                    
                        // Mettre à jour l'interface
                        NewItemSelected();
                    }
                }
            }
            else
            {
                pickUpItem_gameobject.SetActive(false);
            }
        }
        else
        {
            pickUpItem_gameobject.SetActive(false);
        }
        //Item Throw

        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 1 && inventoryList[selectedItem] != itemType.Hammer)
        {
            //Instantiate(itemInstanciate[inventoryList[selectedItem]], position: throwItem_GameObject.transform.position, new Quaternion());
            PhotonNetwork.Instantiate(stringprefab[inventoryList[selectedItem]], position: throwItem_GameObject.transform.position,Quaternion.Euler(
                Random.Range(75f, 105f), 
                Random.Range(0f, 360f), 
                Random.Range(-15f, 15f)
            ));
            inventoryList.RemoveAt(selectedItem);
            if (selectedItem != 0)
            {
                selectedItem--;
            }
            NewItemSelected();
        }
        //UI
        for (int i = 0; i < 4; i++)
        {
            if (i < inventoryList.Count)
            {
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemScriptableObject.sprite;
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }

        int a = 0;
        foreach (var VARIABLE in inventoryBackgroundImage)
        {
            if (a == selectedItem)
            {
                VARIABLE.color = Color.red;
            }
            else
            {
                VARIABLE.color = Color.clear;
            }

            a++;
        }
        
        //Item selection
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Count > 3)
        {
            selectedItem = 3;
            NewItemSelected();
        }
        
        //Weapons Use

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Down");
             itemSetActive[inventoryList[selectedItem]].Use();
        }
        
    }
    private void NewItemSelected()
    {
        //debugage
        if (inventoryList == null || inventoryList.Count == 0)
        {
            Debug.LogWarning("inventoryList est null ou vide");
            return;
        }
        if (selectedItem < 0 || selectedItem >= inventoryList.Count)
        {
            Debug.LogWarning($"selectedItem ({selectedItem}) est hors des limites de inventoryList (count: {inventoryList.Count})");
            selectedItem = 0;
        }
        //
        Hammer_item.ItemGameobjects.SetActive(false);
        Axe_item.ItemGameobjects.SetActive(false);
        LongSword_item.ItemGameobjects.SetActive(false);
        PharmacoBook_item.ItemGameobjects.SetActive(false);
        Arbalete_item.ItemGameobjects.SetActive(false);
        FlameBook_item.ItemGameobjects.SetActive(false);
        Hallebarde_item.ItemGameobjects.SetActive(false);
        //GameObject selectedObject = itemSetActive[inventoryList[selectedItem]].ItemGameobjects;
        itemType currentItemType = inventoryList[selectedItem];
        if (!itemSetActive.ContainsKey(currentItemType))
        {
            Debug.LogError($"itemType {currentItemType} n'existe pas dans itemSetActive");
            return;
        }
        GameObject selectedObject = itemSetActive[currentItemType].ItemGameobjects;
        if (selectedObject != null)
        {
            selectedObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"GameObject pour {currentItemType} est null");
        }
        //
        selectedObject.SetActive(true);

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("selectedItem",selectedItem );
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        /* if (changedProps.ContainsKey("selectedItem") && targetPlayer == PV.Owner)
         {
            selectedItem= (int)changedProps["selectedItem"];
            if(!PV.IsMine)
                NewItemSelected();
         } */
        if (changedProps == null || targetPlayer == null || PV == null)
        {
            Debug.LogWarning("Paramètres null dans OnPlayerPropertiesUpdate");
            return;
        }

        if (changedProps.ContainsKey("selectedItem") && targetPlayer == PV.Owner)
        {
            object selectedItemObj = changedProps["selectedItem"];
            if (selectedItemObj != null)
            {
                int newSelectedItem = (int)selectedItemObj;

                // Vérifier que la valeur est valide
                if (inventoryList != null && newSelectedItem >= 0 && newSelectedItem < inventoryList.Count)
                {
                    selectedItem = newSelectedItem;
                    if (!PV.IsMine)
                    {
                        NewItemSelected();
                    }
                }
                else
                {
                    Debug.LogWarning($"selectedItem invalide reçu: {newSelectedItem}");
                }
            }
        }
    }

    [PunRPC]
    void RPC_PickupItem(itemType pickedItemType, int itemViewID)
    {
        // Trouver l'objet par son ViewID
        PhotonView itemView = PhotonNetwork.GetPhotonView(itemViewID);
        if (itemView != null)
        {
            // Seulement le joueur qui a ramassé l'item l'ajoute à son inventaire
            if (PV.IsMine)
            {
                inventoryList.Add(pickedItemType);
                NewItemSelected();
            }

            // Détruire l'objet pour tous les joueurs
            if (itemView.IsMine || PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(itemView.gameObject);
            }
        }
    }
}

public interface IPickable
{
    void PickItem();
}
