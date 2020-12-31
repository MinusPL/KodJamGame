using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour, IInteractable
{
    public float distanceToOpen = 20f;
    public float speedOfOpening = 20f;
    public float bookOpenDistance = 0.1f;
    public GameObject bookCase ;
    public AudioSource source;
    
    
    public DoorState state = DoorState.CLOSED;
    private Vector3 initialBookShelfPosition;
    private Vector3 initialBookPosition;

    public Material HighlightMaterial;

    private Material[] initialMaterials;
    // Start is called before the first frame update
    void Start()
    {
        initialMaterials = gameObject.GetComponent<Renderer>().materials;
        initialBookPosition = transform.localPosition;
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
            transform.localPosition += Vector3.left * bookOpenDistance;
        }
        else if (state == DoorState.OPENED)
        {
            state = DoorState.CLOSING;
            transform.localPosition -= Vector3.left * bookOpenDistance;
        }
        source.Play();
        Unhighlight();
    }

    public void Highlight()
    {
        Debug.Log("HIGHLIGHT!!!");
        Material[] currentMaterials = gameObject.GetComponent<MeshRenderer>().materials;
        Debug.Log(currentMaterials.Length);
        for (int i = 0; i < currentMaterials.Length; i++)
        {
            Debug.Log("Meterial");
            Debug.Log(currentMaterials[i]);
            currentMaterials[i] = HighlightMaterial;
            Debug.Log("Po");
            Debug.Log(currentMaterials[i]);
        }

        gameObject.GetComponent<MeshRenderer>().materials = currentMaterials;
    }

    public void Unhighlight()
    {
        Debug.Log("UNHIGHLIGHT");
        gameObject.GetComponent<Renderer>().materials = initialMaterials;
    }
}
