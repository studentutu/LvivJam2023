using UnityEngine;

namespace Jam.Scripts.BusEvents
{
    public abstract class IInteraction : MonoBehaviour
    {
        public InteractionTypes Type = InteractionTypes.None;

        public abstract bool IsInAction(); 

        public abstract void InteractionStart();

        public abstract  void InteractionStop();
    }
}