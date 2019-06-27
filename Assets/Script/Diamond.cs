using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Diamond : NetworkBehaviour
{
    [Command]
    void CmdPickUp(){
        // GameObject.Find("Player").GetComponent<Renderer>().material.color = Color.black;
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        CmdPickUp();
    }
}
