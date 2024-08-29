using System;
using PlayFab;

namespace PlayFabExtensions
{
    internal struct PlayFabResult<TSucc>
    {
        private TSucc _success;
        private PlayFabError _error;
        private bool _isSuccess;
        public bool IsSuccess => _isSuccess;
        public bool IsError => !_isSuccess;
        public TSucc Success => (IsSuccess) ? _success : throw new InvalidOperationException("No success value");
        public PlayFabError Error => IsError ? _error : throw new  InvalidOperationException("No error value");
            
        private PlayFabResult(bool isSucc, TSucc success, PlayFabError err)
        {
            this._isSuccess = isSucc;
            this._success = success;
            this._error = err;
        }
            
        public static PlayFabResult<TSucc> CreateSuccess(TSucc success)
        {
            return new PlayFabResult<TSucc>(true, success, default);
        }
            
        public static PlayFabResult<TSucc> CreateError(PlayFabError error)
        {
            return new PlayFabResult<TSucc>(false, default, error);
        }
            
        //implicit casts
            
        public static implicit operator PlayFabResult<TSucc>(TSucc success)
        {
            return CreateSuccess(success);
        }
            
        public static implicit operator PlayFabResult<TSucc>(PlayFabError error)
        {
            return CreateError(error);
        }
    }
}