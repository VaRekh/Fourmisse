using UnityEngine;
using UnityEngine.Events;
using Assets.Library.Data;
using Assets.Library.StateMachines;
using Assets.Library.StateMachines.Collector;
using CollectorFactory = Assets.Library.StateMachines.Collector.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Collector : MonoBehaviour
    {
        [SerializeField]
        private CollectorInfo info;

        private Updater<StateCode, CollectorInfo> collect_updater;

        public UnityEvent<StateCode> StateChanged;


        // Start is called before the first frame update
        private void Start()
        {
            StateChanged = new UnityEvent<StateCode>();

            CollectorFactory collector_factory = new CollectorFactory();

            collect_updater = new Updater<StateCode, CollectorInfo>(info, collector_factory);
            collect_updater.StateChanged.AddListener(ReactToStateChanged);
            collect_updater.Start();
        }

        // Update is called once per frame
        private void Update()
        {
            collect_updater.Update(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var resource = collision.gameObject.GetComponent<Resource>();

            object data = resource?.Collectable;

            if (data == null)
            {
                var anthill = collision.gameObject.GetComponent<Anthill>();

                data = anthill?.Storage;
            }

            collect_updater.OnTriggerEnter2D(collision, data);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            collect_updater.OnTriggerExit2D(collision);
        }

        private void ReactToStateChanged(StateCode new_state)
        {
            StateChanged.Invoke(new_state);
        }
    }
}