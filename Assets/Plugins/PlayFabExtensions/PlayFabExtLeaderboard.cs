using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab.ClientModels;
using UnityEngine;
using static PlayFabExtensions.PlayFabClientHelpers;

namespace PlayFabExtensions
{
    public class PlayFabExtLeaderboard
    {
        /// <summary>
        /// Always sorted in descending order - I.E. higher scores always at top.
        /// to sort in ascending order, multiply the score by -1 before writing, and multiply the score by -1 after reading.
        /// </summary>
        /// <param name="statisticName"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public static async UniTask<IList<PlayerLeaderboardEntry>> GetScoresAsync(string statisticName, int maxResults)
        {
            if (!await WaitForPlayFabAvailable()) return ArraySegment<PlayerLeaderboardEntry>.Empty;
            var request = new GetLeaderboardRequest()
            {
                StatisticName = statisticName,
                MaxResultsCount = maxResults,
            };
            var result = await GetLeaderboardAsync(request);
            if (result.IsError)
            {
                Debug.LogError(result.Error.GenerateErrorReport());
                return ArraySegment<PlayerLeaderboardEntry>.Empty;
            }
            
            Debug.Log($"PlayFab: got leaderboard {statisticName}, {result.Success.Leaderboard.Count} entries");
            
            return result.Success.Leaderboard;
        }

        /// <summary>
        /// Always sorted in descending order - I.E. higher scores always at top.
        /// to sort in ascending order, multiply the score by -1 before writing, and multiply the score by -1 after reading.
        /// </summary>
        /// <param name="statisticName"></param>
        /// <param name="newScore"></param>
        public static async UniTask WriteScoreAsync(string statisticName, int newScore)
        {
            if (!await WaitForPlayFabAvailable()) return;
            var request = new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>()
                {
                    new()
                    {
                        StatisticName = statisticName,
                        Value = newScore,
                    }
                }
            };
            var result = await UpdatePlayerStatisticsAsync(request);
            if (result.IsError)
            {
                Debug.LogError(result.Error.GenerateErrorReport());
            }
            else
            {
                Debug.Log($"PlayFab: updated score {statisticName} to {newScore}");
            }
        }

    }
}