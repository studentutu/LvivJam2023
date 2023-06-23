using System;
using System.Collections;
using System.Collections.Generic;
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

        public class SceneCounter
        {
            public SceneInstance SceneInstance;
            public int Counter;
        }

        public Dictionary<string, SceneCounter>
            _activeScenes = new Dictionary<string, SceneCounter>();

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
            if(_activeScenes.ContainsKey(scene.AssetGUID))
                _activeScenes[scene.AssetGUID].Counter--;
            await UniTask.DelayFrame(2);
            
            var checkIfNeedRelease = _activeScenes.ContainsKey(scene.AssetGUID) && _activeScenes[scene.AssetGUID].Counter < 1;

            if (checkIfNeedRelease)
            {
                var task = Addressables.UnloadSceneAsync(_activeScenes[scene.AssetGUID].SceneInstance);
                await task;
                _activeScenes.Remove(scene.AssetGUID);
            }
        }


        private async UniTask<SceneInstance> LoadScene(AssetReference scene)
        {
            if (_activeScenes.ContainsKey(scene.AssetGUID))
            {
                _activeScenes[scene.AssetGUID].Counter++;
                return _activeScenes[scene.AssetGUID].SceneInstance;
            }

            var instance = await Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _activeScenes.Add(scene.AssetGUID, new SceneCounter{SceneInstance = instance, Counter =  1});

            return instance;
        }
    }
}