﻿using Assets.Scripts;

namespace Assets.Library.StateMachines.Controller.SeekTactician
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
            Pheromone nearest_pheromone = Info.GetNearestPheromone(Info.DetectedPheromones);

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