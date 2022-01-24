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
            public float GeneratedPheromonePerSecond { get; set; }
            public Transform GenerationPosition { get; set; }
            public GameObject PheromoneTemplate { get; set; }
            public UnityEvent<Collectable> ContactWithCollectableLost { get; set; }
            public UnityEvent<Storage> ContactWithStorageHappened { get; set; }
        }

        private struct SerializedInfoBuildParameters
        {
            public UnityEvent<Collectable> ContactWithCollectableLost { get; set; }
            public UnityEvent<Storage> ContactWithStorageHappened { get; set; }
        }

        private Transform MakeDefaultTransform()
        {
            var game_object = new GameObject();
            var transform = game_object.transform;
            return transform;
        }

        private GlandInfoConstructorParameters MakeGlandInfoDefaultParameters()
        {
            var serialized_info = MakeSerializedInfoDefaultValidParameters();
            return new GlandInfoConstructorParameters
            {
                GeneratedPheromonePerSecond = serialized_info.GeneratedPheromonePerSecond,
                GenerationPosition = serialized_info.GenerationPosition,
                PheromoneTemplate = serialized_info.PheromoneTemplate,
                ContactWithCollectableLost = new UnityEvent<Collectable>(),
                ContactWithStorageHappened = new UnityEvent<Storage>()
            };
        }

        private Info MakeGlandInfo(GlandInfoConstructorParameters parameters)
        {
            var serialized_info = new SerializedInfo
            {
                GeneratedPheromonePerSecond = parameters.GeneratedPheromonePerSecond,
                GenerationPosition = parameters.GenerationPosition,
                PheromoneTemplate = parameters.PheromoneTemplate
            };
            return new Info
            (
                serialized_info,
                parameters.ContactWithCollectableLost,
                parameters.ContactWithStorageHappened
            );
        }

        private SerializedInfo MakeSerializedInfoDefaultValidParameters()
        {
            return new SerializedInfo
            {
                GeneratedPheromonePerSecond = 1f,
                GenerationPosition = MakeDefaultTransform(),
                PheromoneTemplate = new GameObject()
            };
        }

        private SerializedInfoBuildParameters MakeSerializedInfoBuildDefaultValueParameters()
        {
            return new SerializedInfoBuildParameters
            {
                ContactWithCollectableLost = new UnityEvent<Collectable>(),
                ContactWithStorageHappened = new UnityEvent<Storage>()
            };
        }

        [TestCase(1f)]
        [TestCase(2f)]
        public void GetPheromoneProductionTimeInSecond_SetGeneratedPheromonePerSecond_ReturnsInverseValue
        (
            float value
        )
        {
            var generated_pheromone_per_second = new StrictlyPositiveFloat(value);
            var parameters = MakeGlandInfoDefaultParameters();
            parameters.GeneratedPheromonePerSecond = generated_pheromone_per_second;
            var expected = 1f / parameters.GeneratedPheromonePerSecond;
            var info = MakeGlandInfo(parameters);

            var actual = info.PheromoneProductionTimeInSecond;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WhenContactWithCollectableLost_ListenerShouldReactsWhenEventOccurs()
        {
            var parameters = MakeGlandInfoDefaultParameters();
            var info = MakeGlandInfo(parameters);

            info.ListenToLossOfContactWithCollectable(OnLossOfContact);
            bool reacted_to_event = false;

            parameters.ContactWithCollectableLost.Invoke(new Collectable(new UintReference(0)));
            Assert.That(reacted_to_event, Is.True);

            void OnLossOfContact(Collectable collectable)
            {
                reacted_to_event = true;
            }
        }

        [Test]
        public void WhenContactWithCollectableLost_ListenerShouldNotReactsWhenUnsubscribed()
        {
            var parameters = MakeGlandInfoDefaultParameters();
            var info = MakeGlandInfo(parameters);

            parameters.ContactWithCollectableLost.AddListener(OnLossOfContact);
            info.StopListeningToLossOfContactWithCollectable(OnLossOfContact);
            bool reacted_to_event = false;

            parameters.ContactWithCollectableLost.Invoke(new Collectable(new UintReference(0)));
            Assert.That(reacted_to_event, Is.False);

            void OnLossOfContact(Collectable collectable)
            {
                reacted_to_event = true;
            }
        }

        [Test]
        public void WhenContactWithStoragelHappened_ListenerShouldReactsWhenEventOccurs()
        {
            var parameters = MakeGlandInfoDefaultParameters();
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
            var parameters = MakeGlandInfoDefaultParameters();
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
            var parameters = MakeGlandInfoDefaultParameters();
            var info = MakeGlandInfo(parameters);

            var pheromone = info.InstantiatePheromone();

            Assert.That
            (
                pheromone.transform.position,
                Is.EqualTo(parameters.GenerationPosition.position)
            );

            Assert.That
            (
                pheromone.transform.rotation,
                Is.EqualTo(parameters.GenerationPosition.rotation)
            );
        }

        [Test, Combinatorial]
        public void InstantiatePheromone_ReturnsPheromoneMatchingPosition
        (
            [Values(-2f, 0f, 1f, 5f)] float position_x,
            [Values(-2f, 0f, 1f, 5f)] float position_y
        )
        {
            var parameters = MakeGlandInfoDefaultParameters();
            parameters.GenerationPosition.position = new Vector3(position_x, position_y);
            var info = MakeGlandInfo(parameters);

            var pheromone = info.InstantiatePheromone();

            Assert.That
            (
                pheromone.transform.position,
                Is.EqualTo(parameters.GenerationPosition.position)
            );
        }

        [Test]
        public void InstantiatePheromone_ReturnsPheromoneMatchingRotation
        (
            [Values(-25f, 0f, 90f, 180f)] float rotation_z
        )
        {
            var parameters = MakeGlandInfoDefaultParameters();
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
        public void WhenPheromoneTemplateIsInvalid_SerializedInfoShouldFailInBuildingInfo()
        {
            var build_parameters = MakeSerializedInfoBuildDefaultValueParameters();
            var serialized_info = MakeSerializedInfoDefaultValidParameters();
            serialized_info.PheromoneTemplate = null;
            Assert.That
            (
                () =>
                {
                    serialized_info.Build
                    (
                        build_parameters.ContactWithCollectableLost,
                        build_parameters.ContactWithStorageHappened
                    );
                },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was Null")
            );
        }

        [Test]
        public void WhenGeneratedPheromonePerSecondPheromonePerSecondIsInvalid_SerializedInfoShouldFailInBuildingInfo
        (
            [Values(-2f, -1f, 0f)] float generated_pheromone_per_second
        )
        {
            var build_parameters = MakeSerializedInfoBuildDefaultValueParameters();
            var serialized_info = MakeSerializedInfoDefaultValidParameters();
            serialized_info.GeneratedPheromonePerSecond = generated_pheromone_per_second;

            Assert.That
            (
                () =>
                {
                    serialized_info.Build
                    (
                        build_parameters.ContactWithCollectableLost,
                        build_parameters.ContactWithStorageHappened
                    );
                },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was False")
            );
        }

        [Test]
        public void WhenGenerationPositionIsInvalid_SerializedInfoShouldFailInBuildingInfo()
        {
            var build_parameters = MakeSerializedInfoBuildDefaultValueParameters();
            var serialized_info = MakeSerializedInfoDefaultValidParameters();

            serialized_info.GenerationPosition = null;
           
            Assert.That
            (
                () =>
                {
                    serialized_info.Build
                    (
                        build_parameters.ContactWithCollectableLost,
                        build_parameters.ContactWithStorageHappened
                    );
                },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was Null")
            );
        }

        [Test]
        public void WhenDataIsValid_SerializedInfoShouldSucceedInBuildingInfo()
        {
            var build_parameters = MakeSerializedInfoBuildDefaultValueParameters();
            var serialized_info = MakeSerializedInfoDefaultValidParameters();
            Info? info = null;

            Assert.That
            (
                () =>
                {
                    info = serialized_info.Build
                    (
                        build_parameters.ContactWithCollectableLost,
                        build_parameters.ContactWithStorageHappened
                    );
                },
                Throws.Nothing
            );

            Assert.That
            (
                info,
                Is.Not.Null
            );
        }
    }
}