using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents
{
    public class AudioManager : MonoBehaviour
    {
        [Serializable]
        public class SoundForInteraction
        {
            public AudioSource Source;
            public InteractionTypes Type;
        }

        [SerializeField] private List<SoundForInteraction> List = new();
        public float MaxVolume = 0.7f;

        private CompositeDisposable _disposable = new();
        private CancellationTokenSource _token;

        private void OnEnable()
        {
            MessageBroker.Default.Receive<ChangeInteractionEvent>().Subscribe(x => ChangeInteraction(x.Interaction))
                .AddTo(_disposable);

            ChangeInteraction(InteractionTypes.None);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
            _disposable = new();
        }

        private void ChangeInteraction(InteractionTypes type)
        {
            foreach (var sources in List)
                sources.Source.volume = 0;

            var find = List.Find(x => x.Type == type);
            _token?.Cancel();

            _token = new CancellationTokenSource();
            LerpVolume(find.Source, _token).Forget();
        }

        private async UniTask LerpVolume(AudioSource source, CancellationTokenSource token)
        {
            while (source.isPlaying && !token.IsCancellationRequested)
            {
                if (source.volume > MaxVolume)
                    return;

                source.volume += Time.deltaTime;
                await UniTask.Yield();
            }
        }
    }
}