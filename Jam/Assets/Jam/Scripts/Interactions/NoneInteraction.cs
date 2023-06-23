namespace Jam.Scripts.BusEvents
{
    public class NoneInteraction :IInteraction
    {
        public override bool IsInAction()
        {
            return true;
        }

        public override void InteractionStart()
        {
            
        }

        public override void InteractionStop(InteractionTypes newType)
        {
           
        }
    }
}