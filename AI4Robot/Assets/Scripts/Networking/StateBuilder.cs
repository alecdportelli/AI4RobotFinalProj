using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Robots;

namespace Networking
{
    public class StateBuilder
    {

        public StateBuilder()
        {
            
        }

        public byte[] CollectRobotState( Phugiod7 robot )
        {
            Vector3 currRotations = robot.GetRotationState();
            Vector3 currEndEffectorPos = robot.GetEndEffectorPosition();

            StateVector stateVector = new StateVector(
                currRotations,
                currEndEffectorPos
            );

            string stateJson = JsonUtility.ToJson(stateVector);
            byte[] stateBytes = System.Text.Encoding.UTF8.GetBytes(stateJson);

            return stateBytes;
        }


        public byte[] CollectTargetPosition( Vector3 position, int xPng, int yPng )
        {
            TargetPosition targetPosition = new TargetPosition
            (
                position, xPng, yPng
            );

            string stateJson = JsonUtility.ToJson(targetPosition);
            byte[] stateBytes = System.Text.Encoding.UTF8.GetBytes(stateJson);

            return stateBytes;
        }
    }
}
