using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.BusEvents
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        private void OnEnable()
        {
            _slider.value = 0;
            
            MessageBroker.Default.Receive<UpdateStressEvent>()
                .Subscribe(x => UpdateStressLevel(x))
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
        }

        private void UpdateStressLevel(UpdateStressEvent stressUpdate)
        {
            var current = _slider.value * 100f;
            current += stressUpdate.Ammount * (stressUpdate.Increase ? 1 : -1);

            var normal = current / 100f;
            _slider.value = normal;
        }
    }
}