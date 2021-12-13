#nullable enable
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

using UnityAssertionException = UnityEngine.Assertions.AssertionException;

using Assets.Library;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;
using Assets.Library.Data;

namespace Tests.Library.Data
{
    public class GlandInfoTest
    {
        private struct GlandInfoConstructorParameters
        {
            public StrictlyPositiveFloat GeneratedPheromonePerSecond { get; set; }
            public Transform Transform { get; set; }
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
                Transform = MakeDefaultTransform(),
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
                constructor_parameters.Transform,
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
        public void GetGenerationPosition_WhenPositionInTransformIsDefault_ReturnsSamePosition()
        {
            var parameters = MakeDefaultParameters();
            Vector2 expected = parameters.Transform.position;

            var info = MakeGlandInfo(parameters);
            var actual = info.GenerationPosition;

            Assert.AreEqual(expected, actual);
        }

        [TestCase(0f, 1f)]
        [TestCase(4f, 2f)]
        [TestCase(-2f, 5f)]
        [TestCase(1f, 0f)]
        public void GetGenerationPosition_WhenPositionInTransformIsModified_ReturnsModifiedPosition
        (
            float x_expected,
            float y_expected
        )
        {
            var expected = new Vector2(x_expected, y_expected);
            var parameters = MakeDefaultParameters();
            var info = MakeGlandInfo(parameters);

            parameters.Transform.position = expected;
            var actual = info.GenerationPosition;

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
                Transform = MakeDefaultTransform(),
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
                Transform = MakeDefaultTransform(),
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
                Transform = MakeDefaultTransform(),
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