using UnityEngine;
using Assets.Library.Data;
using Assets.Library.StateMachines;
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
            info.GenerationTransform = transform;

            GlandFactory gland_factory = new GlandFactory();

            gland_updater = new Updater<GlandStateCode, GlandInfo>(info, gland_factory);
            gland_updater.Start();
        }

        private void Update()
        {
            gland_updater.Update(Time.deltaTime);
        }
    }
}