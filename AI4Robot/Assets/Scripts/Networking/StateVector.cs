using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Networking
{
    [Serializable]
    public class StateVector
    {
        public Rotations Rotations;
        public EndEffectorPosition EndEffectorPosition;

        public StateVector(Vector3 rotations, Vector3 endEffectorPosition)
        {
            this.Rotations = new Rotations(
                rotations.x,
                rotations.y,
                rotations.z
            );

            this.EndEffectorPosition = new EndEffectorPosition(
                endEffectorPosition.x,
                endEffectorPosition.y,
                endEffectorPosition.z
            );
        }
    }


    [Serializable]
    public class Rotations
    {
        public float linkOne;
        public float linkTwo;
        public float linkThree;

        public Rotations(float linkOne, float linkTwo, float linkThree)
        {
            this.linkOne    = linkOne;
            this.linkTwo    = linkTwo;
            this.linkThree  = linkThree;
        }
    }


    [Serializable]
    public class EndEffectorPosition
    {
        public float x;
        public float y;
        public float z;

        public EndEffectorPosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
