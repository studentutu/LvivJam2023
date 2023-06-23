using System;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.BusEvents
{
    public class UIManager : MonoBehaviour
    {
        [Serializable]
        public class GameState
        {
            public bool Playing = false;
            public float CurrentStress;
            public float CurrentPoints;
            public float TimeLeftSeconds;
        }

        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _points;
        [SerializeField] private TMP_Text _timeLeft;
        [SerializeField] private GameState _InitialgameState;

        private GameState _gameState = new GameState();

        private CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            _slider.value = 0;

            MessageBroker.Default.Receive<UpdateStressEvent>()
                .Subscribe(x => UpdateStressLevel(x))
                .AddTo(_disposable);

            MessageBroker.Default.Receive<UpdatePointsEvent>()
                .Subscribe(x => UpdatePoints(x))
                .AddTo(_disposable);

            _gameState = new GameState()
            {
                Playing = false, CurrentStress = 0, CurrentPoints = 0,
                TimeLeftSeconds = _InitialgameState.TimeLeftSeconds
            };
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
            _gameState.CurrentStress = normal;
        }

        private void UpdatePoints(UpdatePointsEvent update)
        {
            _gameState.CurrentPoints += update.Ammount * (update.Increase ? 1 : -1);
            _points.text = "Points : " + _gameState.CurrentPoints.ToString("F1");
        }

        private void Update()
        {
            _gameState.TimeLeftSeconds -= Time.deltaTime;
            var timeSpan = TimeSpan.FromSeconds(_gameState.TimeLeftSeconds).ToString(@"mm\:ss");

            _timeLeft.text = "TimeLeft : " + timeSpan;
        }
    }
}