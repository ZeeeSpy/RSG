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
    private Camera currentcamera;
    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        LayerMask maska = LayerMask.GetMask("PlayerSquares");
        LayerMask maskb = LayerMask.GetMask("Ignore Raycast");
        mask = maska | maskb;
        mask = ~mask;
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
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        rb.velocity = new Vector2(movement.x, movement.y);
    }

    private void ShootingStance() {

        RaycastHit2D ray = Physics2D.Raycast(transform.position, aim, 5, mask);
        Debug.DrawLine(transform.position, ray.point, Color.green);
  
    }
}
