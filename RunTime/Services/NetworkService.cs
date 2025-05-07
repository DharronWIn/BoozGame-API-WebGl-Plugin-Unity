using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Best.HTTP;
using Best.HTTP.Request.Authenticators;
using Best.HTTP.Request.Upload;
/*using Core.Services.Socket;
using Core.Services.Storage;*/
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoozGameAPIProvider
{

    public static class NetworkService 
    {
        private const string HaveNewToken = "havenewtoken";
        private static bool _inRefreshProcess;
        private static bool _badSession;
        private static string cachedVersion;

        public static async Task<HTTPResponse> PostRequestAsync(string endpoint, Dictionary<string, object> body,
            Dictionary<string, string> otherHeader = null, bool ignoreNullResponse = false)
        {
            var request = HTTPRequest.CreatePost(AppConfigs.HttpHost() + endpoint);

        try
        {       
                if (otherHeader != null)
                    foreach (var other in otherHeader)
                        request.SetHeader(other.Key, other.Value);
                AddTokenAndHeader(request);
                if (body != null)
                    request.UploadSettings.UploadStream = new JSonDataStream<Dictionary<string, object>>(body);
                return await AsyncRequestFinishedCallback(request, await request.GetHTTPResponseAsync(),
                    ignoreNullResponse);

            }
            catch (AsyncHTTPException e)
            {
                Debug.LogError($"Exception occurred during POST request: {e}");
                if (e.Message.ToLower() == HaveNewToken.ToLower())
                    return await PostRequestAsync(endpoint, body, otherHeader, ignoreNullResponse);
                NetworkBusyHandler(e);
                throw;
            }
            finally
            {
                // Hide loader once the request completes (whether success or failure)
            }
        }
        

        public static async Task<HTTPResponse> GetRequestAsync(string endpoint, Dictionary<string, object> body = null,
            Dictionary<string, string> otherHeader = null)
        {
            var request = HTTPRequest.CreateGet(AppConfigs.HttpHost() + endpoint);
            try
            {
                if (otherHeader != null)
                    foreach (var other in otherHeader)
                        request.SetHeader(other.Key, other.Value);
                AddTokenAndHeader(request);
                if (body != null)
                    request.UploadSettings.UploadStream = new JSonDataStream<Dictionary<string, object>>(body);
                return await AsyncRequestFinishedCallback(request, await request.GetHTTPResponseAsync());
            }
            catch (AsyncHTTPException e)
            {
                Debug.LogError($"Exception occurred during GET request: {e}");
                if (e.Message.ToLower() == HaveNewToken.ToLower())
                    return await GetRequestAsync(endpoint, body, otherHeader);
                NetworkBusyHandler(e);
                throw;
            }
            finally
            {
                // Hide loader once the request completes (whether success or failure)
            }
        }

        public static async Task<HTTPResponse> PutRequestAsync(string endpoint, Dictionary<string, object> body,
            Dictionary<string, string> otherHeader = null)
        {
            var request = HTTPRequest.CreatePut(AppConfigs.HttpHost() + endpoint);
            try
            {
                if (otherHeader != null)
                    foreach (var other in otherHeader)
                        request.SetHeader(other.Key, other.Value);
                AddTokenAndHeader(request);
                if (body != null)
                    request.UploadSettings.UploadStream = new JSonDataStream<Dictionary<string, object>>(body);
                return await AsyncRequestFinishedCallback(request, await request.GetHTTPResponseAsync());
            }
            catch (AsyncHTTPException e)
            {
                Debug.LogError($"Exception occurred during PUT request: {e}");
                if (e.Message.ToLower() == HaveNewToken.ToLower())
                    return await PutRequestAsync(endpoint, body, otherHeader);
                NetworkBusyHandler(e);
                throw;
            }
            finally
            {
                // Hide loader once the request completes (whether success or failure)
            }
        }

    private static async Task<HTTPResponse> AsyncRequestFinishedCallback(HTTPRequest req, HTTPResponse resp, bool ignoreNullResponse = false)
        {
            Debug.Log($"response : {resp.DataAsText} | StatusCode : {resp.StatusCode} | IsSuccess : {resp.IsSuccess}");
            if (resp.IsSuccess)
            {
                Debug.Log("IsSuccess");
                if (ResponseIsOk(resp, ignoreNullResponse))
                    return resp;
                throw new AsyncHTTPException(resp.StatusCode, ResponseMessage(resp), resp.DataAsText);
            }

            // On Access Token Expired
            if (resp.StatusCode == 401)
                if (await AsyncRefreshTokenHandler())
                    throw new AsyncHTTPException(resp.StatusCode, HaveNewToken, resp.DataAsText);
            if (resp.StatusCode == 410)
            {
                // Update Interceptor code
                return null;
            }
               
            if (resp.StatusCode == 503)
            {
                //SceneManager.LoadSceneAsync("Maintenance");
                return null;
            }

            throw new AsyncHTTPException(resp.StatusCode, $"Request finished with error! Request state: {req.State}",resp.DataAsText);
            
        }

        private static async Task<bool> AsyncRefreshTokenHandler()
        {
            if (_inRefreshProcess == false)
            {
                _inRefreshProcess = true;
                try
                {
                    Debug.Log("Requesting Refresh Token");
                    var request = HTTPRequest.CreatePut(AppConfigs.HttpHost());
                    AddTokenAndHeader(request);
                    var respX = await request.GetHTTPResponseAsync();
                    Debug.Log(respX.DataAsText + " " + respX.IsSuccess);
                if (respX.IsSuccess)
                    {
                        if (JsonConvert.DeserializeObject<Dictionary<string, object>>(respX.DataAsText)[
                                "success"].ToString().ToLower() == "true")
                        {
                            //var refreshToken = RefreshToken.FromJson(respX.DataAsText);
                            //PlayerPrefs.SetString(AppConfigs.AccessToken, refreshToken.data.accessToken);
                            //Debug.Log("Refresh Token Success" + refreshToken.ToString());
                        }
                        else
                        {
                            //Call logOut process here
                            _badSession = true;

                            PlayerPrefs.DeleteAll();

                        }
                    }
                    else
                    {
                        _badSession = true;
                        Debug.Log("Token Expired");
                        throw new Exception(respX.ToString());
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception occurred during GET new Access Token: {e}");
                    throw;
                }
                finally
                {
                    _inRefreshProcess = false;
                }
            }
            else
            {
                await _checker();
            }

        if (_badSession)
            Debug.Log("Token Expired");
        return true;
        }

        private static void AddTokenAndHeader(HTTPRequest request)
        {
            var token = ClientManager.Instance.token;
            Debug.Log($"User Token : " + token);
            if (token != "") request.Authenticator = new BearerTokenAuthenticator(token);
            //request.SetHeader("platform", AppConfigs.GetPlatform());
            //request.SetHeader("version", cachedVersion);
            //request.SetHeader("version", "1.7.4");
            //request.SetHeader("Content-Type", "application/json");
        }

        private static void AddSpendingKey(HTTPRequest request)
        {
            request.SetHeader("X-API-KEY", AppConfigs.XApiKey());
        }

        private static async Task<Task> _checker()
        {
            if (_inRefreshProcess == false) return null;
            await Task.Delay(10);
            if (_inRefreshProcess == false) return null;
            return await _checker();
        }

        private static bool ResponseIsOk(HTTPResponse response, bool ignoreNullResponse = false)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.DataAsText);
            if (ignoreNullResponse && string.IsNullOrEmpty(response.DataAsText)) return true;
            return data["etat"].ToString().ToLower() == "true";
        }

        private static string ResponseMessage(HTTPResponse response)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.DataAsText);
            //return string.IsNullOrEmpty( data["message"].ToString()) ? "" : data["message"].ToString();
            return "Response Message Request";
        }

        private static void NetworkBusyHandler(AsyncHTTPException e)
        {
            if (e.StatusCode < 1){
                //PopUpManager.Instance.ShowPopup("Un probleme semble venir de notre coté ressayer plus tard", PopUpManager.PopUpMessageType.Error);
                //LoaderManager.Instance.HideLoader();
                throw new Exception("No connexion");
            } 
        }
    }
}

