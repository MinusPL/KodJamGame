using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;

    private Transform target;
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
                if (Vector3.Angle(target.transform.forward, transform.position - target.transform.position) < 10)
                {
                    var playerController = target.GetComponent<PlayerController>();
                    playerController.DecreaseHealth();
                }
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
                if (Physics.Raycast(transform.position, (transform.position - collider.transform.position).normalized,
                    out hit, lookRadius))
                {
                    if(hit.collider.CompareTag("Player"))
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
}