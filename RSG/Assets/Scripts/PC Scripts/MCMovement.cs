/*
 *  Script used to control the player, including "shooting stance" and playing sounds
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text Ammocounttext;
    public Text EQPText;
    public Slider hpslider;

    public AudioClip gunsound;
    public AudioClip equipgun;
    public AudioClip emptygun;
    AudioSource audioSource;
    bool playaudio = true;
    private bool endofAiming;
    private GameOverScript gameover;

    [SerializeField]
    private int PLAYER_HITPOINTS;

    public SpriteRenderer InteractIcon;
    private LightSwitchScript currentlightswitch;

    // Start is called before the first frame update
    void Start()
    {
        //initial values for equipment for debugging and playtesting
        audioSource = GetComponent<AudioSource>();
        /*
        PISTOL_AMMO_COUNT = 12;
        PLAYER_HITPOINTS = 10;
        EMINE_COUNT = 3;
        TMINE_COUNT = 0;
        */

        gameover = (GameOverScript)Object.FindObjectOfType(typeof(GameOverScript));

    }

    // Update is called once per frame
    void Update()
    {
        currentcamera = Camera.current;
        ReadInputs();
        if (shooting)
        {
            ShootingStance();
        } else
        {
            playaudio = true;
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
                aim.Normalize();
                shooting = true;
            }
        } else
        {
            shooting = false;
        }

        if (InteractIcon.enabled == true && Input.GetButtonUp("Interact"))
        {
            currentlightswitch.Togglelight();
        }

        endofAiming = Input.GetButtonUp("Shoot");
    }


    private void ShootingStance()
    {
        if (playaudio)
        {
            audioSource.PlayOneShot(equipgun, 1f);
            playaudio = false;
        }
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

    public void DamagePlayer(int dmg)
    {
        PLAYER_HITPOINTS = PLAYER_HITPOINTS - dmg;
        hpslider.value = PLAYER_HITPOINTS;
        if (PLAYER_HITPOINTS <= 0)
        {
            //Player Dead
            Debug.Log("Player Died");
            gameover.GameOver();
        }
    }

    public void HealPlayer(int heal)
    {
        if (heal < 0)
        {
            return;
        }
        PLAYER_HITPOINTS = PLAYER_HITPOINTS + heal;
        if (PLAYER_HITPOINTS > 10)
        {
            PLAYER_HITPOINTS = 10;
        }
    }

    public void ToggleInteractable(LightSwitchScript lightswitch)
    {
        currentlightswitch = lightswitch;
        InteractIcon.enabled = !InteractIcon.enabled;
    }
}
