
/***************************
            Skin
***************************/
// Description
// : Id - 스킨 고유 ID
// : BrawlerId : 관련 브롤러 ID
// : Name - 스킨
// : Description - 스킨 설명
// Author : ChoiHyunSan
public class Skin
{
    public int Id { get; set; }

    public int BrawlerId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int zemPrice { get; set; }

    public List<UserSkin> UserSkins { get; set; } = new();
}