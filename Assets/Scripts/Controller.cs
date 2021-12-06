using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library.Data;
using Assets.Library.StateMachines;
using ControllerStateCode = Assets.Library.StateMachines.Controller.StateCode;
using ControllerFactory = Assets.Library.StateMachines.Controller.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Seeker))]
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private ControllerInfo info;
        [SerializeField]
        private LayerReference ant_pheromone_detection_layer;

        private Updater<ControllerStateCode, ControllerInfo> controller_updater;

        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(ant_pheromone_detection_layer);

            var parent_transform = transform.parent;
            Assert.IsNotNull(parent_transform);

            var pheromone_detectors = parent_transform.GetComponentsInChildren<PheromoneDetector>();
            Assert.IsTrue(pheromone_detectors.Length == 1);
            var detector = pheromone_detectors[0].Detector;
            Assert.IsNotNull(detector);

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

            var seeker = GetComponent<Seeker>();
            Assert.IsNotNull(seeker);
            var seeker_updater = seeker.SeekerUpdater;
            Assert.IsNotNull(seeker_updater);

            info.Init
            (
            pheromone_detection_area,
            rb,
            parent_transform,
            seeker_updater,
            detector
            );


            ControllerFactory controller_factory = new ControllerFactory();
            controller_updater = new Updater<ControllerStateCode, ControllerInfo>(info, controller_factory);
            controller_updater.Start();
        }

        // Update is called once per frame
        private void Update()
        {
            controller_updater.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            controller_updater.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}