/***************************
      UserInventory
***************************/
// Description
// : GameUserId - 유저 ID (FK)
// : ItemId - 아이템 ID (FK)
// : Quantity - 해당 아이템 보유 수량
// Author : ChoiHyunSan
public class UserInventory
{
    public long GameUserId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; } = 1;

    public GameUser? GameUser { get; set; }
    public Item? Item { get; set; }
}