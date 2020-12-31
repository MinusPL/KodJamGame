using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public enum eSTATE
	{
        IDLE,
        WANDER,
        WALK_TO_POINT,
        FOLLOW,
        ATTACK
	}
    
    public float lookRadius = 10f;
    public float attackRadius = 4.0f;
    public float damage = 4.0f;
    public float attackCooldown = 0.07f;
    public GameObject model;
    public GameObject lightTarget;
    float attackTimer = 0.0f;

    private Animator anim;
    private Transform target;
    private NavMeshAgent agent;
    private CharacterController characterController;

    private eSTATE state;

    // Start is called before the first frame update
    void Start()
    {
        anim = model.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + (transform.forward * 10.0f), Color.blue);
        
        
        Debug.DrawLine(transform.position, transform.position + (target.transform.position - transform.position).normalized * 10.0f, Color.green);

        System.Random rnd = new System.Random();
        int check = 0;
        switch (state)
		{
            case eSTATE.IDLE:
                anim.SetBool("moving", false);
                check = rnd.Next(1, 100);
                if (lookForPlayer())
                {
                    agent.SetDestination(target.position);
                    FaceTarget();
                    state = eSTATE.FOLLOW;
                }
                else
                {
                    if (check <= 25) state = eSTATE.WANDER;
                }
			break;
            case eSTATE.WANDER:
                anim.SetBool("moving", false);
                rnd = new System.Random();
                check = rnd.Next(1, 100);
                if (lookForPlayer())
                {
                    agent.SetDestination(target.position);
                    FaceTarget();
                    state = eSTATE.FOLLOW;
                }
                else
                {
                    if (check <= 20)
					{
                        state = eSTATE.IDLE;
                    }
                    else
					{
                        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 5.0f;
                        randomDirection += transform.position;
                        NavMeshHit hit;
                        NavMesh.SamplePosition(randomDirection, out hit, 5.0f, 1);
                        agent.SetDestination(hit.position);
                        state = eSTATE.WALK_TO_POINT;
                    }
                }
                break;
            case eSTATE.WALK_TO_POINT:
                anim.SetBool("moving", true);
                
                break;
		}

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            anim.SetBool("moving", (agent.velocity.magnitude / agent.speed) > 0.0);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }

        attackTimer -= Time.deltaTime;

        if (distance <= attackRadius)
        {
            if (attackTimer <= 0.0f)
            {
                attackTimer = attackCooldown;
                OnAttack();
            }
        }
    }

    private bool lookForPlayer()
	{
        if (target == null) return false;
        else
		{
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
			{
                return true;
			}
            else
			{
                return false;
			}
        }
    }

    private void FixedUpdate()
    {
        var colliders = Physics.OverlapSphere(transform.position, lookRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (collider.transform.position - transform.position).normalized,
                    out hit, lookRadius))
                {
                    //Debug.DrawLine(transform.position, collider.transform.position);
                    //Debug.DrawRay(transform.position, transform.position + ((collider.transform.position - transform.position).normalized * 10.0f), Color.red);
                    //Debug.DrawRay(transform.position, (collider.transform.position - transform.position).normalized);
                    //Debug.DrawLine(transform.position, hit.point, Color.green);
                    if (hit.collider.CompareTag("Player"))
                    {
                        target = collider.transform;
                    }
                    else
                    {
                        target = null;
                    }
                }
                return;
            }
        }

        target = null;
    }

	void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
	{
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.white;
    }

    private void OnAttack()
	{
        var relativePoint = transform.InverseTransformPoint(target.transform.position);
        if (relativePoint.x >= -5.5 && relativePoint.x <= 5.5)
		{
            anim.SetTrigger("attack");
            var playerController = target.GetComponent<PlayerController>();
            playerController.DecreaseHealth(damage);
        }
        
        /*if (Vector3.Angle(target.transform.forward, (target.transform.position - transform.position).normalized) < 20)
        {
            var playerController = target.GetComponent<PlayerController>();
            playerController.DecreaseHealth(damage);
        }*/
    }

    public void SetLightFlashed(bool flag)
	{
        anim.SetBool("flashed",flag);
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddEnemy(gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().RemoveEnemy(gameObject);
        }
    }

}