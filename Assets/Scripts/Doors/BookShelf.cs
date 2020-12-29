using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour, IInteractable
{
    public float distanceToOpen = 20f;
    public float speedOfOpening = 20f;
    public GameObject bookCase;
    
    
    public DoorState state = DoorState.CLOSED;
    private Vector3 initialBookShelfPosition;
    private Vector3 initialBookPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialBookPosition = transform.position;
        initialBookShelfPosition = bookCase.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == DoorState.OPENING)
        {
            if (Vector3.Distance(bookCase.transform.localPosition, initialBookShelfPosition) < distanceToOpen)
            {
                bookCase.transform.localPosition -= bookCase.transform.TransformDirection(Vector3.forward) * (speedOfOpening * Time.deltaTime);
            }
            else
            {
                state = DoorState.OPENED;
                bookCase.transform.localPosition = initialBookShelfPosition -
                                                   bookCase.transform.TransformDirection(Vector3.forward) *
                                                   distanceToOpen;
            }
        }
        else if (state == DoorState.CLOSING)
        {
            if ((bookCase.transform.position - initialBookShelfPosition).normalized != bookCase.transform.TransformDirection(Vector3.forward))
            {
                bookCase.transform.localPosition += bookCase.transform.TransformDirection(Vector3.forward) * (speedOfOpening * Time.deltaTime);
            }
            else
            {
                state = DoorState.CLOSED;
                bookCase.transform.localPosition = initialBookShelfPosition;
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
