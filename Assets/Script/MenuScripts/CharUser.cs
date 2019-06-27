using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharUser", menuName = "Slash 2D MMORPG/CharUser", order = 0)]
public class CharUser : ScriptableObject
{
    public int username {get;set; }
    public string nameChar {get;set; }
    public string race {get;set; }
    public string classe {get;set; }
    public int level {get;set; }
    public float experience {get;set; }
    public string guild {get;set; }
    public string inventory {get;set; }
    public int AGI {get;set; }
    public int STR {get;set; }
    public int INT {get;set; }
    public int VIT {get;set; }
    public int LUK {get;set; }
    public int DEX {get;set; }

}
