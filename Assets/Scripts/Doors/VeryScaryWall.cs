using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeryScaryWall : MonoBehaviour, IInteractable
{
    public float distanceToOpen = 20f;

    public float speedOfOpening = 5f;


    private Vector3 initialPosition;
    private bool opened = false;

    private bool opening = false;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            transform.position += Vector3.down * speedOfOpening * Time.deltaTime;
            if (Vector3.Distance(transform.position, initialPosition) >= distanceToOpen)
            {
                opening = false;
            }
        }
    }

    public void Interact(PlayerController playerController)
    {
        if (!opened)
        {
            opened = true;
            opening = true;
        }
    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
}
