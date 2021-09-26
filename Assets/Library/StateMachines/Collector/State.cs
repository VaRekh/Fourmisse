namespace Assets.Library.StateMachines.Collector
{
    public class State : State<StateUpdater>
    {
        protected CollectorInfo Info { get; private set; }

        public State(StateUpdater updater, CollectorInfo info)
            : base(updater)
        {
            Info = info;
        }
    }
}