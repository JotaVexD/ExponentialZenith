using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Slash 2D MMORPG/Item", order = 0)]
public class Item : ScriptableObject
{
    public int id;	
    public string ItemName;
    public Sprite Icon;
}
