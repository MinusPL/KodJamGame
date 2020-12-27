using System.Collections.Generic;

public interface IInventory
{
    List<IItemStack> Inventory { get; }
    void AddItem(IItemStack item);
    IItemStack GetItem(int index);
    int ItemsCount();
    void RemoveItem(int index);
    void DecreaseAmount(int index, uint amount);
}