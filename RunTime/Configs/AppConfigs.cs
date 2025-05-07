
namespace BoozGameAPIProvider
{


    public static class AppConfigs
    {
        public static string AccessToken => "access_token";
        public static string VersionKey => "v1";
        public static string TokenKey = "q";
        public static string GameIdKey = "gameId";
        public static string GameId = "27a1f0bb-3343-4523-af3c-cdd59b44fa79";
        public static string BaseUrl => "https://api.boozgame.com/";
        public static string HttpHost()
        {
            return BaseUrl + "api/"+VersionKey+"/";
        }
        public static string XApiKey()
        {
            return "";
        }
    }


}