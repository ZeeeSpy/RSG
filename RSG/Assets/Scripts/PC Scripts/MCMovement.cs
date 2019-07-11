using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCMovement : MonoBehaviour
{
    readonly private float charspeed = 0.8f;
    public Animator animator;

    public Rigidbody2D rb;
    private Vector3 movement;
    private bool shooting = false;
    private Vector3 aim;
    private float movementspeed;
    private Camera currentcamera;
    private Vector2 aimingdirection;
    private Vector3 bulletcorection;
    //[SerializeField]
    //private int PISTOL_AMMO_COUNT = 0;


    private bool endofAiming;


    public GameObject bulletprefab;
    readonly private float bulletSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentcamera = Camera.current;
        ReadInputs();
        if (shooting)
        {
            ShootingStance();
        }
        Animate();
        Shoot();
    }

    private void ReadInputs()
    {
        movement = new Vector3((Input.GetAxis("Horizontal") * (charspeed)), Input.GetAxis("Vertical") * (charspeed), 0.0f);
        movementspeed = Mathf.Clamp(movement.magnitude, 0.0f, 1.0f);
        movement.Normalize();

        if (Input.GetButton("Shooting Stance"))
        {
            if (currentcamera)
            {
                aim = currentcamera.ScreenToWorldPoint(Input.mousePosition);
                aim.z = 0;
                aim = aim - transform.position;
                aim.Normalize();
                shooting = true;
            }
        } else
        {
            shooting = false;
        }

        endofAiming = Input.GetButtonUp("Fire1");
    }


    private void ShootingStance()
    {
        rb.velocity = Vector2.zero;
        movementspeed = 0;
    }

    private void Animate()
    {
        if (movement != Vector3.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        if (!shooting)
        {
            animator.SetFloat("Aiming", 0f);
            rb.velocity = movement * movementspeed;
            animator.SetFloat("Speed", movementspeed);
        }

        if (shooting)
        {
            animator.SetFloat("Aiming", 1f);
            animator.SetFloat("Speed", 0);
            animator.SetFloat("Horizontal", aim.x);
            animator.SetFloat("Vertical", aim.y);
        }

    }

    private void Shoot()
    {
        if (endofAiming && shooting)
        {
            bulletcorection.x = Mathf.Clamp(aim.x, -0.04f, 0.07f);
            bulletcorection.y = Mathf.Clamp(aim.y, -0.05f, 0.05f);
            GameObject bullet = Instantiate(bulletprefab, transform.position+bulletcorection, Quaternion.identity);
            bullet.GetComponent<BulletScript>().velocity = aim * bulletSpeed;
            Destroy(bullet, 1f);
        }
    }
 
}
