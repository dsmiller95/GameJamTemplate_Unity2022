using System.Linq;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using static PlayFabExtensions.PlayFabClientHelpers;

namespace PlayFabExtensions
{
    public static class PlayFabExtGeneral
    {
        public static async UniTask AttemptLogEventAsync(WriteClientPlayerEventRequest playerEvent)
        {
            if (!await WaitForPlayFabAvailable()) return;
            var result = await WritePlayerEventAsync(playerEvent);
            if (result.IsError)
            {
                Debug.LogError(result.Error.GenerateErrorReport());
            }
            else
            {
                Debug.Log($"PlayFab: logged event {playerEvent.EventName}");
            }
        }

        public static async UniTask WriteNameAsync(string playerName)
        {
            if (!await WaitForPlayFabAvailable()) return;
            playerName = playerName.PadRight(3, '_');
            if (playerName.Length > 42)
            {
                playerName = playerName[..42];
            }

            var request = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = playerName,
            };
            var result = await UpdateUserTitleDisplayNameAsync(request);
            if (result.IsError)
            {
                Debug.LogError(result.Error.GenerateErrorReport());
            }
            else
            {
                Debug.Log($"PlayFab: updated name to {playerName}");
            }
        }

        public static async UniTask<string> GetPlayfabUserId()
        {
            if (!await WaitForPlayFabAvailable()) return null;
            return PlayFabSettings.staticPlayer.PlayFabId;
        }
    }
}