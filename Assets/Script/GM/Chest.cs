using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Chest : MonoBehaviour
{
    [SerializeField] Transform itemsParent;
    [SerializeField] SlotChests[] slotChest;
    

    public event Action<SlotChests> OnRightClickEvent;

    private void Start()
    {
        for(int i = 0; i < slotChest.Length; i++)
        {

            slotChest[i].OnRightClickEvent += OnRightClickEvent;
        }
    }

    private void OnValidate(){
        if(itemsParent != null){
            slotChest = itemsParent.GetComponentsInChildren<SlotChests>();
        }

    }

    public bool AddItem(Item item){
        for(int i = 0; i < slotChest.Length; i++){
            if(slotChest[i].Item == null){
                slotChest[i].Item = item;
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item){
        for(int i = 0; i < slotChest.Length; i++){
            if(slotChest[i].Item == item){
                slotChest[i].Item = null;
                return true;
            }
        }
        return false;
    }

    public bool isFull(){
        for(int i = 0; i < slotChest.Length; i++){
            if(slotChest[i].Item == null){
                return false;
            }
        }
        return true;
    }
}
