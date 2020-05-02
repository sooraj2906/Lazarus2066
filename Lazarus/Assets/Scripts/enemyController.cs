using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class enemyController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float playerRange = 5.0f;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private GameObject Player;
    [SerializeField] private float rotationSpeed = 10f;
    public UIManager uiManager;
    public PlayerController playerController;
    private bool isFollow = false;
    public GameObject lvl5, lvl5stairs, lvl4, lvl4stairs, lvl3, lvl3stairs, lvl2, lvl2stairs;
    private Vector3 startPos;
    public GameObject col;

    float attackDelay =1, nextAttack = 0;
    bool isAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
        playerController = FindObjectOfType<PlayerController>();
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        Agent = this.GetComponent<NavMeshAgent>();
        isFollow = false;
        startPos = this.transform.position;
    }
    
    void Update()
    {
        //Wait for player in idle
        //Chase player when within range
        if(Vector3.Distance(this.transform.position, Player.transform.position) <= playerRange)
        {

            //Attack player if within attack range
            if (Vector3.Distance(this.transform.position, Player.transform.position) <= 1.25f || isAttacking)
            {
                Debug.Log("Stopped Moving");
                //Agent.angularSpeed = 0;
                Agent.isStopped = true;
                anim.SetBool("walk", false);
                if (Time.time > nextAttack)
                {
                    StartCoroutine(Attack());
                    Debug.Log("Hitting");
                    anim.SetTrigger("attack");
                    //playerController.currentHealth -= 10;
                    //uiManager.UpdateHealth(playerController.currentHealth);
                    nextAttack = Time.time + attackDelay;
                }
            }

            else
            {
                Debug.Log("Following");
                Agent.isStopped = false;
                isFollow = true;
                anim.SetBool("walk", true);
                Agent.SetDestination(Player.transform.position);// - new Vector3(0.4f, 0, 0.4f));y
            }
        }
        //Go back to start position after player moves out of range
        else
        {
            Agent.isStopped = false;
            //Agent.angularSpeed = 120;
            Agent.SetDestination(startPos);
            isFollow = false;
            if (Vector3.Distance(this.transform.position, startPos) <= 0.5f && anim.GetBool("walk") == true)
            {
                anim.SetBool("walk", false);
            }
        }
    }

    void AttackColliderEnable()
    {
        col.GetComponent<SphereCollider>().enabled = false;
    }

    void AttackColliderDisable()
    {
        col.GetComponent<SphereCollider>().enabled = true;
    }
    //private void RotateTowards(GameObject target)
    //{
    //    Transform targetTransform = target.transform;
    //    Vector3 direction = (targetTransform.position - transform.position).normalized;
    //    Quaternion lookRotation = Quaternion.LookRotation(direction);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    //}

    //private void FaceTarget(GameObject target)
    //{
    //    Vector3 lookPos = target.transform.position - transform.position;
    //    lookPos.y = 0;
    //    Quaternion rotation = Quaternion.LookRotation(lookPos);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    //}

    IEnumerator Attack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.6333f );
        isAttacking = false;
    }
}
