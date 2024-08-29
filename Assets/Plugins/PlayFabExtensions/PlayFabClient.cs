using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFabExtensions
{
    public class PlayFabClient : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = false;
        
        [Tooltip("when true, will use the production playfab environment even in the editor")]
        [SerializeField] private bool forceUseProdSettings;
        [SerializeField] private PlayFabSharedSettings developmentPlayfabSettings;
        
        private bool returningPlayer = true;

        private void OnEnable()
        {
            if(dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
#if UNITY_EDITOR
            if (!forceUseProdSettings)
            {
                ForceSetPlayfabSharedSettings(developmentPlayfabSettings);
            }
#endif
            
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
        
        private void ForceSetPlayfabSharedSettings(PlayFabSharedSettings newSettings)
        {
            var property = typeof(PlayFabSettings).GetField("_playFabShared", BindingFlags.Static | BindingFlags.NonPublic);
            if (property == null)
            {
                throw new Exception("Could not find or set PlayFabSharedSettings");
            }
            property.SetValue(null, newSettings);
        }

        private void OnApplicationQuit()
        {
            var body = new Dictionary<string, object>()
            {
                { "Time Played", Time.realtimeSinceStartup },
                { "Returning Player", returningPlayer },
                { "Version", Application.version }
            };

            var statProviders = this.GetComponents<IPlayfabExtraDataProvider>();
            foreach (IPlayfabExtraDataProvider statProvider in statProviders)
            {
                statProvider.AddExtraForApplicationQuit(body);
            }
            PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest()
                {
                    Body = body,
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
            PlayFabExtGeneral.AttemptLogEventAsync(postLoginEvent).Forget();
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError("PlayFab could not connect:");
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}