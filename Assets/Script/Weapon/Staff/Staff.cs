using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff", menuName = "Weapon/Staff")]
public class Staff : ScriptableObject 
{
    public new string name;
    public string description;

    public Sprite artwork;
    public int damage;
    public float speed;
    public Sprite projectileSprite;

}
