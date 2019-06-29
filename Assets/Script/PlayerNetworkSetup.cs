using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerNetworkSetup : NetworkBehaviour
{
    public override void OnStartLocalPlayer(){
        GetComponent<PlayerController>().enabled = true;
        GetComponent<Character>().enabled = true;
        Camera.main.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,-10f);
        Camera.main.transform.LookAt(this.transform.position);
        Camera.main.transform.parent = this.transform;

    }

    private void FixedUpdate() {
        if(!isLocalPlayer){
            return;
        }
        GameObject canvas = GameObject.Find("HUD");
        canvas.transform.parent = this.transform;
    }

}
