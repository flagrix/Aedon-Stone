using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
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
        
        itemInstanciate.Add(itemType.Hammer, HammerItemPrefab);
        itemInstanciate.Add(itemType.Axe, AxeItemPrefab);
        itemInstanciate.Add(itemType.LongSword, LongSwordItemPrefab);
        itemInstanciate.Add(itemType.PharmacoBook, PharmacoBookItemPrefab);
        itemInstanciate.Add(itemType.Arbalete, ArbaleteItemPrefab);
        itemInstanciate.Add(itemType.FlameBook, FlameBookItemPrefab);
        itemInstanciate.Add(itemType.Hallebarde, HallebardeItemPrefab);
        if (PV.IsMine)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        NewItemSelected();
    }
    
    void Update()
    {
        if(!PV.IsMine)return;
        //Item Pickup
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, playerReach)&& inventoryList.Count < 4)
        {
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if (item != null)
            {
                pickUpItem_gameobject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    inventoryList.Add(hitInfo.collider.GetComponent<ItemPickable>().itemScriptableObject.itemType);
                    item.PickItem();
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
            Instantiate(itemInstanciate[inventoryList[selectedItem]], position: throwItem_GameObject.transform.position, new Quaternion());
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
                VARIABLE.color = Color.green;
            }
            else
            {
                VARIABLE.color = Color.red;
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
        Hammer_item.ItemGameobjects.SetActive(false);
        Axe_item.ItemGameobjects.SetActive(false);
        LongSword_item.ItemGameobjects.SetActive(false);
        PharmacoBook_item.ItemGameobjects.SetActive(false);
        Arbalete_item.ItemGameobjects.SetActive(false);
        FlameBook_item.ItemGameobjects.SetActive(false);
        Hallebarde_item.ItemGameobjects.SetActive(false);
        GameObject selctedObject = itemSetActive[inventoryList[selectedItem]].ItemGameobjects;
        
        selctedObject.SetActive(true);

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("selectedItem",selectedItem );
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("selectedItem") && targetPlayer == PV.Owner)
        {
           selectedItem= (int)changedProps["selectedItem"];
           if(!PV.IsMine) 
               NewItemSelected();
        } 
    }
}

public interface IPickable
{
    void PickItem();
}
