using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robots
{
    public class Phugiod7
    {
        string name;

        Transform link1;
        Transform link2;
        Transform link3;
        Transform endEffector;

        GameObject instance;

        float xPos;
        float yPos;
        float zPos;

        // Hard coded values - could be set through cnfg file
        float LINK_1_MIN = -90;
        float LINK_1_MAX = 90;

        float LINK_2_MIN = 0;
        float LINK_2_MAX = 90;

        float LINK_3_MIN = 0;
        float LINK_3_MAX = 90;


        public Phugiod7( string name, GameObject phugoidPrefab, float xPos, float yPos, float zPos )
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

            // Check if all links are found
            if (link1 == null || link2 == null || link3 == null || endEffector == null)
            {
                Debug.LogError("One or more links could not be found in the robot prefab.");
            }
        }


        public float SetLink1Rotation( float rot )
        {
            rot = Mathf.Clamp(rot, LINK_1_MIN, LINK_1_MAX);
            link1.localEulerAngles = new Vector3(0, rot, 0);
            return rot;
        }


        public float SetLink2Rotation( float rot )
        {
            rot = Mathf.Clamp(rot, LINK_2_MIN, LINK_2_MAX);
            link2.localEulerAngles = new Vector3(rot, 0, 0);
            return rot;
        }


        public float SetLink3Rotation( float rot )
        {
            rot = Mathf.Clamp(rot, LINK_3_MIN, LINK_3_MAX);
            link3.localEulerAngles = new Vector3(rot, 0, 0);
            return rot;
        }


        public void SetLinkRotations( float link1Rot, float link2Rot, float link3Rot )
        {
            SetLink1Rotation( link1Rot );
            SetLink2Rotation( link2Rot );
            SetLink3Rotation( link3Rot );
        }
    }
}
