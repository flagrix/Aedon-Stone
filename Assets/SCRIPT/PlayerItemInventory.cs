using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInventory : MonoBehaviour
{
    [Header("General")]
    public List<itemType> inventoryList;
    public int selectedItem = 0;
    
    [Space(20)] [Header("Keys")]
    [Header("Keys")]
    [SerializeField]KeyCode throwItemKey;

    [SerializeField] private KeyCode pickItemKey;
    
    [Space(20)]
    [Header("Item gameobjects")]
    [SerializeField] GameObject Hammer_item;
    [SerializeField] GameObject Axe_item;
    [SerializeField] GameObject LongSword_item;
    [SerializeField] GameObject PharmacoBook_item;
    [SerializeField] GameObject Arbalete_item;
    [SerializeField] GameObject FlameBook_item;
    [SerializeField] GameObject Hallebarde_item;
    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { } ;

    void Start()
    {
        itemSetActive.Add(itemType.Hammer, Hammer_item);
        itemSetActive.Add(itemType.Axe, Axe_item);
        itemSetActive.Add(itemType.LongSword, LongSword_item);
        itemSetActive.Add(itemType.PharmacoBook, PharmacoBook_item);
        itemSetActive.Add(itemType.Arbalete, Arbalete_item);
        itemSetActive.Add(itemType.FlameBook, FlameBook_item);
        itemSetActive.Add(itemType.Hallebarde, Hallebarde_item);
        
        NewItemSelected();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 0)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 0)
        {
            selectedItem = 2;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Count > 0)
        {
            selectedItem = 3;
            NewItemSelected();
        }
    }
    private void NewItemSelected()
    {
        Hammer_item.SetActive(false);
        Axe_item.SetActive(false);
        LongSword_item.SetActive(false);
        PharmacoBook_item.SetActive(false);
        Arbalete_item.SetActive(false);
        FlameBook_item.SetActive(false);
        Hallebarde_item.SetActive(false);
        GameObject selctedObject = itemSetActive[inventoryList[selectedItem]];
        
        selctedObject.SetActive(true);
    }
}
