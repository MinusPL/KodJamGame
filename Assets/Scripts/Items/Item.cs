using System;
using UnityEngine;

public 
    class Item : MonoBehaviour, ICollectable
{
    public IItemStack itemStack;

    public uint itemId;
    public string itemName;
    public string description;
    public uint amount = 1;
    public Texture itemTexture;
    
    public Material HighlightMaterial;

    private Material[] initialMaterials;
    private bool collectable = true;

    protected void Start()
    {
        Debug.Log("START!!!");
        initialMaterials = gameObject.GetComponent<Renderer>().materials;
    }

    public void Collect(PlayerController playerController)
    {
        playerController.AddItem(itemStack);
        playerController.inventoryController.AddItem(itemStack);
        collectable = false;
        Destroy(gameObject);
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
        if(collectable)
            gameObject.GetComponent<Renderer>().materials = initialMaterials;
    }
}