using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Library
{
    [Serializable]
    public class ControllerInfo
    {
        [SerializeField]
        private LayerReference anthill_layer;

        public LayerReference AnthillLayer
            => anthill_layer;
    }
}
