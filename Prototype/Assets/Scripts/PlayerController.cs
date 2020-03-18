using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int maxStamina = 100;
    public float regeneration = 1.0f;
    public float speed = 2.0f;
    public float sprintSpeed = 4.0f;
    public float rotationSpeed = 100;
    public float gravity = 8;
    public float rot = 0f;
    public Vector3 moveDir = new Vector3(0, -10, 0);
    public Animator anim;
    public CharacterController charController;

    private int currentHealth;
    private int currentStamina;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        InvokeRepeating("Regenerate", 0.0f, 0.25f / regeneration);
    }
    
    void Update()
    {

        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                //showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                //hidePaused();
            }
        }

        if (Time.timeScale == 1)
        {



            //Debug.Log(charController.isGrounded);
            //Punch
            if (Input.GetMouseButtonDown(0) && currentStamina > 10)
            {
                //Debug.Log("Mouse down");
                anim.SetTrigger("punch");
                currentStamina -= 10;
                //UIManager.instance.UpdateStamina(currentStamina);
            }

            if (charController.isGrounded)
            {
                //Movement
                if (Input.GetKey(KeyCode.W))
                {
                    //Debug.Log("W down");
                    anim.SetInteger("Condition", 1);
                    moveDir = new Vector3(0, -10, 1);
                    moveDir *= speed;
                    moveDir = transform.TransformDirection(moveDir);

                }
                else
                {
                    //Debug.Log("W up");
                    anim.SetInteger("Condition", 0);
                    moveDir = new Vector3(0, -10, 0);
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetInteger("Sprint", 1);
                    moveDir *= sprintSpeed;
                }
                else
                {
                    anim.SetInteger("Sprint", 0);
                }

                rot += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
                transform.eulerAngles = new Vector3(0, rot, 0);
                moveDir.y -= gravity * Time.deltaTime;
                charController.Move(moveDir * Time.deltaTime);
            }

            UIManager.instance.UpdateStamina(currentStamina);

        }
    }

    public void TookDamage(int damage)
    {

            currentHealth -= damage;
            anim.SetTrigger("HitDamage");
            UIManager.instance.UpdateHealth(currentHealth);

    }

    void Regenerate()
    {
        if (currentStamina < maxStamina)
            currentStamina += 1;
    }

}
