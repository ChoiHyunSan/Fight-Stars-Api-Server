
/***************************

      ShopController

***************************/
// Description
// : 게임 재화를 이용한 구매에서 사용되는 API입니다.
//   
// Author : ChoiHyunSan

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/shop")]
[ApiController]
public class ShopController : ControllerBase
{
    IShopService _shopService;

    public ShopController(
        IShopService shopService  
        )
    {
        _shopService = shopService;
    }

    [HttpPost("buy/character")]
    [Authorize]
    public async Task<IActionResult> BuyCharacter([FromBody] BuyCharacterRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _shopService.BuyCharacter(userId, request));
    }

    [HttpPost("buy/skin")]
    [Authorize]
    public async Task<IActionResult> BuySkin([FromBody] BuySkinRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(await _shopService.BuySkin(userId, request));
    }
}