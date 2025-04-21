
/***************************
          Brawler
***************************/
// Description
// : Id - 브롤러 고유 ID
// : Name - 브롤러 이름
// : Description - 브롤러 설명
// Author : ChoiHyunSan

public class Brawler
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<UserBrawler> UserBrawlers { get; set; } = new();
}