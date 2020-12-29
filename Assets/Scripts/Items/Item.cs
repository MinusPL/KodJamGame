using UnityEngine;

public class Item : MonoBehaviour, ICollectable
{
    protected IItemStack itemStack;

    public uint itemId;
    public string itemName;
    public string description;
    public uint amount = 1;

    public void Collect(PlayerController playerController)
    {
        playerController.AddItem(itemStack);
        Destroy(gameObject);
    }

    public void Highlight()
    {
    }

    public void Unhighlight()
    {
    }
}