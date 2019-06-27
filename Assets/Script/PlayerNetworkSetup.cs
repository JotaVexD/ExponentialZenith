using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetworkSetup : NetworkBehaviour
{
    public override void OnStartLocalPlayer(){
        GetComponent<PlayerController>().enabled = true;
        Camera.main.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,-10f);
        Camera.main.transform.LookAt(this.transform.position);
        Camera.main.transform.parent = this.transform;
    }

}
