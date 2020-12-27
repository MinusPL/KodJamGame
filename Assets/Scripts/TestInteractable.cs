using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerController playerController)
    {
        transform.Rotate(0, 0, 360 * Time.deltaTime);
    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
}
