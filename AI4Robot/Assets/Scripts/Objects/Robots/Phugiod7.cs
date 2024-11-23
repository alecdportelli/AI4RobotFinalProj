using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Robots
{
    public class Phugiod7
    {
        string name;

        Transform link1;
        Transform link2;
        Transform link3;
        Transform endEffector;

        TriggerDetection[] childTriggerChecks;

        GameObject instance;

        PID Link1PID;
        PID Link2PID;
        PID Link3PID;

        float xPos;
        float yPos;
        float zPos;

        float link1Rot;
        float link2Rot;
        float link3Rot;

        // Joint constraints 
        const float LINK_1_MIN = -180;
        const float LINK_1_MAX = 180;

        const float LINK_2_MIN = 0;
        const float LINK_2_MAX = 90;

        const float LINK_3_MIN = 0;
        const float LINK_3_MAX = 120;

        const float ROTATION_SPEED = 10f; // Degrees per second


        public Phugiod7(
            string name,

            GameObject phugoidPrefab,

            float xPos,
            float yPos,
            float zPos,

            float rot1,
            float rot2,
            float rot3
        )
        {
            this.name = name;

            this.xPos = xPos;
            this.yPos = yPos;
            this.zPos = zPos;

            instance = Object.Instantiate(phugoidPrefab);
            instance.transform.position = new Vector3(this.xPos, this.yPos, this.zPos);
            instance.name = this.name;

            link1 = instance.transform.Find("Link1");
            link2 = link1.Find("Link2");
            link3 = link2.Find("Link3");
            endEffector = link3.Find("EndEffector");

            SetLinkRotations( rot1, rot2, rot3 );

            // Check if all links are found
            if (link1 == null || link2 == null || link3 == null || endEffector == null)
            {
                Debug.LogError("One or more links could not be found in the robot prefab.");
            }

            // Get trigger checks within the instance of robot 
            childTriggerChecks = instance.GetComponentsInChildren<TriggerDetection>();

            // Build PIDs
            Link1PID = new PID( 1, 0, 0, LINK_1_MIN, LINK_1_MAX );
            Link2PID = new PID( 1, 0, 0, LINK_2_MIN, LINK_2_MAX );
            Link3PID = new PID( 1, 0, 0, LINK_3_MIN, LINK_3_MAX );

            // Set setpoints 
            Link1PID.SetSetpoint(0);
            Link2PID.SetSetpoint(0);
            Link3PID.SetSetpoint(0);
        }


        public void Update( float link1Setpoint, float link2Setpoint, float link3Setpoint )
        {
            Link1PID.SetSetpoint(link1Setpoint);
            Link2PID.SetSetpoint(link2Setpoint);
            Link3PID.SetSetpoint(link3Setpoint);

            Link1PID.Update(link1Rot, Time.deltaTime);
            Link2PID.Update(link2Rot, Time.deltaTime);
            Link3PID.Update(link3Rot, Time.deltaTime);

            // Compute control signals
            float link1Control = Link1PID.Update(link1Rot, Time.deltaTime);
            float link2Control = Link2PID.Update(link2Rot, Time.deltaTime);
            float link3Control = Link3PID.Update(link3Rot, Time.deltaTime);

            // Update rotations
            SetLink1Rotation(Mathf.MoveTowards(link1Rot, link1Rot + link1Control, ROTATION_SPEED * Time.deltaTime));
            SetLink2Rotation(Mathf.MoveTowards(link2Rot, link2Rot + link2Control, ROTATION_SPEED * Time.deltaTime));
            SetLink3Rotation(Mathf.MoveTowards(link3Rot, link3Rot + link3Control, ROTATION_SPEED * Time.deltaTime));
        }


        public float SetLink1Rotation( float rot )
        {
            rot = Mathf.Clamp(rot, LINK_1_MIN, LINK_1_MAX);
            link1.localEulerAngles = new Vector3(0, rot, 0);
            link1Rot = rot;
            return rot;
        }


        public float SetLink2Rotation( float rot )
        {
            rot = Mathf.Clamp(rot, LINK_2_MIN, LINK_2_MAX);
            link2.localEulerAngles = new Vector3(rot, 0, 0);
            link2Rot = rot;
            return rot;
        }


        public float SetLink3Rotation( float rot )
        {
            rot = Mathf.Clamp(rot, LINK_3_MIN, LINK_3_MAX);
            link3.localEulerAngles = new Vector3(rot, 0, 0);
            link3Rot = rot;
            return rot;
        }


        public void SetLinkRotations( float link1Rot, float link2Rot, float link3Rot )
        {
            SetLink1Rotation( link1Rot );
            SetLink2Rotation( link2Rot );
            SetLink3Rotation( link3Rot );
        }


        public float GetLink1Rotation()
        {
            return link1.localEulerAngles.y;
        }


        public float GetLink2Rotation()
        {
            return link2.localEulerAngles.x;
        }


        public float GetLink3Rotation()
        {
            return link3.localEulerAngles.x;
        }


        public Vector3 GetEndEffectorPosition()
        {
            return endEffector.transform.position;
        }


        public Vector3 GetRotationState()
        {
            return new Vector3(
                GetLink1Rotation(),
                GetLink2Rotation(),
                GetLink3Rotation()
            );
        }


        public bool CheckIsInContact()
        {
            // Iterate over the child TriggerCheck components and check if any are triggered
            foreach (var triggerCheck in childTriggerChecks)
            {
                if (triggerCheck.IsTriggered())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
