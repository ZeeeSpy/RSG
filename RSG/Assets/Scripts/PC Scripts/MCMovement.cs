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

    public GameObject bulletprefab;
    readonly private float bulletSpeed = 3f;

    public GameObject EMine;
    public GameObject TMine;
    public GameObject shell;

    [SerializeField]
    private int PISTOL_AMMO_COUNT;
    [SerializeField]
    private int PLAYER_HITPOINTS;
    [SerializeField]
    private int EMINE_COUNT;
    [SerializeField]
    private int TMINE_COUNT;

    private string[] EQP = new string[3];
    private int CurrentEQP;

    // Start is called before the first frame update
    void Start()
    {
        //initial values for equipment for debugging and playtesting
        audioSource = GetComponent<AudioSource>();

        PISTOL_AMMO_COUNT = 12;
        PLAYER_HITPOINTS = 10;
        EMINE_COUNT = 0;
        TMINE_COUNT = 0;

        
        CurrentEQP =  0;
        EQP[0] = "EQP: E.Mine x"+EMINE_COUNT;
        EQP[1] = "EQP: T.Mine x"+TMINE_COUNT;
        EQP[2] = "EQP: Cartridge x"+PISTOL_AMMO_COUNT;
        EQPText.text = EQP[CurrentEQP];

        Ammocounttext.text = "9x19mm : " + PISTOL_AMMO_COUNT;
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

        if (Input.GetButtonUp("EQP Cycle"))
        {
            CycleEQP();
        }

        if (Input.GetButtonUp("Use EQP") && !shooting) //can only use items when not shooting
        {
            UseEQP();
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

    private void Shoot()
    {
        if (endofAiming && shooting)
        {
            if (PISTOL_AMMO_COUNT > 0)
            {
                audioSource.PlayOneShot(gunsound, 1f);
                bulletcorection.x = Mathf.Clamp(aim.x, -0.03f, 0.07f);
                bulletcorection.y = Mathf.Clamp(aim.y, -0.05f, 0.05f);
                GameObject bullet = Instantiate(bulletprefab, transform.position + bulletcorection, Quaternion.identity);
                PISTOL_AMMO_COUNT--;
                Ammocounttext.text = "9x19mm : " + PISTOL_AMMO_COUNT;
                bullet.GetComponent<BulletScript>().velocity = aim * bulletSpeed;
                Destroy(bullet, 1f);
            } else
            {
                audioSource.PlayOneShot(emptygun, 1f);
            }
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
 
    public void GetAmmo(int Ammo)
    {
        PISTOL_AMMO_COUNT = PISTOL_AMMO_COUNT + Ammo;
        Ammocounttext.text = "9x19mm : " + PISTOL_AMMO_COUNT;
    }

    private void CycleEQP()
    {
        audioSource.PlayOneShot(equipgun, 1f);
        CurrentEQP++;
        if (CurrentEQP >= EQP.Length)
        {
            CurrentEQP = 0;
        }
        Debug.Log(EQP[CurrentEQP]);
        EQPText.text = EQP[CurrentEQP];
    }

    private void UseEQP() //ugly needs to be refactored. Use "item" objects
    {
        if (CurrentEQP == 0) //EMine
        {
            if (EMINE_COUNT > 0)
            {
                GameObject equipable = Instantiate(EMine, transform.position - new Vector3(0, 0.18f, 0), Quaternion.identity);
                EMINE_COUNT--;
                UpdateEQPUI();
            }
        } else if (CurrentEQP == 1) //TMine
        {
            if (TMINE_COUNT > 0)
            {
                GameObject equipable = Instantiate(TMine, transform.position - new Vector3(0, 0.18f, 0), Quaternion.identity);
                TMINE_COUNT--;
                UpdateEQPUI();
            }
        } else if (CurrentEQP == 2) //Cartridge
        {
            if (PISTOL_AMMO_COUNT > 0)
            {
                //Needs to be a system simmilar to shooting
                PISTOL_AMMO_COUNT--;
                UpdateEQPUI();
                Ammocounttext.text = "9x19mm : " + PISTOL_AMMO_COUNT;
            }
        }
    }

    private void UpdateEQPUI()
    {
        EQP[0] = "EQP: E.Mine x" + EMINE_COUNT;
        EQP[1] = "EQP: T.Mine x" + TMINE_COUNT;
        EQP[2] = "EQP: Cartridge x" + PISTOL_AMMO_COUNT;
        EQPText.text = EQP[CurrentEQP];
    }
}
