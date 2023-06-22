using System;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        public InteractionTypes CurrentInteraction;
        private CompositeDisposable _disposable = new();
        
        public void OnEnable()
        {
            MessageBroker.Default.Receive<ChangeInteractionEvent>().Subscribe(x => ChangeInteraction(x.Interaction))
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
            _disposable = new();
        }

        private void ChangeInteraction(InteractionTypes newInteraction)
        {
            CurrentInteraction = newInteraction;
        }
    }
}