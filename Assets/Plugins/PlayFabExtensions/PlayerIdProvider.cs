using System;
using UnityEngine;

namespace PlayFabExtensions
{
    public interface IProvidePlayerId
    {
        public Guid? GetPlayerId();
        public Guid GetPlayerIdOrGenerateNew();
    }

    public static class ProvidePlayerIdFactory
    {
        public static readonly IProvidePlayerId Instance = new PlayerPrefPlayerIdProvider();
    }

    internal class PlayerPrefPlayerIdProvider : IProvidePlayerId
    {
        public Guid? GetPlayerId()
        {
            string customID = PlayerPrefs.GetString("customID", "newUser");
            if (customID != "newUser") return Guid.Parse(customID);
            return null;

        }

        public Guid GetPlayerIdOrGenerateNew()
        {
            string customID = PlayerPrefs.GetString("customID", "newUser");
            if (customID != "newUser") return Guid.Parse(customID);
            customID = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("customID", customID);
            return Guid.Parse(customID);
        }
    }
}