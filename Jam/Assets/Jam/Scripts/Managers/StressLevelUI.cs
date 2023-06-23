using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.BusEvents
{
    public class StressLevelUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            _slider.value = 0;

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
            _slider.value = data.NormalizedStress;
        }
    }
}