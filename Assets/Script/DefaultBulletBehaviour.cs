using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBulletBehaviour : MonoBehaviour
{
    public Staff staff;
    private int damage;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        damage = staff.damage;
        speed = staff.speed;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position += transform.up * speed * Time.deltaTime;
    }
}
