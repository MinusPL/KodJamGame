using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    List<GameObject> slots;
    public GameObject slotPrefab;
    public GameObject slotsAnchor;
    public int slotCount = 42;


    // Start is called before the first frame update
    void Start()
    {
        slots = new List<GameObject>();
        for(int i = 0; i < slotCount; i++)
		{
            AddSlot();
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddSlot()
	{
        var slot = Instantiate(slotPrefab);
        slot.transform.parent = slotsAnchor.transform;
        slots.Add(slot); 
	}

    public void AddItemInSlot(IItemStack item, int slotNumber)
	{
        //slots[slotNumber].
	}
}
