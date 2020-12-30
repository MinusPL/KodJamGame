using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySymbol : Item
{
    private class KeySymbolItemStack : IItemStack
    {
        public uint ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public Texture ItemTexture {get;set;}
        public uint Amount { get; set; }
        public void UseItem(PlayerController player)
        {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        itemStack = new KeySymbolItemStack
        {
            ItemID = itemId,
            ItemName = itemName,
            ItemDescription = description,
            ItemTexture = itemTexture,
            Amount = amount
        };
        
    }
}