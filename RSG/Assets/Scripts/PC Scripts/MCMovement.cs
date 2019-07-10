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
                shooting = true;
            }
        } else
        {
            shooting = false;
        }
    }

    private void Animate()
    {
        if (movement != Vector3.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movementspeed);
        rb.velocity = movement * movementspeed;
    }

    private void ShootingStance() {

    }
}
