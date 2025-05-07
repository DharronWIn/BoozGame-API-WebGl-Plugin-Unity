using BoozGameAPIProvider;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ClientManager : MonoBehaviour
{

    public string token;
    public string gameId;
    static string url;
    static Dictionary<string, string> queryParams = new();

    [HideInInspector] public UnityEvent OnUserAuthorized;
    [HideInInspector] public UnityEvent OnUserUnauthorized;

    public Authorization authorization;

    public ResultDto clientData;

    public static ClientManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
      CheckUserAuthorized();
    }

    private async void OnEnable()
    {
      
    }

    //?q=sdfsdfsdf&gameId=qskdfnksqdf
    public async void CheckUserAuthorized()
    {
        url = Application.absoluteURL;

        if (string.IsNullOrEmpty(url))
        {
            queryParams = UrlParamsReader.GetQueryParameters(url);
            Debug.Log("URL complète : " + url);
            if (UrlContainsKey(AppConfigs.TokenKey) && UrlContainsKey(AppConfigs.GameIdKey))
            {
                token = GetParams(AppConfigs.TokenKey);
                gameId = GetParams(AppConfigs.GameIdKey);

                try
                {
                    if (gameId == AppConfigs.GameId)
                    {
                        await Provider.CheckUserAuthorizedRequest(gameId);
                    }
                    else
                    {
                        authorization.Unauthorize("L'id du jeu ne correspond pas !");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("CheckUserAuthorized Failed " + e.Message + "/" + token + " gameId : " + gameId);
                    OnUserUnauthorized?.Invoke();
                    authorization.Unauthorize();
                    throw e;
                }


                OnUserAuthorized?.Invoke();
                authorization.Authorize();

                Debug.Log("User Autorisé a jouer a ce jeu token : " + token + " gameId : " + gameId);
            }
            else
            {
                OnUserUnauthorized?.Invoke();
                authorization.Unauthorize();

            }
        }
        else
        {
            OnUserUnauthorized?.Invoke();
            authorization.Unauthorize();
        }
    }

/*    public async void SaveGameData()
    {

    }*/

    private void Update()
    {
    }
    public static bool UrlContainsKey(string key)
    {
        return queryParams.ContainsKey(key);
    }

    static string GetParams(string key)
    {
        return queryParams[key];
    }
}
