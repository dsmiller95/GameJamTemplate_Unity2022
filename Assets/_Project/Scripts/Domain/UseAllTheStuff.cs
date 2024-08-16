using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dman.Utilities.Logger;
using R3;
using UnityEngine;

namespace Domain
{
    public class UseAllTheStuff : MonoBehaviour
    {
        [SerializeField] private SerializableReactiveProperty<string> reactiveString;
        private AsyncFnOnceCell _asyncFnOnceCell;

        private void Awake()
        {
            reactiveString.Subscribe(s =>
            {
                Log.Info($"Reactive string changed to {s}");
            }).AddTo(this);
            _asyncFnOnceCell = new AsyncFnOnceCell(gameObject);
            _asyncFnOnceCell.TryRun(RunForever, "could not start forever game loop");
        }
        
        private async UniTask RunForever(CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                Log.Info("Running forever");
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancel);
            }
        }
    }
}
