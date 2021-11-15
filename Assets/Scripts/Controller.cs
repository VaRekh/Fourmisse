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
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private SeekerInfo seeker_info;
        [SerializeField]
        private ControllerInfo controller_info;
        [SerializeField]
        private LayerReference ant_pheromone_detection_layer;
            
        private Action Move;

        private Updater<ControllerStateCode, ControllerInfo> controller_updater;

        private Updater<SeekerStateCode, SeekerInfo> seeker_updater;

        public UnityEvent<SeekerStateCode> SeekerStateChanged
            => seeker_updater.StateChanged;


        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(ant_pheromone_detection_layer);
            var parent_transform = transform.parent;
            Assert.IsNotNull(parent_transform);

            var collectors = parent_transform.GetComponentsInChildren<Collector>();
            Assert.IsTrue(collectors.Length == 1);
            seeker_info.CollectorStateChanged = collectors[0].StateChanged;

            var pheromone_detectors = parent_transform.GetComponentsInChildren<PheromoneDetector>();
            Assert.IsTrue(pheromone_detectors.Length == 1);

            SeekerFactory seeker_factory = new SeekerFactory();
            seeker_updater = new Updater<SeekerStateCode, SeekerInfo>(seeker_info, seeker_factory);


            var rb = GetComponentInParent<Rigidbody2D>();
            Assert.IsNotNull(rb);

            var candidate_colliders = parent_transform.GetComponentsInChildren<Collider2D>();

            Collider2D pheromone_detection_area = null;

            foreach (Collider2D candidate_collider in candidate_colliders)
            {
                if (candidate_collider.gameObject.layer == ant_pheromone_detection_layer.Index)
                {
                    pheromone_detection_area = candidate_collider;
                    break;
                }
            }
            Assert.IsNotNull(pheromone_detection_area);


            controller_info.Collider = pheromone_detection_area;
            controller_info.Rigidbody = rb;
            controller_info.Ant = parent_transform;
            controller_info.SeekerStateChanged = SeekerStateChanged;
            controller_info.PheromoneDetector = pheromone_detectors[0];


            ControllerFactory controller_factory = new ControllerFactory();
            controller_updater = new Updater<ControllerStateCode, ControllerInfo>(controller_info, controller_factory);
            controller_updater.Start();
            seeker_updater.Start();
        }



        // Update is called once per frame
        private void Update()
        {
            seeker_updater.Update(Time.deltaTime);
            controller_updater.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            controller_updater.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}