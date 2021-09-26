using UnityEngine;
using UnityEngine.Events;
using Assets.Library;
using Assets.Library.StateMachines.Collector;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Collector : MonoBehaviour
    {
        [SerializeField]
        private CollectorInfo info;

        private StateUpdater collect_state;

        public UnityEvent<StateCode> StateChanged;


        // Start is called before the first frame update
        private void Start()
        {
            StateChanged = new UnityEvent<StateCode>();

            collect_state = new StateUpdater(info);
            collect_state.StateChanged.AddListener(ReactToStateChanged);
            collect_state.Start();
        }

        // Update is called once per frame
        private void Update()
        {
            collect_state.Update(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collect_state.OnTriggerEnter2D(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            collect_state.OnTriggerExit2D(collision);
        }

        private void ReactToStateChanged(StateCode new_state)
        {
            StateChanged.Invoke(new_state);
        }
    }
}