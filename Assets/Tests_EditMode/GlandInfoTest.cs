#nullable enable
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

using UnityAssertionException = UnityEngine.Assertions.AssertionException;

using Assets.Library;
using Assets.Library.StateMachines.Gland;

namespace Tests.Library.Data
{
    public class GlandInfoTest
    {
        private struct GlandInfoConstructorParameters
        {
            public StrictlyPositiveFloat GeneratedPheromonePerSecond { get; set; }
            public Transform GenerationTransform { get; set; }
            public GameObject PheromoneTemplate { get; set; }
            public UnityEvent<Collectable> ContactWithCollectableLost { get; set; }
            public UnityEvent<Storage> ContactWithStorageHappened { get; set; }
        }

        private Transform MakeDefaultTransform()
        {
            var game_object = new GameObject();
            var transform = game_object.transform;
            return transform;
        }

        private GlandInfoConstructorParameters MakeDefaultParameters()
        {
            return new GlandInfoConstructorParameters
            {
                GeneratedPheromonePerSecond = new StrictlyPositiveFloat(1f),
                GenerationTransform = MakeDefaultTransform(),
                PheromoneTemplate = new GameObject(),
                ContactWithCollectableLost = new UnityEvent<Collectable>(),
                ContactWithStorageHappened = new UnityEvent<Storage>()
            };
        }

        private GlandInfo MakeGlandInfo(GlandInfoConstructorParameters constructor_parameters)
        {
            return new GlandInfo
            (
                constructor_parameters.GeneratedPheromonePerSecond,
                constructor_parameters.GenerationTransform,
                constructor_parameters.PheromoneTemplate,
                constructor_parameters.ContactWithCollectableLost,
                constructor_parameters.ContactWithStorageHappened
            );
        }

