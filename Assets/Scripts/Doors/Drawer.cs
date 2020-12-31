using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour, IInteractable
{
    public DoorState state = DoorState.CLOSED;
    public float distanceToOpen = 0.5f;
    public float speedOfOpening = 5f;
    private Vector3 initialPosiiton;
    // Start is called before the first frame update

    public AudioSource drawerSound;

    void Start()
    {
        initialPosiiton = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == DoorState.OPENING)
        {
            if (transform.localPosition.x > initialPosiiton.x - distanceToOpen)
            {
                transform.localPosition -= Vector3.right * (speedOfOpening * Time.deltaTime);
            }
            else
            {
                state = DoorState.OPENED;
                transform.localPosition = new Vector3(initialPosiiton.x - distanceToOpen, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else if (state == DoorState.CLOSING)
        {
            if (transform.localPosition.x < initialPosiiton.x)
            {
                transform.localPosition += Vector3.right * (speedOfOpening * Time.deltaTime);
            }
            else
            {
                state = DoorState.CLOSED;
                transform.localPosition = new Vector3(initialPosiiton.x, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }

    public void Interact(PlayerController playerController)
    {
        if (state == DoorState.CLOSED)
        {
            state = DoorState.OPENING;
        }
        else if (state == DoorState.OPENED)
        {
            state = DoorState.CLOSING;
        }
        else if (state == DoorState.OPENING)
        {
            state = DoorState.CLOSING;
        }
        else if (state == DoorState.CLOSING)
        {
            state = DoorState.OPENING;
        }
        drawerSound.Play();
    }

    public void Highlight()
    {
    }

    public void Unhighlight()
    {
    }
}
