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
            public float MilitaryPoints;
            public float VolonterPoints;
        }

        [SerializeField] private Slider _slider;
        [SerializeField] private Slider _sliderMilitary;
        [SerializeField] private Slider _sliderStorage;
        [SerializeField] private float StressDelta = 1f;
        [SerializeField] private TMP_Text _points;
        [SerializeField] private TMP_Text _timeLeft;
        [SerializeField] private GameState _InitialgameState;

        private GameState _gameState = new GameState();

        private CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            _slider.value = 0;

            MessageBroker.Default.Receive<UpdatePointsEvent>()
                .Subscribe(x => UpdatePoints(x))
                .AddTo(_disposable);
            
            MessageBroker.Default.Receive<UpdateStressDataDeltaEvent>()
                .Subscribe(x => UpdateStress(x.Amount))
                .AddTo(_disposable);

            _gameState = new GameState()
            {
                Playing = false, CurrentStress = 0, CurrentPoints = 0,
                TimeLeftSeconds = _InitialgameState.TimeLeftSeconds,
                MilitaryPoints = _InitialgameState.MilitaryPoints,
                VolonterPoints = _InitialgameState.VolonterPoints
            };
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
        }

        private void UpdatePoints(UpdatePointsEvent update)
        {
            var amount = update.Ammount * (update.Increase ? 1 : -1);

            _gameState.CurrentPoints += amount;
            if (update.Type == InteractionTypes.Shooting)
            {
                _gameState.MilitaryPoints = Normalize(_gameState.MilitaryPoints, update.Ammount);
            }

            if (update.Type == InteractionTypes.Helping)
            {
                _gameState.VolonterPoints = Normalize(_gameState.VolonterPoints, update.Ammount);
            }

            _points.text = "Points : " + _gameState.CurrentPoints.ToString("F1");
        }

        private float Normalize(float current, float update)
        {
            var currentH = current * 100;
            currentH += update;
            currentH /= 100f;
            return Mathf.Clamp01(currentH);
        }

        private void Update()
        {
            _gameState.TimeLeftSeconds -= Time.deltaTime;
            var timeSpan = TimeSpan.FromSeconds(_gameState.TimeLeftSeconds).ToString(@"mm\:ss");

            _timeLeft.text = "TimeLeft : " + timeSpan;

            _gameState.MilitaryPoints -= StressDelta * Time.deltaTime;
            _gameState.MilitaryPoints = Mathf.Clamp01(_gameState.MilitaryPoints);
            _sliderMilitary.value = _gameState.MilitaryPoints;
            
            _gameState.VolonterPoints -= StressDelta * Time.deltaTime;
            _gameState.VolonterPoints = Mathf.Clamp01(_gameState.VolonterPoints);
            _sliderStorage.value = _gameState.VolonterPoints;

            UpdateStress(-StressDelta * Time.deltaTime);
        }

        private void UpdateStress(float delta)
        {
            _gameState.CurrentStress = Normalize (_gameState.CurrentStress, delta);
            _slider.value = _gameState.CurrentStress;
            
            MessageBroker.Default.Publish(new NormalizedStressEvent { NormalizedStress = _gameState.CurrentStress});
        }
    }
}