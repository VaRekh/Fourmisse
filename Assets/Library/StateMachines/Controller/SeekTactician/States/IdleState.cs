namespace Assets.Library.StateMachines.Controller.SeekTactician.States
{
    public class IdleState : State<StateCode, SeekTacticianInfo>
    {
        public IdleState(Updater<StateCode, SeekTacticianInfo> updater, SeekTacticianInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Idle);

            Info.ClearTrackedPheromones();
        }

        public override void Update(float delta_time)
        {
            PheromoneInfo nearest_pheromone = Info.GetNearestPheromone(Info.DetectedPheromones);

            if (nearest_pheromone == null)
            {
                Updater.Change(StateCode.RandomMove);
            }
            else
            {
                Updater.Change(StateCode.TrackPheromone, nearest_pheromone);
            }
        }
    }
}