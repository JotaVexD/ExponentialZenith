using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Objects Asset
    public Staff staff;
    private Sprite projectileSprite;
    private int damage;
    private float speed;

    private GameObject go;

    public GameObject projectileDefault;
    public Rigidbody projectile;

    // Objects
    public Transform firePoint;

    // Weapon Variables
    public float atackSpeed;
    private float timer = 0f;

    void Start(){
        projectileSprite = staff.projectileSprite;
        damage = staff.damage;
        speed = staff.speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0)){
            if(timer >= atackSpeed){
                Shoot();
            }
        }

    }

    void Shoot(){
        // if(Player.GetComponent<PlayerController>().moveInput != 0){
        timer = 0f;
        go = Instantiate(projectileDefault,firePoint.position, firePoint.rotation);
        go.GetComponent<SpriteRenderer>().sprite = projectileSprite;
        go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * speed);
        Destroy(go,5);
    }

    // void moveGameObject(GameObject go){
    //     go.transform.position += transform.up * speed * Time.deltaTime;
    // }
}
