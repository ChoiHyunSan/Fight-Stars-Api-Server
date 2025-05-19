/***************************
           Item
***************************/
// Description
// : Id - 아이템 고유 ID
// : Name - 아이템 이름
// : Type - 아이템 유형
// : Description - 설명 텍스트
// Author : ChoiHyunSan
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int goldPrice { get; set; }
    public int zemPrice { get; set; }
    public List<UserInventory> UserInventories { get; set; } = new();
}