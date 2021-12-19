using UnityEngine;
using UnityEngine.Events;

using Assets.Library;
using Assets.Library.StateMachines;
using Assets.Library.StateMachines.Gland;
using GlandStateCode = Assets.Library.StateMachines.Gland.StateCode;
using GlandFactory = Assets.Library.StateMachines.Gland.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Seeker))]
    public class Gland : MonoBehaviour
    {
        [SerializeField]
        private GlandInfo info;

        private Updater<GlandStateCode, GlandInfo> gland_updater;

        private void Start()
        {
            var seeker = GetComponent<Seeker>();
            info.SeekerStateChanged = seeker.StateChanged;
            GlandInfo TEMPINFO = new GlandInfo
            (
                new StrictlyPositiveFloat(1f / info.GenerationInterval),
                transform,
                info.Pheromone,
                new UnityEvent<Collectable>(), // ATTENTION !
                new UnityEvent<Storage>()
            );
            GlandFactory gland_factory = new GlandFactory();

            gland_updater = new Updater<GlandStateCode, GlandInfo>(TEMPINFO, gland_factory);
            gland_updater.Start();
        }

        private void Update()
        {
            gland_updater.Update(Time.deltaTime);
        }
    }
}