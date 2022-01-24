#nullable enable
using NUnit.Framework;
using UnityAssertionException = UnityEngine.Assertions.AssertionException;
using Assets.Library;

namespace Tests.Library
{
    public class StrictlyPositiveFloatTest
    {
        [Test]
        public void Constructor_SetZero_ThrowsArgument()
        {
            Assert.That
            (
                () => { new StrictlyPositiveFloat(0f); },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was False")
            );
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public void Constructor_SetStrictlyNegativeValue_ThrowsArgument(float value)
        {
            Assert.That
            (
                () => { new StrictlyPositiveFloat(value); },
                Throws.InstanceOf<UnityAssertionException>()
                        .With.Message.Contains("Value was False")
            );
        }

        [TestCase(2f)]
        [TestCase(1f)]
        public void Constructor_SetStrictlyPositiveValue_DoesNotThrow(float value)
        {
            Assert.DoesNotThrow
            (
                () =>
                {
                    new StrictlyPositiveFloat(value);
                }
            );
        }

        [TestCase(1f)]
        [TestCase(2f)]
        public void GetValue_SetNonZeroValue_ReturnSameValue(float expected)
        {
            var number = new StrictlyPositiveFloat(expected);
            var actual = number.Value;
            Assert.AreEqual(expected, actual);
        }
    }
}
