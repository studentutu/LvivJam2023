using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Jam.Scripts.BusEvents
{
    public class LoadSceneManager : MonoBehaviour
    {
        public List<AssetReference> _initialScenes;

        public Dictionary<AssetReference, SceneInstance>
            _activeScenes = new Dictionary<AssetReference, SceneInstance>();

        private CompositeDisposable _disposable = new CompositeDisposable();

        private void Awake()
        {
            LoadDefaultScenes().Forget();
        }

        private async UniTask LoadDefaultScenes()
        {
            foreach (var scenes in _initialScenes)
            {
                await LoadScene(scenes);
            }
        }

        public void OnEnable()
        {
            MessageBroker.Default.Receive<LoadSceneEvent>().Subscribe(x => LoadScene(x.Scene).Forget())
                .AddTo(_disposable);
            
            MessageBroker.Default.Receive<RemoveSceneEvent>().Subscribe(x => RemoveScene(x.Scene).Forget())
                .AddTo(_disposable);
            
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        private async UniTask RemoveScene(AssetReference scene)
        {
            var task = Addressables.UnloadSceneAsync(_activeScenes[scene]);
            await task;
            _activeScenes.Remove(scene);
        }


        private async UniTask<SceneInstance> LoadScene(AssetReference scene)
        {
            var instance = await Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _activeScenes.Add(scene, instance);
            
            return instance;
        }
    }
}
