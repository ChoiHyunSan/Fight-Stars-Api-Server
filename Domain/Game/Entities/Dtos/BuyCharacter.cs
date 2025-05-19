
/***************************

     BuyCharacterRequest

***************************/
// Description : 캐릭터 구매요청 DTO
// Author : ChoiHyunSan
public class BuyCharacterRequest
{
    public int CharacterId { get; set; }
    public int Price { get; set; }
    public int CurrentGold {get; set;}
}

/***************************

    BuyCharacterResponse

***************************/
// Description : 캐릭터 구매요청 응답 DTO
// Author : ChoiHyunSan
public class BuyCharacterResponse
{
    public bool Success { get; set; }
    public int ResultGold { get; set; }

    public UserBrawlerDto Brawler { get; set; }
}

/***************************

       BuySkinRequest

***************************/
// Description : 캐릭터 구매요청 응답 DTO
// Author : ChoiHyunSan
public class BuySkinRequest
{
    public int SkinId { get; set; }
    public int Price { get; set; }
    public int CurrentGem { get; set; }
}

/***************************

       BuySkinResponse

***************************/
// Description : 캐릭터 구매요청 응답 DTO
// Author : ChoiHyunSan
public class BuySkinResponse
{
    public bool Success { get; set; }
    public int ResultGem { get; set; }
}