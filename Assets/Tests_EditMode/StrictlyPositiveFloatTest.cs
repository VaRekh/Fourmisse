#nullable enable
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using Assets.Library;

namespace Tests.Library
{
    public class StrictlyPositiveFloatTest
    {
        [Test]
        public void Constructor_SetZero_ThrowsArgument()
        {
            Assert.Throws<ArgumentException>
            (
                () =>
                {
                    var number = new StrictlyPositiveFloat(0f);
                }
            );
        }

        [TestCase(-1)]
        [TestCase(-2)]
        public void Constructor_SetStrictlyNegativeValue_ThrowsArgument(float value)
        {
            Assert.Throws<ArgumentException>
            (
                () =>
                {
                    new StrictlyPositiveFloat(value);
                }
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
