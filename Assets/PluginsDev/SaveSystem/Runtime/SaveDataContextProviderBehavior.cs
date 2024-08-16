using System.Collections.Generic;
using System.IO;
using Dman.Utilities;
using Dman.Utilities.Logger;
using UnityEngine;

namespace SaveSystem
{
    [UnitySingleton]
    [RequireComponent(typeof(EarlyAwakeHook), typeof(LateDestroyHook))]
    public class SaveDataContextProviderBehavior : MonoBehaviour, 
        ISaveDataContextProvider,
        ISaveDataPersistence,
        IPersistSaveData,
        IAwakeEarly,
        IDestroyLate
    {
        [SerializeField] private string rootFolderPath;

        [Tooltip("these are loaded on awake, and persisted on destroy")]
        [SerializeField] private string[] contextsToManage;
        
        private SaveDataContextProvider _provider;
        
        public void AwakeEarly()
        {
            _provider = SaveDataContextProvider.CreateAndPersistTo(this);
            foreach (string context in contextsToManage)
            {
                _provider.LoadContext(context);
            }
        }
        private void Awake()
        {
            Debug.Assert(_provider != null, "SaveDataContextProviderBehavior.Awake: _provider != null");
        }
        private void OnDestroy()
        {
            Debug.Assert(_provider != null, "SaveDataContextProviderBehavior.OnDestroy: _provider != null");
            Debug.Assert(_provider.IsDisposed == false, "SaveDataContextProviderBehavior.OnDestroy: _provider.IsDisposed == false");
        }
        public void DestroyLate()
        {
            foreach (string context in contextsToManage)
            {
                _provider.PersistContext(context);
            }
            _provider.Dispose();
            _provider = null;
        }

        private string EnsureSaveFilePath(string contextKey)
        {
            var fileName = $"{contextKey}.json";
            var directoryPath = Path.Join(Application.persistentDataPath, rootFolderPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var saveFile = Path.Join(directoryPath, fileName);
            return saveFile;
        }
        
        public TextWriter WriteTo(string contextKey)
        {
            var filePath = EnsureSaveFilePath(contextKey);
            Log.Info($"Saving to {filePath}");
            return new StreamWriter(filePath, append: false);
        }

        public TextReader ReadFrom(string contextKey)
        {
            var filePath = EnsureSaveFilePath(contextKey);
            if (!File.Exists(filePath)) return null;
            Log.Info($"Reading from {filePath}");
            return new StreamReader(filePath);
        }

        public void Delete(string contextKey)
        {
            var filePath = EnsureSaveFilePath(contextKey);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public ISaveDataContext GetContext(string contextKey) => _provider.GetContext(contextKey);
        public void PersistContext(string contextKey) => _provider.PersistContext(contextKey);
        public void LoadContext(string contextKey) => _provider.LoadContext(contextKey);
        public void DeleteContext(string contextKey) => _provider.DeleteContext(contextKey);
        public IEnumerable<string> AllContexts() => _provider.AllContexts();
    }
}