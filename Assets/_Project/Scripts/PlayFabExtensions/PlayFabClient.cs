using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFabExtensions
{
    public class PlayFabClient : MonoBehaviour
    {
        private bool returningPlayer = true;

#if !UNITY_EDITOR
        public void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "Takoyaki Volleyball Club";
            }

            var playerIdProvider = ProvidePlayerIdFactory.Instance;
            Guid? userId = playerIdProvider.GetPlayerId();
            if (!userId.HasValue)
            {
                returningPlayer = false;
                userId = playerIdProvider.GetPlayerIdOrGenerateNew();
            }

            if (!userId.HasValue)
            { // based on nullable annotations, this branch should never be hit. 
                Debug.LogError("user id must be instantiated");
                userId = Guid.NewGuid();
            }
        
            var request = new LoginWithCustomIDRequest
            {
                CustomTags = new Dictionary<string, string>
                {
                    { "Version", Application.version },
                    { "IsEditor", Application.isEditor.ToString() }
                },
                CustomId = userId.Value.ToString(),
                CreateAccount = true
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        
        

        private void OnApplicationQuit()
        {
            PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest()
                {
                    Body = new Dictionary<string, object>() {
                        { "Time Played", Time.realtimeSinceStartup },
                        { "Returning Player", returningPlayer },
                        { "Version", Application.version }
                    },
                    EventName = "game_quit"
                },
                result => Debug.Log("Success"),
                error => Debug.LogError(error.GenerateErrorReport()));
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("PlayFab connected");
            
            var postLoginEvent = new WriteClientPlayerEventRequest()
            {
                Body = new Dictionary<string, object>() {
                    { "Returning Player", returningPlayer },
                    { "Version", Application.version }
                },
                EventName = "game_start"
            };
            PlayFabClientExtensions.AttemptLogEventAsync(postLoginEvent).Forget();
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError("PlayFab could not connect:");
            Debug.LogError(error.GenerateErrorReport());
        }

        
#endif
    }
}