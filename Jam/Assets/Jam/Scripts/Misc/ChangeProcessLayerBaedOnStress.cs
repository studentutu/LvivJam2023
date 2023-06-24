using Jam.Scripts.BusEvents;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

namespace Jam.Scripts.Misc
{
    public class ChangeProcessLayerBaedOnStress : MonoBehaviour
    {
        [SerializeField] public Volume _cameraVolume;
        
        private CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            MessageBroker.Default.Receive<NormalizedStressEvent>()
                .Subscribe(x => UpdateStress(x))
                .AddTo(_disposable);
            
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
        }

        private void UpdateStress(NormalizedStressEvent data)
        {
            _cameraVolume.weight = data.NormalizedStress;
        }

    }
}