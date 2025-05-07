using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
namespace BoozGameAPIProvider
{
    public static class Provider 
    {
        public static async Task CheckUserAuthorizedRequest(string gameId)
        {
            try
            {
                var res = await NetworkService.PostRequestAsync(EndPoints.Game, new Dictionary<string, object> {
                    {"gameId", gameId }
                });

                var accessGameResponse = AccessGameResponse.FromJson(res.DataAsText);
                Debug.Log(accessGameResponse.result);
                ClientManager.Instance.clientData = accessGameResponse.result;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                throw;
            }
    
       }

        public static async Task SaveGameProgressRequest(GameDataDto gameDataDto)
        {
            var res = await NetworkService.PutRequestAsync(EndPoints.Game, new Dictionary<string, object> {
                {"gameId", gameDataDto.gameId },
                {"score", gameDataDto.score },
                {"gameData", gameDataDto.gameProgress },
            });
        }
    }
}
