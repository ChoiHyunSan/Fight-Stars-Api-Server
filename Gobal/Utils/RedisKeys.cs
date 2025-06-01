namespace FightStars_ApiServer.Gobal.Utils
{
    public static class RedisKeys
    {
        // Shop
        private const string ShopPrefix = "shop";
        public static string Brawler(int id) => $"{ShopPrefix}:brawler:{id}";
        public static string Skin(int id) => $"{ShopPrefix}:skin:{id}";


    }

}
