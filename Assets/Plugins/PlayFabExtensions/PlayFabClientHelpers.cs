using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFabExtensions
{
    internal static class PlayFabClientHelpers {

        public static async Task<bool> WaitForPlayFabAvailable()
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
            { 
                await UniTask.WhenAny(
                    UniTask.Delay(5000),
                    UniTask.WaitUntil(PlayFabClientAPI.IsClientLoggedIn)
                );
                if (!PlayFabClientAPI.IsClientLoggedIn())
                {
                    Debug.LogError("Failed to log event because the client is not logged in");
                    return false;
                }
            }

            return true;
        }
        

        public static async UniTask<PlayFabResult<TRes>> GetAsync<TReq, TRes>(
            Action<TReq, Action<TRes>, Action<PlayFabError>, object, Dictionary<string, string>> apiCall,
            TReq request)
        {
            var tcs = new UniTaskCompletionSource<PlayFabResult<TRes>>();
            await UniTask.SwitchToMainThread();
            apiCall(request, 
                result => tcs.TrySetResult(result),
                error => tcs.TrySetResult(error),
                null,
                null);
            return await tcs.Task;
        }
        
        public static UniTask<PlayFabResult<WriteEventResponse>> WritePlayerEventAsync(
            WriteClientPlayerEventRequest request)
        {
            return GetAsync<WriteClientPlayerEventRequest, WriteEventResponse>(
                PlayFabClientAPI.WritePlayerEvent, request);
        }

        public static UniTask<PlayFabResult<GetLeaderboardResult>> GetLeaderboardAsync(GetLeaderboardRequest request)
        {
            return GetAsync<GetLeaderboardRequest, GetLeaderboardResult>(
                PlayFabClientAPI.GetLeaderboard, request);
        }
        
        public static UniTask<PlayFabResult<UpdatePlayerStatisticsResult>> UpdatePlayerStatisticsAsync(UpdatePlayerStatisticsRequest request)
        {
            return GetAsync<UpdatePlayerStatisticsRequest, UpdatePlayerStatisticsResult>(
                PlayFabClientAPI.UpdatePlayerStatistics, request);
        }
        
        public static UniTask<PlayFabResult<UpdateUserTitleDisplayNameResult>> UpdateUserTitleDisplayNameAsync(UpdateUserTitleDisplayNameRequest request)
        {
            return GetAsync<UpdateUserTitleDisplayNameRequest, UpdateUserTitleDisplayNameResult>(
                PlayFabClientAPI.UpdateUserTitleDisplayName, request);
        }
    }
}