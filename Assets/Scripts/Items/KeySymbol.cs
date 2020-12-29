using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySymbol : Item
{
    private class KeySymbolItemStack: IItemStack
    {
        public uint ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public uint Amount { get; set; }
        public void UseItem(PlayerController player)
        {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        itemStack = new KeySymbolItemStack
        {
            ItemID = itemId,
            ItemName = itemName,
            ItemDescription = description,
            Amount = amount
        };
        
    }
}
