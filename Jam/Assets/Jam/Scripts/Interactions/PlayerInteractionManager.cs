using System;
using System.Collections.Generic;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using UnityStarterAssets;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        public StarterAssetsInputs _input;
        public InteractionTypes CurrentInteraction;
        public List<IInteraction> Interactions = new List<IInteraction>();


        private CompositeDisposable _disposable = new();
        private IInteraction _currentInteraction;

        public void OnEnable()
        {
            MessageBroker.Default.Receive<ChangeInteractionEvent>().Subscribe(x => ChangeInteraction(x.Interaction))
                .AddTo(_disposable);

            ChangeInteraction(CurrentInteraction);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
            _disposable = new();
        }

        private void ChangeInteraction(InteractionTypes newInteraction)
        {
            if (_currentInteraction != null)
                _currentInteraction.InteractionStop();

            CurrentInteraction = newInteraction;
            _currentInteraction = Interactions.Find(x => x.Type == CurrentInteraction);
        }


        private void Update()
        {
            var interactInUse = _input.toggledInteraction;

            if (_currentInteraction == null)
                return;

            if (interactInUse && _currentInteraction.enabled)
                return;

            if (!interactInUse && !_currentInteraction.enabled)
                return;

            if (interactInUse)
                _currentInteraction.InteractionStart();
            else
                _currentInteraction.InteractionStop();
        }
    }
}