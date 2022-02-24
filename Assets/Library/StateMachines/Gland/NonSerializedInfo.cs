#nullable enable
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Gland
{
    public struct NonSerializedInfo
    {
        public UnityEvent<Collectable> ContactWithCollectableLost { get; set; }
        public UnityEvent<Storage> ContactWithStorageHappened { get; set; }
        public UnityEvent<Collectable> ContactWithNonEmptyCollectableHappened { get; set; }
        public BoundedUint Load { get; set; }
        public UnityAction<GameObject, Identifier, uint> InitPheromone { get; set; }
        public Identifier Identifier { get; set; }

        public void CheckValidity()
        {
            Assert.IsNotNull(ContactWithCollectableLost);
            Assert.IsNotNull(ContactWithStorageHappened);
            Assert.IsNotNull(ContactWithNonEmptyCollectableHappened);
            Assert.IsNotNull(Load);
            Assert.IsNotNull(InitPheromone);
        }
    }
}
