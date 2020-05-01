using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int maxHealth = 100;
    [SerializeField]private float speed = 2.0f;
    [SerializeField] private float sprintSpeed = 4.0f;
    private int maxStamina = 100;
    private float regen = 0.5f;
    private float rotationSpeed = 100;
    private float gravity = 8;
    private float rot = 0f;
    private bool isPaused = false;
    private bool isRunning = false;
    private Vector3 moveDir = new Vector3(0, -10, 0);
    private Animator anim;
    private CharacterController charController;
    private Rigidbody rb;
    private Interactable interactable;
    private Camera cam;
    public GameObject pauseMenu;

    private int currentHealth;
    private int currentStamina;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        cam = Camera.main;
    }
    
    void Update()
    {
        if (isPaused == false)
        {
            if(Input.GetKey(KeyCode.P))
            {
                Pause(true);
            }

            charController.Move(Vector3.forward * 0.001f);
            //Punch
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Mouse down");
                anim.SetTrigger("punch");
            }

            //if (charController.isGrounded)
            // {
            //Movement
            if (Input.GetKey(KeyCode.W))
            {
                isRunning = true;
                //Debug.Log("W down");
                anim.SetInteger("Condition", 1);
                moveDir = new Vector3(0, -10, 1);
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir);
            }
            else
            {
                isRunning = false;
                //Debug.Log("W up");
                anim.SetInteger("Condition", 0);
                moveDir = new Vector3(0, -10, 0);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (isRunning == false)
                {
                    anim.Play("jump");
                    //anim.SetTrigger("jump");
                }
                else if (isRunning == true)
                {
                    anim.Play("runJump");
                    //anim.SetTrigger("runjump");
                }
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                anim.Play("FrontFlip");
            }

            rot += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, rot, 0);

            charController.Move(moveDir * Time.deltaTime);
        }
        else if(isPaused == true)
        {
            if(Input.GetKey(KeyCode.P))
            {
                Pause(false);
            }
        }
        
    }

    private void Pause(bool pause)
    {
        if(pause==true)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "level5Collider")
        {
            //Destroy(level5.gameObject);
        }
    }
    

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
