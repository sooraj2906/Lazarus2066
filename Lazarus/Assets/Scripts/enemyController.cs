using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class enemyController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float playerRange = 3.0f;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private GameObject Player;
    private bool isFollow;
    private Vector3 startPos;


    void Start()
    {
        anim = GetComponent<Animator>();
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
            isFollow = true;
            anim.SetBool("walk", true);
            Agent.SetDestination(Player.transform.position);
            //Attack player if within attack range
            if(Vector3.Distance(this.transform.position, Player.transform.position) <= 2.0f)
            {
                anim.SetTrigger("attack");
                StartCoroutine("WaitSec");
            }
        }
        //Go back to start position after player moves out of range
        else
        {
            Agent.SetDestination(startPos);
            isFollow = false;
            if(Vector3.Distance(this.transform.position, startPos) <= 0.5f && anim.GetBool("walk") == true)
            {
                anim.SetBool("walk", false);
            }
        }
    }

    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(1.0f);
    }
    
}
