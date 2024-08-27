using System.Collections.Generic;

namespace PlayFabExtensions
{
    public interface IPlayfabExtraDataProvider
    {
        public void AddExtraForApplicationQuit(Dictionary<string, object> body);
    }
}