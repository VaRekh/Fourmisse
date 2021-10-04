using System;
using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library.StateMachines;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;
using SeekerFactory = Assets.Library.StateMachines.Seeker.Factory;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private float movespeed;
        [SerializeField]
        private float direction_change_per_second;
        [SerializeField]
        private Transform anthill;
        [SerializeField]
        private ControllerInfo info;

        private Rigidbody2D rb;
        private Stopwatch stopwatch;
        private Collector collector;

        private Action Move;

        private Vector2Generator vector_generator;

        private Updater<SeekerStateCode, ControllerInfo> seeker_updater;

        private float DirectionChangeInterval
            => 1f / direction_change_per_second;


        // Start is called before the first frame update
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            collector = GetComponentInChildren<Collector>();

            Assert.IsNotNull(anthill);
            Assert.IsNotNull(collector);

            stopwatch = new Stopwatch(DirectionChangeInterval);

            info.CollectorStateChanged = collector.StateChanged;
            SeekerFactory seeker_factory = new SeekerFactory();

            seeker_updater = new Updater<SeekerStateCode, ControllerInfo>(info, seeker_factory);
            seeker_updater.StateChanged.AddListener(ReactToStateChanged);
            seeker_updater.Start();

            Range range = new Range(-1f, 1f);
            vector_generator = new Vector2Generator(range, range);

            void ReactToStateChanged(SeekerStateCode new_state)
            {
                switch (new_state)
                {
                    case SeekerStateCode.Seek:
                        Move = Seek;
                        break;
                    case SeekerStateCode.Collect:
                        Move = Collect;
                        break;
                    case SeekerStateCode.Return:
                        Move = Return;
                        break;
                    case SeekerStateCode.Dump:
                        Move = Dump;
                        break;
                    default:
                        break;
                }
            }

            void Seek()
            {
                if (stopwatch.CurrentValue >= DirectionChangeInterval)
                {
                    Vector2 random_normalized_direction = vector_generator.GetNormalized();
                    rb.ChangeDirection(movespeed, random_normalized_direction);

                    stopwatch.Reset();
                }
                stopwatch.Update(Time.deltaTime);
            }

            void Collect()
            {
                rb.ChangeDirection(0f, Vector2.zero);

                Move = DoNothing;
            }

            void Return()
            {
                Vector2 current_position = transform.position;
                Vector2 anthill_position = anthill.position;
                Vector2 anthill_direction = current_position.To(anthill_position);
                Vector2 normalized_direction = anthill_direction.normalized;
                rb.ChangeDirection(movespeed, normalized_direction);

                Move = DoNothing;
            }

            void Dump()
            {
                rb.ChangeDirection(0f, Vector2.zero);

                Move = DoNothing;
            }

            void DoNothing()
            { }
        }



        // Update is called once per frame
        private void Update()
        {
            Move();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            seeker_updater.OnTriggerEnter2D(collision);
        }
    }
}