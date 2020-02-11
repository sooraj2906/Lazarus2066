using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4.0f;
    public float rotationSpeed = 100;
    public float gravity = 8;
    public float rot = 0f;
    public Vector3 moveDir = new Vector3(0, -10, 0);
    public Animator anim;
    public CharacterController charController;
    void Start()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        //Debug.Log(charController.isGrounded);
        //Punch
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse down");
            anim.SetTrigger("punch");
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

            rot += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, rot, 0);
            moveDir.y -= gravity * Time.deltaTime;
            charController.Move(moveDir * Time.deltaTime);
        }
        
    }
}