        [TestCase(1f)]
        [TestCase(2f)]
        public void GetPheromoneProductionTimeInSecond_SetGeneratedPheromonePerSecond_ReturnsInverseValue
        (
            float value
        )
        {
            var generated_pheromone_per_second = new StrictlyPositiveFloat(value);
            var parameters = MakeDefaultParameters();
            parameters.GeneratedPheromonePerSecond = generated_pheromone_per_second;
            var expected = 1f / parameters.GeneratedPheromonePerSecond;
            var info = MakeGlandInfo(parameters);

            var actual = info.PheromoneProductionTimeInSecond;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WhenContactWithCollectableLost_ListenerShouldReactsWhenEventOccurs()
        {
            var parameters = MakeDefaultParameters();
            var info = MakeGlandInfo(parameters);

            info.ListenToLossOfContactWithCollectable(OnLossOfContact);
            bool reacted_to_event = false;

            parameters.ContactWithCollectableLost.Invoke(new Collectable(0));
            Assert.That(reacted_to_event, Is.True);

            void OnLossOfContact(Collectable collectable)
            {
                reacted_to_event = true;
            }
        }

        [Test]
        public void WhenContactWithCollectableLost_ListenerShouldNotReactsWhenUnsubscribed()
        {
            var parameters = MakeDefaultParameters();
            var info = MakeGlandInfo(parameters);

            parameters.ContactWithCollectableLost.AddListener(OnLossOfContact);
            info.StopListeningToLossOfContactWithCollectable(OnLossOfContact);
            bool reacted_to_event = false;

            parameters.ContactWithCollectableLost.Invoke(new Collectable(0));
            Assert.That(reacted_to_event, Is.False);

            void OnLossOfContact(Collectable collectable)
            {
                reacted_to_event = true;
            }
        }

        [Test]
        public void WhenContactWithStoragelHappened_ListenerShouldReactsWhenEventOccurs()
        {
            var parameters = MakeDefaultParameters();
            var info = MakeGlandInfo(parameters);

            info.ListenToContactWithStorage(OnContact);
            bool reacted_to_event = false;

            parameters.ContactWithStorageHappened.Invoke(new Storage(0));
            Assert.That(reacted_to_event, Is.True);

            void OnContact(Storage storage)
            {
                reacted_to_event = true;
            }
        }

        [Test]
        public void WhenContactWithStoragelHappened_ListenerShouldNotReactsWhenUnsubscribed()
        {
            var parameters = MakeDefaultParameters();
            var info = MakeGlandInfo(parameters);

            parameters.ContactWithStorageHappened.AddListener(OnContact);
            info.StopListeningToContactWithStorage(OnContact);
            bool reacted_to_event = false;

            parameters.ContactWithStorageHappened.Invoke(new Storage(0));
            Assert.That(reacted_to_event, Is.False);

            void OnContact(Storage storage)
            {
                reacted_to_event = true;
            }
        }

        [Test]
        public void InstantiatePheromone_ReturnsPheromoneMatchingProvidedParameters()
        {
            var parameters = MakeDefaultParameters();
            var info = MakeGlandInfo(parameters);

            var pheromone = info.InstantiatePheromone();

            Assert.That
            (
                pheromone.transform.position,
                Is.EqualTo(parameters.GenerationTransform.position)
            );

            Assert.That
            (
                pheromone.transform.rotation,
                Is.EqualTo(parameters.GenerationTransform.rotation)
            );
        }

        [Test, Combinatorial]
        public void InstantiatePheromone_ReturnsPheromoneMatchingPosition
        (
            [Values(-2f, 0f, 1f, 5f)] float position_x,
            [Values(-2f, 0f, 1f, 5f)] float position_y
        )
        {
            var parameters = MakeDefaultParameters();
            parameters.GenerationTransform.position = new Vector3(position_x, position_y);
            var info = MakeGlandInfo(parameters);

            var pheromone = info.InstantiatePheromone();

            Assert.That
            (
                pheromone.transform.position,
                Is.EqualTo(parameters.GenerationTransform.position)
            );
        }

        [Test]
        public void InstantiatePheromone_ReturnsPheromoneMatchingRotation
        (
            [Values(-25f, 0f, 90f, 180f)] float rotation_z
        )
        {
            var parameters = MakeDefaultParameters();
            parameters.PheromoneTemplate.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
            var info = MakeGlandInfo(parameters);

            var pheromone = info.InstantiatePheromone();

            Assert.That
            (
                pheromone.transform.rotation,
                Is.EqualTo(parameters.PheromoneTemplate.transform.rotation)
            );
        }

        [Test]
        public void WhenConstructorIsCalledWithNullTransform_ThrowsNullArgumentException()
        {
            var parameters = new GlandInfoConstructorParameters
            {
                GeneratedPheromonePerSecond = new StrictlyPositiveFloat(1f),
                ContactWithCollectableLost = new UnityEvent<Collectable>(),
                ContactWithStorageHappened = new UnityEvent<Storage>(),
                PheromoneTemplate = new GameObject()
            };

            Assert.That
            (
                () => { MakeGlandInfo(parameters); },
                Throws  .InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was Null")
            );
        }

        [Test]
        public void WhenConstructorIsCalledWithNullContactWithCollectableLost_ThrowsNullArgumentException()
        {
            var parameters = new GlandInfoConstructorParameters
            {
                GeneratedPheromonePerSecond = new StrictlyPositiveFloat(1f),
                GenerationTransform = MakeDefaultTransform(),
                ContactWithStorageHappened = new UnityEvent<Storage>(),
                PheromoneTemplate = new GameObject()
            };

            Assert.That
            (
                () => { MakeGlandInfo(parameters); },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was Null")
            );
        }

        [Test]
        public void WhenConstructorIsCalledWithNullContactWithStorageHappened_ThrowsNullArgumentException()
        {
            var parameters = new GlandInfoConstructorParameters
            {
                GeneratedPheromonePerSecond = new StrictlyPositiveFloat(1f),
                GenerationTransform = MakeDefaultTransform(),
                ContactWithCollectableLost = new UnityEvent<Collectable>(),
                PheromoneTemplate = new GameObject()
            };

            Assert.That
            (
                () => { MakeGlandInfo(parameters); },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was Null")
            );
        }

        [Test]
        public void WhenConstructorIsCalledWithNullPheromoneTemplate_ThrowsNullArgumentException()
        {
            var parameters = new GlandInfoConstructorParameters
            {
                GeneratedPheromonePerSecond = new StrictlyPositiveFloat(1f),
                GenerationTransform = MakeDefaultTransform(),
                ContactWithCollectableLost = new UnityEvent<Collectable>(),
                ContactWithStorageHappened = new UnityEvent<Storage>()
            };

            Assert.That
            (
                () => { MakeGlandInfo(parameters); },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was Null")
            );
        }
    }
}