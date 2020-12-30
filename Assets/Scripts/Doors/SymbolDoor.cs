using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SymbolDoor : MonoBehaviour, IInteractable
{

    public uint keyId;
    public void Interact(PlayerController playerController)
    {
        if (playerController.Inventory.Find(x => x.ItemID == keyId) == null)
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
    }
}
