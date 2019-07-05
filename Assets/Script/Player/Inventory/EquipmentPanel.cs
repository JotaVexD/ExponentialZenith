using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

[Serializable]
public class EquippedItens
{
    public int itemId;
    public string itemName;
    public EquipmentType equipmentType;
}

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] List<EquippableItem> startingEquipableItems;
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

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
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            equipmentSlots[i].OnPointerClickEvent += OnPointerClickEvent;
            equipmentSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            equipmentSlots[i].OnRightClickEvent += OnRightClickEvent;
            equipmentSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            equipmentSlots[i].OnEndDragEvent += OnEndDragEvent;
            equipmentSlots[i].OnDragEvent += OnDragEvent;
            equipmentSlots[i].OnDropEvent += OnDropEvent;
        }
    }

    private void OnValidate(){
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();

    }

    public void GetFromDBEquip(){
        StartCoroutine(GetEquip());
    }

    public bool AddItem(EquippableItem item,out EquippableItem previousItem,int x)
    {
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (EquippableItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                if(x != 0){
                    SendingToDB();
                }
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItem item)
    {
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                SendingToDB();
                return true;
            }
        }
        return false;
    }

    public void SendingToDB(){
        int i = 0;
        EquippedItens[] equippedInstance = new EquippedItens[equipmentSlots.Length];
        for(; i < equipmentSlots.Length; i++){
            if(equipmentSlots[i].Item != null){
                equippedInstance[i] = new EquippedItens();
                equippedInstance[i].itemId = equipmentSlots[i]._item.id;
                equippedInstance[i].itemName = equipmentSlots[i]._item.ItemName;
                equippedInstance[i].equipmentType = equipmentSlots[i].EquipmentType;
            }
        }

        string equippedToJason = JsonHelper.ToJson(equippedInstance, true);
        StartCoroutine(SendEquip(equippedToJason));
        
    }

    public IEnumerator SendEquip(string equippedToJason)
    {
        yield return new WaitForSeconds(0.2f);
        WWWForm form = new WWWForm();
        form.AddField("equipped", equippedToJason);
        form.AddField("charName", CharConfig.Instance.charData.nameChar);
        string url = "http://192.168.0.57/mmo2d/updateequippable.php";

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

    public IEnumerator GetEquip()
    {
        WWWForm form = new WWWForm();
        form.AddField("charName", CharConfig.Instance.charData.nameChar);
        string url = "http://192.168.0.57/mmo2d/getequippable.php";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                EquippedItens[] equipped = JsonHelper.FromJson<EquippedItens>(www.downloadHandler.text);
                    
                for(int x = 0; x < equipped.Length;x++){
                    if(equipped[x].itemId != 0 && equipped[x].itemName != null){
                        string aux = equipped[x].itemId + "_" + equipped[x].itemName;
                        string[] itens = AssetDatabase.FindAssets("t:Item "+aux);
                        foreach (string guid in itens)
                        {
                            EquippableItem item_temp = (EquippableItem)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(EquippableItem));
                            
                            GameObject test = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
                            test.GetComponent<Character>().Equip(item_temp,1);
                        } 
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        this.transform.parent.gameObject.SetActive(false);
        yield break;
    }
}
