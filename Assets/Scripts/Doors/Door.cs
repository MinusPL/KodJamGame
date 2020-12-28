using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public enum DoorDirection
    {
        LEFT,
        RIGHT
    }

    public DoorDirection doorDirection;
    public GameObject origin;
    
    private DoorState state = DoorState.CLOSED;
    private Vector3 initialPosiiton;
    private Quaternion initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        initialPosiiton = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == DoorState.OPENING)
        {
            if (doorDirection == DoorDirection.RIGHT)
            {
                if (transform.localRotation.eulerAngles.y > 90)
                    transform.RotateAround(origin.transform.position, Vector3.up, -20 * Time.deltaTime);
                else
                    state = DoorState.OPENED;
            }
            else if (doorDirection == DoorDirection.LEFT)
            {
                if (transform.localRotation.eulerAngles.y < 90)
                    transform.RotateAround(origin.transform.position, Vector3.up, 20 * Time.deltaTime);
                else
                    state = DoorState.OPENED;
            }
        }
        else if (state == DoorState.CLOSING)
        {
            if (doorDirection == DoorDirection.RIGHT)
            {
                if (transform.localRotation.eulerAngles.y < 180)
                    transform.RotateAround(origin.transform.position, Vector3.up, 20 * Time.deltaTime);
                else
                    state = DoorState.CLOSED;
            }
            else if (doorDirection == DoorDirection.LEFT)
            {
                if (transform.localRotation.eulerAngles.y > 0 && transform.localRotation.eulerAngles.y < 180)
                    transform.RotateAround(origin.transform.position, Vector3.up, -20 * Time.deltaTime);
                else
                    state = DoorState.CLOSED;
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
    }

    public void Highlight()
    {
    }

    public void Unhighlight()
    {
    }
}
