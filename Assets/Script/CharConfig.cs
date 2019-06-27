using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharConfig : MonoBehaviour
{
    private static CharConfig _instance;
    public static CharConfig Instance
    {
        get
        {
            if(_instance == null)
            {
                throw new System.Exception("No CharConfig File");
            }
            return _instance;
        }
    }
    public CharUser charData;

    void Awake() {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
