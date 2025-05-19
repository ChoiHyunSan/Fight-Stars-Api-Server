/***************************
           UserSkin
***************************/
// Description
// : GameUserId - 유저 ID (FK)
// : ItemId - 아이템 ID (FK)
// : Quantity - 해당 아이템 보유 수량
// Author : ChoiHyunSan

using System.ComponentModel.DataAnnotations;

public class UserSkin
{
    public long GameUserId { get; set; }
    public int SkinId { get; set; }
    public GameUser? GameUser { get; set; }
    public Skin? Skin { get; set; }

    public static UserSkin Create(GameUser gameUser, Skin skin)
    {
        return new UserSkin()
        {
            GameUserId = gameUser.Id,
            GameUser = gameUser,
            Skin = skin,
            SkinId = skin.Id
        };
    }
}