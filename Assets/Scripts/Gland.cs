using UnityEngine;
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
        private SerializedInfo info;

        private Updater<GlandStateCode, Info> gland_updater;

        private void Start()
        {
            Info actual_info = info.Build();

            GlandFactory gland_factory = new GlandFactory();

            gland_updater = new Updater<GlandStateCode, Info>(actual_info, gland_factory);
            gland_updater.Start();
        }

        private void Update()
        {
            gland_updater.Update(Time.deltaTime);
        }
    }
}