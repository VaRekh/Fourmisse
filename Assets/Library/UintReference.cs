﻿using System;
using UnityEngine;

namespace Assets.Library
{

    [Serializable]
    public class UintReference
    {
        [SerializeField]
        public uint Value;

        public UintReference(uint value)
        {
            Value = value;
        }
    }
}