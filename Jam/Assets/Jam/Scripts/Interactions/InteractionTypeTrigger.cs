using Jam.Scripts.BusEvents.BusEvents.Interactions;
using Jam.Scripts.BusEvents.ChangeTriggerZone;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents
{
    /// <summary>
    ///     To Change interaction.
    /// </summary>
    public class InteractionTypeTrigger : MonoBehaviour
    {
        public InteractionTypes Type;
        public ChangeTrigger Trigger;

        private void OnEnable()
        {
            Trigger.ChangeOnTriggerEnter.AddListener(OnEnter);
        }
        
        private void OnDisable()
        {
            Trigger.ChangeOnTriggerEnter.RemoveListener(OnEnter);
        }

        private void OnEnter()
        {
            MessageBroker.Default.Publish<ChangeInteractionEvent>(new ChangeInteractionEvent{Interaction = Type});
        }
        
    }
}