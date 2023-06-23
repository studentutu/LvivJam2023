using System;
using System.Collections.Generic;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents.Skins
{
    public class ChangeSkinOnZoneChange : MonoBehaviour
    {
        [Serializable]
        public class SkinWithType
        {
            public SkinnedMeshRenderer _skin;
            public InteractionTypes TypeForSkin;
        }

        [SerializeField] private SkinnedMeshRenderer _renderer;
        [SerializeField] private List<SkinWithType> _skins = new List<SkinWithType>();

        private CompositeDisposable _disposable = new CompositeDisposable();
        
        private void OnEnable()
        {
            MessageBroker.Default.Receive<ChangeInteractionEvent>()
                .Subscribe(x =>ChangeSkin(x.Interaction))
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
        }

        private void ChangeSkin(InteractionTypes type)
        {
            var find = _skins.Find(x => x.TypeForSkin == type);

            _renderer.sharedMesh = find._skin.sharedMesh;
            _renderer.materials = find._skin.materials;
            
        }
    }
}