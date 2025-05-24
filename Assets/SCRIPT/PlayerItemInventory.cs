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
    private string precompile;
    [SerializeField] public float xbuy;
    [SerializeField] public float ybuy;
    [SerializeField] public float zbuy;
    
    
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
    [SerializeField]public Slider slot1;
    [SerializeField]public Slider slot2;
    [SerializeField]public Slider slot3;
    [SerializeField]public Slider slot4;
    public List<Slider> SliderList;
    [SerializeField] public GameObject Buy;
    
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
        stringprefab.Add(itemType.FlameBook, "PhotonPrefabs/flamobook");
        stringprefab.Add(itemType.Hallebarde, "PhotonPrefabs/hallebarde (1)");
        stringprefab.Add(itemType.Axe, "PhotonPrefabs/MetallAx2 (1)");
        stringprefab.Add(itemType.PharmacoBook, "PhotonPrefabs/PharmacoBook (1)");
        stringprefab.Add(itemType.LongSword, "PhotonPrefabs/LongSword (1)");
        stringprefab.Add(itemType.Hammer, "PhotonPrefabs/uploads_files_3650488_Blacksmith's+Rounding+Hammerno+sharps");
        //
        SliderList = new List<Slider>(){ slot1, slot2, slot3, slot4 };
        
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
        InteractItem();
        
        //Item Throw
        precompile = stringprefab[inventoryList[selectedItem]];
        ThrowItem(precompile);
       
        //UI
        AdaptUI();
        // overwiew du hammer
        if (inventoryList[selectedItem] == itemType.Hammer)
        {
            itemSetActive[inventoryList[selectedItem]].NodeOverview();
        }
        

        //Item selection
        SelectItem();
        
        //Weapons Use

        if (Input.GetMouseButtonDown(0))
        {
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

    public void ThrowItem(string instanceObject )
    {
        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 1 && inventoryList[selectedItem] != itemType.Hammer)
        {
            //Instantiate(itemInstanciate[inventoryList[selectedItem]], position: throwItem_GameObject.transform.position, new Quaternion());
            PhotonNetwork.Instantiate(instanceObject, position: throwItem_GameObject.transform.position,Quaternion.Euler(
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
    }

    public void InteractItem()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, playerReach))
        {
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            Ouvrier ouvrier = hitInfo.collider.GetComponent<Ouvrier>();
            //menu pick up weapon
            if (item != null && inventoryList.Count < 4)
            {
                pickUpItem_gameobject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    ItemPickable itemPickable = hitInfo.collider.GetComponent<ItemPickable>();
                    if (itemPickable != null)
                    {
                        itemType pickedItemType = itemPickable.itemScriptableObject.itemType;
                        
                        inventoryList.Add(pickedItemType);
                        
                        item.PickItem();
                        PhotonView itemPhotonView = hitInfo.collider.GetComponent<PhotonView>();
                        if (itemPhotonView != null)
                        {
                            PhotonNetwork.Destroy(itemPhotonView.gameObject);
                        }
                        else
                        {
                            Debug.LogWarning("L'item n'a pas de PhotonView, destruction locale seulement");
                            Destroy(hitInfo.collider.gameObject);
                        }
                        
                        NewItemSelected();
                    }
                }
            }
            else
            {
                pickUpItem_gameobject.SetActive(false);
            }
            //menu ouvrier
            if (ouvrier != null)
            {
                pickUpItem_gameobject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    ShowPanel();
                }
            }
            
        }
    }
    public void ShowPanel()
    {
        if (!photonView.IsMine) return;

        Buy.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePanel()
    {
        Buy.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AdaptUI()
    {
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
                VARIABLE.enabled = true;
            }
            else
            {
                //VARIABLE.color = Color.clear;
                VARIABLE.enabled = false;
            }

            a++;
        }
        
        for (int i = 0; i < inventoryList.Count; i++)
        {
            itemSetActive[inventoryList[i]].itemScriptableObject.tempecoule += Time.deltaTime;
            if (SliderList[i] != null)
            {
                var actualcooldown = itemSetActive[inventoryList[i]].itemScriptableObject.tempecoule;
                var totalcooldown = itemSetActive[inventoryList[i]].itemScriptableObject.cooldown;
                if (actualcooldown / totalcooldown * 100 > 100)
                {
                    SliderList[i].value = 0;
                }
                else
                {
                    SliderList[i].value = actualcooldown / totalcooldown * 100;
                }
            }
        }
    }

    public void SelectItem()
    {
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
    }
    public void BuyWeapon(string Weapon)
    { 
        PhotonNetwork.Instantiate(Weapon,new Vector3(xbuy,ybuy, zbuy),Quaternion.Euler(0f, 0f, 0f));
        HidePanel();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
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
