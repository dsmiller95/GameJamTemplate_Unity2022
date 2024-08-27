using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dman.Leaderboard;
using PlayFabExtensions;
using UnityEngine;

namespace PlayfabLeaderboard
{
    public class PlayfabLeaderboardRepository : ILeaderboardRepository
    {
        private int _currentWrites = 0;

        public UniTask WaitForPendingWriters()
        {
            return UniTask.WaitUntil(() => _currentWrites == 0);
        }

        public TimeSpan WaitAfterWritesToRead => TimeSpan.FromSeconds(1);

        public async UniTask WriteScore(LeaderboardDefinition leaderboard, int score, CancellationToken cancel)
        {
            LeaderboardValidGuard(leaderboard);
            _currentWrites++;
            try
            {
                var realScore = score * (leaderboard.higherIsBetter ? 1 : -1);
                await PlayFabExtLeaderboard.WriteScoreAsync(leaderboard.leaderboardName, realScore);
            }
            finally
            {
                _currentWrites--;
            }
        }

        public async UniTask WritePlayerName(string name, CancellationToken cancel)
        {
            NameValidGuard(name);
            _currentWrites++;
            try
            {
                await PlayFabExtGeneral.WriteNameAsync(name);
            }
            finally
            {
                _currentWrites--;
            }
        }

        public async UniTask<LeaderboardEntry[]> GetLeaderboard(LeaderboardDefinition leaderboard, int maxResults, CancellationToken cancel)
        {
            LeaderboardValidGuard(leaderboard);
            Debug.Assert(maxResults > 0, "maxResults must be greater than 0");
            
            var result = await PlayFabExtLeaderboard.GetScoresAsync(leaderboard.leaderboardName, maxResults);
            var playerId = await PlayFabExtGeneral.GetPlayfabUserId();
            var multiplier = leaderboard.higherIsBetter ? 1 : -1;
            var entries = result.Select(x => new LeaderboardEntry
            {
                name = x.DisplayName,
                isCurrentUser = x.PlayFabId == playerId,
                score = x.StatValue * multiplier,
                rank = x.Position,
            }).ToArray();
            
            return entries;
        }

        private void LeaderboardValidGuard(LeaderboardDefinition leaderboardDefinition)
        {
            if (string.IsNullOrWhiteSpace(leaderboardDefinition.leaderboardName))
            {
                throw new Exception(
                    $"leaderboard definition must not be null or whitespace, got '{leaderboardDefinition.leaderboardName}'");
            }
        }

        private void NameValidGuard(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception(
                    $"name must not be null or whitespace, got '{name}'");
            }
        }
        
    }
}