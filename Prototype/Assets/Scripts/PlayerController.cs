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
    public Interactable focus;
    public LayerMask movementMask;
    Camera cam;

    private int currentHealth;
    private int currentStamina;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        InvokeRepeating("Regenerate", 0.0f, 0.25f / regeneration);
        cam = Camera.main;
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

        if (Input.GetMouseButtonDown(1))
        {
            // We create a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits
            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    // Set our focus to a new focus
    void SetFocus(Interactable newFocus)
    {
        // If our focus has changed
        if (newFocus != focus)
        {
            // Defocus the old one
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;   // Set our new focus
            //motor.FollowTarget(newFocus);   // Follow the new focus
        }

        newFocus.OnFocused(transform);
    }

    // Remove our current focus
    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        //motor.StopFollowingTarget();
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