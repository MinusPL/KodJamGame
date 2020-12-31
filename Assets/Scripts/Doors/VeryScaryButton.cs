using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeryScaryButton : MonoBehaviour, IInteractable
{
    public GameObject objectToMove;

    public uint keyId;
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
        if (playerController.Inventory.Find(x => x.ItemID == keyId) == null)
        {
            return;
        }
        objectToMove.GetComponent<IInteractable>().Interact(playerController);
    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
}
