using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CameraFollow : MonoBehaviour
 {
    Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;

    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
 }
 