using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Library.StateMachines;
using Assets.Library;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;
using SeekerFactory = Assets.Library.StateMachines.Seeker.Factory;
using ControllerStateCode = Assets.Library.StateMachines.Controller.StateCode;
using ControllerFactory = Assets.Library.StateMachines.Controller.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private SeekerInfo seeker_info;
        [SerializeField]
        private ControllerInfo controller_info;

        private Action Move;

        private Updater<ControllerStateCode, ControllerInfo> controller_updater;

        private Updater<SeekerStateCode, SeekerInfo> seeker_updater;

        public UnityEvent<SeekerStateCode> SeekerStateChanged
            => seeker_updater.StateChanged;




        // Start is called before the first frame update
        private void Start()
        {
            var collector = GetComponentInChildren<Collector>();
            Assert.IsNotNull(collector);

            seeker_info.CollectorStateChanged = collector.StateChanged;


            SeekerFactory seeker_factory = new SeekerFactory();
            seeker_updater = new Updater<SeekerStateCode, SeekerInfo>(seeker_info, seeker_factory);


            var rb = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(rb);

            controller_info.Rigidbody = rb;
            controller_info.Ant = transform;
            controller_info.SeekerStateChanged = SeekerStateChanged;


            ControllerFactory controller_factory = new ControllerFactory();
            controller_updater = new Updater<ControllerStateCode, ControllerInfo>(controller_info, controller_factory);
            controller_updater.StateChanged.AddListener(ReactToControllerStateChanged);
            controller_updater.Start();
            seeker_updater.Start();



            static void ReactToControllerStateChanged(ControllerStateCode code)
            {
                print(Enum.GetName(typeof(ControllerStateCode), code));
            }
        }



        // Update is called once per frame
        private void Update()
        {
            controller_updater.Update(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            seeker_updater.OnTriggerEnter2D(collision);
        }
    }
}