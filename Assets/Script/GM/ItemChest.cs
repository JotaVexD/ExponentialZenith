using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    public List<Item> itemsInThere;
    [SerializeField] KeyCode itemPickupKeycode = KeyCode.E;

    private bool isInRange;
    private bool openChest;
    private bool openChestOneTime;

    private void Update() {
        if(Input.GetKeyDown(itemPickupKeycode) && isInRange == true){
            openChest = true;
            openChestOneTime = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        
    }

    private void OnTriggerExit2D(Collider2D other) {

    }

    private void OpenChest(Collider2D other){
        
    }
}
