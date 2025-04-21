
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

/***************************

     GameDataController

***************************/
// Description
// : 게임 데이터에 관련된 API를 처리하는 컨트롤러입니다.
//   게임 데이터는 게임의 상태, 플레이어 정보, 게임 설정 등을 포함합니다.
//
// Author : ChoiHyunSan

[Route("api/data")]
[ApiController]
public class GameDataController : ControllerBase
{
    private readonly IUserLoadDataService _userLoadDataService;
    private readonly IJwtService _jwtService;

    public GameDataController(
        IUserLoadDataService userLoadDataService,
        IJwtService jwtService)
    {
        _userLoadDataService = userLoadDataService;
        _jwtService = jwtService;
    }

    [HttpGet("load")]
    [Authorize]
    public async Task<IActionResult> GetUserData()
    {
        var userId = _jwtService.GetUserIdFromClaims(User);
        var userData = await _userLoadDataService.LoadUserDataAsync(userId);
        return Ok(userData);
    }


}