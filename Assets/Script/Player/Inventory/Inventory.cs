using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.Serialization;

[Serializable]
public class InventoryItens
{
    public int itemId;
    public string itemName;
}

public class Inventory : MonoBehaviour
{
    [FormerlySerializedAs("items")]
    [SerializeField] List<Item> startingItems;

    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlots[] itemSlots;

    public event Action<ItemSlots> OnPointerEnterEvent;
    public event Action<ItemSlots> OnPointerClickEvent;
    public event Action<ItemSlots> OnPointerExitEvent;
    public event Action<ItemSlots> OnRightClickEvent;
    public event Action<ItemSlots> OnBeginDragEvent;
    public event Action<ItemSlots> OnEndDragEvent;
    public event Action<ItemSlots> OnDragEvent;
    public event Action<ItemSlots> OnDropEvent;

    private void Start()
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            itemSlots[i].OnPointerClickEvent += OnPointerClickEvent;
            itemSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            itemSlots[i].OnRightClickEvent += OnRightClickEvent;
            itemSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            itemSlots[i].OnEndDragEvent += OnEndDragEvent;
            itemSlots[i].OnDragEvent += OnDragEvent;
            itemSlots[i].OnDropEvent += OnDropEvent;
        }
        SetStartingItems();
    }

    private void OnValidate(){
        if(itemsParent != null){
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlots>();
            
        }

        SetStartingItems();
    }

    private void SetStartingItems(){
        int i = 0;

        for(;i < startingItems.Count && i < itemSlots.Length; i++){
            itemSlots[i].Item = startingItems[i];
        }

        for(;i < itemSlots.Length; i++){
            itemSlots[i].Item = null;
        }
    }

    public bool AddItem(Item item){
        for(int i = 0; i < itemSlots.Length; i++){
            if(itemSlots[i].Item == null){
                itemSlots[i].Item = item;
                SendingToDB();
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item){
        for(int i = 0; i < itemSlots.Length; i++){
            if(itemSlots[i].Item == item){
                itemSlots[i].Item = null;
                SendingToDB();
                return true;
            }
        }
        return false;
    }

    public bool isFull(){
        for(int i = 0; i < itemSlots.Length; i++){
            if(itemSlots[i].Item == null){
                return false;
            }
        }
        return true;
    }

    public void GetFromDB(){
        StartCoroutine(GetInv());
    }

    public void SendingToDB(){
        int i = 0;
        InventoryItens[] inventoryInstance = new InventoryItens[itemSlots.Length];
        for(; i < itemSlots.Length; i++){
            if(itemSlots[i].Item != null){
                inventoryInstance[i] = new InventoryItens();
                inventoryInstance[i].itemId = itemSlots[i]._item.id;
                inventoryInstance[i].itemName = itemSlots[i]._item.ItemName;
            }
        }

        string inventoryToJason = JsonHelper.ToJson(inventoryInstance, true);
    
        StartCoroutine(SendInv(inventoryToJason));
        
    }

    public IEnumerator SendInv(string inventoryToJason)
    {
        WWWForm form = new WWWForm();
        form.AddField("inventory", inventoryToJason);
        form.AddField("charName", CharConfig.Instance.charData.nameChar);
        string url = "http://192.168.0.57/mmo2d/updateinventory.php";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                yield break;
            }
        }
        
    }

    public IEnumerator GetInv()
    {
        WWWForm form = new WWWForm();
        form.AddField("charName", CharConfig.Instance.charData.nameChar);
        string url = "http://192.168.0.57/mmo2d/getinventory.php";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                InventoryItens[] inventory = JsonHelper.FromJson<InventoryItens>(www.downloadHandler.text);
                    
                for(int x = 0; x < inventory.Length;x++){
                    if(inventory[x].itemId != 0 && inventory[x].itemName != null){
                        string aux = inventory[x].itemId + "_" + inventory[x].itemName;
                        string[] itens = AssetDatabase.FindAssets("t:Item "+aux);
                        foreach (string guid in itens)
                        {
                            Item item_temp = (Item)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Item));
                            AddItem(item_temp);
                        } 
                    }
                }
            }
        }
        yield return new WaitForSeconds(1f);
        this.transform.parent.Find("EquipmentPanel").gameObject.GetComponent<EquipmentPanel>().GetFromDBEquip();
    }
}
