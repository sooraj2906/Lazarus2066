using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class enemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Vector3 Target;
    [SerializeField] private bool PathIsValid = true;
    [SerializeField] private float Max_X;
    [SerializeField] private float Max_Z;
    [SerializeField] private float Min_X;
    [SerializeField] private float Min_Z;
    [SerializeField] float randoms;
    [SerializeField] private GameObject Player;
    [SerializeField] private bool Follow = false;
    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        CheckPoints();
    }
    
    void Update()
    {
        randoms += Time.deltaTime;
        CharacterMove();
        Behaviour();
    }

    void CharacterMove()
    {
        if (Vector3.Distance(this.transform.position, new Vector3(Target.x, Target.y, Target.z)) < 5f)
        {

            if (PathIsValid)
            {
                int checkrand = (int)randoms;

                if (checkrand % 2 == 0)
                {

                    StartCoroutine(WaitForsometime());

                }
                else
                {
                    CheckPoints();
                }

            }
        }
    }

    void CheckPoints()
    {
        Vector3 checkpath = new Vector3(Random.Range(Min_X, Max_X), transform.position.y, Random.Range(Min_Z, Max_Z));
        NavMeshPath path = new NavMeshPath();
        Agent.CalculatePath(checkpath, path);

        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            PathIsValid = false;
            //Debug.Log("PathNotValid");
            CheckPoints();
        }
        else
        {
            Follow = false;
            PathIsValid = false;
            //Debug.Log("PathIsValid");
            Target = checkpath;
            Agent.SetDestination(Target);
            PathIsValid = true;
            StopCoroutine(WaitForsometime());
        }
    }

    IEnumerator WaitForsometime()
    {

        Debug.Log("waitforsec");
        yield return new WaitForSeconds((float)Random.Range(6, 10));
        Agent.isStopped = true;
        yield return new WaitForSeconds((float)Random.Range(15, 20));
        Agent.isStopped = false;
        StopCoroutine(WaitForsometime());
    }

    void Behaviour()
    {
        if (Vector3.Distance(this.transform.position, Player.transform.position) < 10f)
        {
            Follow = true;
            Agent.SetDestination(Player.transform.position);
        }
        else
        {
            if (Follow)
            {
                CheckPoints();
            }
        }
    }
}
