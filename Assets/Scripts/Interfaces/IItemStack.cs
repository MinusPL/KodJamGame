public interface IItemStack
{
    uint ItemID { get; set; }
    string ItemName { get; set; }
    string ItemDescription { get; set; }
    uint Amount { get; set; }
    void UseItem(PlayerController player);
}
