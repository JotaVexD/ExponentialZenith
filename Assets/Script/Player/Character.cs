using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Kryz.CharacterStats;

public class Character : MonoBehaviour
{
    public CharacterStat Strength;
    public CharacterStat Agility;
    public CharacterStat Intelligence;
    public CharacterStat Vitality;
    public CharacterStat Dexterity;
    public CharacterStat Luck;
    

    [SerializeField] Inventory inventory;
    [SerializeField] Chest chest;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    private ItemSlots draggedSlot;
    public GameObject ChestOpen;

    private void OnValidate(){
        if(itemTooltip == null){
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Start(){
        Strength.BaseValue = CharConfig.Instance.charData.STR;
        Agility.BaseValue = CharConfig.Instance.charData.AGI;
        Intelligence.BaseValue = CharConfig.Instance.charData.INT;
        Vitality.BaseValue = CharConfig.Instance.charData.VIT;
        Dexterity.BaseValue = CharConfig.Instance.charData.DEX;
        Luck.BaseValue = CharConfig.Instance.charData.LUK;
        statPanel.UpdateStatValues();
    }


    private void Awake()
    {
        statPanel.SetStats(Strength,Agility,Intelligence,Vitality,Dexterity,Luck);
        statPanel.UpdateStatValues();

        // Setup Events:
        // Right Click
        inventory.OnRightClickEvent += Equip;
        chest.OnRightClickEvent += GoToInventory;
        equipmentPanel.OnRightClickEvent += UnEquip;
        //Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        //Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        //Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        //End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        //Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        //Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
    }

    private void Equip(ItemSlots itemSlot){
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if(equippableItem != null){
            Equip(equippableItem);
        }
    }

    private void UnEquip(ItemSlots itemSlot){
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if(equippableItem != null){
            UnEquip(equippableItem);
        }
    }

    private void ShowTooltip(ItemSlots itemSlot){
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if(equippableItem != null){
            itemTooltip.ShowTooltip(equippableItem);
        }
    }

    private void HideTooltip(ItemSlots itemSlot){
        itemTooltip.HideTooltip();
    }

    private void BeginDrag(ItemSlots itemSlot){
        if(itemSlot.Item != null){
            draggedSlot = itemSlot;
            // draggableItem.enabled = true;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position =  Input.mousePosition;
        }
    }

    private void EndDrag(ItemSlots itemSlot){
        draggedSlot = null;
        // draggableItem.enabled = false;
    }

    private void Drag(ItemSlots itemSlot){
        if(draggableItem.enabled){

            draggableItem.transform.position = Input.mousePosition;
        }
    }

    private void Drop(ItemSlots dropItemSlot){
        if(dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item)){
            EquippableItem dragItem = draggedSlot.Item as EquippableItem;
            EquippableItem dropItem = dropItemSlot.Item as EquippableItem;
            
            if(draggedSlot is EquipmentSlot){
                if(dragItem != null) dragItem.Unequip(this);
                if(dropItem != null) dropItem.Equip(this);
            }
            if(dropItemSlot is EquipmentSlot){
                if(dragItem != null) dragItem.Equip(this);
                if(dropItem != null) dropItem.Unequip(this);
            }

            statPanel.UpdateStatValues();
            
            Item draggedItem = draggedSlot.Item;
            draggedSlot.Item = dropItemSlot.Item;
            dropItemSlot.Item = draggedItem;
        }
    }

    private void GoToInventory(SlotChests slotChest){
        RemoveFromChest(slotChest.Item);
    }

    public void RemoveFromChest(Item item){
        ChestOpen = GetComponent<PlayerController>().Chest;
        List<Item> itemsInThere = ChestOpen.GetComponent<ItemChest>().itemsInThere;
        itemsInThere.Remove(item);

        if(!inventory.isFull() && chest.RemoveItem(item)){
            inventory.AddItem(item);
        }
    }

    public void Equip(EquippableItem item)
    {
        if(inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if(equipmentPanel.AddItem(item,out previousItem))
            {
                if(previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void UnEquip(EquippableItem item)
    {
        if(!inventory.isFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }
}
