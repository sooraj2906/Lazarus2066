using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField]private float speed = 2.0f;
    [SerializeField] private float sprintSpeed = 4.0f;
    public int maxStamina = 100;
    private float regen = 0.5f;
    public float rotationSpeed = 100;
    private float gravity = 8;
    private float rot = 0f;
    private bool isPaused = false;
    private bool isRunning = false;
    private Vector3 moveDir = new Vector3(0, -10, 0);
    private Animator anim;
    private CharacterController charController;
    private Rigidbody rb;
    public Interactable interactable;
    public UIManager uiManager;
    private Camera cam;

    public int currentHealth;
    public int currentStamina;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        uiManager = FindObjectOfType<UIManager>();
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
                
            }

            charController.Move(Vector3.forward * 0.001f);
            //Punch
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Mouse down");
                anim.SetTrigger("punch");
            }
            
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
                        //SetFocus(interactable);
                    }
                }
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

            rot = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rot, 0);
            charController.Move(moveDir * Time.deltaTime);
        }
        else if(isPaused == true)
        {
            if(Input.GetKey(KeyCode.P))
            {
                
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.name == "HandCollider")
        {
            Debug.Log(other.gameObject.name);
            currentHealth -= 10;
            uiManager.UpdateHealth(currentHealth);
        }
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
