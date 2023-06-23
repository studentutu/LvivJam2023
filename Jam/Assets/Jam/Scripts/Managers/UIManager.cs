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
            public float MilitaryStress;
            public float StorageStress;
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

        private void UpdatePoints(UpdatePointsEvent update)
        {
            var amount = update.Ammount * (update.Increase ? 1 : -1);

            _gameState.CurrentPoints += amount;
            if (update.Type == InteractionTypes.Shooting)
            {
                _gameState.MilitaryStress = Normalize(_gameState.MilitaryStress, update.Ammount);
            }

            if (update.Type == InteractionTypes.Helping)
            {
                _gameState.StorageStress = Normalize(_gameState.StorageStress, update.Ammount);
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

            _gameState.MilitaryStress -= StressDelta * Time.deltaTime;
            _gameState.MilitaryStress = Mathf.Clamp01(_gameState.MilitaryStress);
            _sliderMilitary.value = _gameState.MilitaryStress;
            
            _gameState.StorageStress -= StressDelta * Time.deltaTime;
            _gameState.StorageStress = Mathf.Clamp01(_gameState.StorageStress);
            _sliderStorage.value = _gameState.StorageStress;

            _slider.value = (_gameState.MilitaryStress + _gameState.StorageStress) * 0.5f;
            _gameState.CurrentStress = _slider.value;
            
            MessageBroker.Default.Publish(new UpdateStressEvent { NormalizedStress = _gameState.CurrentStress});
        }
    }
}