using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item){
        for(int i = 0; i < itemSlots.Length; i++){
            if(itemSlots[i].Item == item){
                itemSlots[i].Item = null;
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
}
