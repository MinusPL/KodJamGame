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
    public float openingSpeed = 90.0f;
    public bool locked = false;
    public uint keyId;
    
    private DoorState state = DoorState.CLOSED;
    private Vector3 initialPosiiton;
    private Quaternion initialRotation;

    public AudioSource doorSound;

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
                    transform.RotateAround(origin.transform.position, transform.TransformDirection(Vector3.forward), openingSpeed * Time.deltaTime);
                else
                    state = DoorState.OPENED;
            }
            else if (doorDirection == DoorDirection.LEFT)
            {
                if (transform.localRotation.eulerAngles.y < 90)
                    transform.RotateAround(origin.transform.position, transform.TransformDirection(Vector3.forward), openingSpeed * Time.deltaTime);
                else
                    state = DoorState.OPENED;
            }
        }
        else if (state == DoorState.CLOSING)
        {
            Debug.Log(transform.localRotation.eulerAngles);
            if (doorDirection == DoorDirection.RIGHT)
            {
                if (transform.localRotation.eulerAngles.y < 180)
                    transform.RotateAround(origin.transform.position, transform.TransformDirection(Vector3.forward), -openingSpeed * Time.deltaTime);
                else
                {
                    state = DoorState.CLOSED;
                    transform.localRotation = initialRotation;
                    transform.localPosition = initialPosiiton;
                }
            }
            else if (doorDirection == DoorDirection.LEFT)
            {
                if (transform.localRotation.eulerAngles.y > 0 && transform.localRotation.eulerAngles.y < 180)
                    transform.RotateAround(origin.transform.position, transform.TransformDirection(Vector3.forward), -openingSpeed * Time.deltaTime);
                else
                {
                    state = DoorState.CLOSED;
                    transform.localRotation = initialRotation;
                    transform.localPosition = initialPosiiton;
                }
            }
        }
    }

    public void Interact(PlayerController playerController)
    {
        if (locked)
        {
            if (playerController.Inventory.Find(x => x.ItemID == keyId) == null)
            {
                return;
            }
        }
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
        doorSound.Play();
    }

    public void Highlight()
    {
    }

    public void Unhighlight()
    {
    }
}
