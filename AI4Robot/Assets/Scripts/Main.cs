using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimulationObject;
using Robots;
using Networking;

public class Main : MonoBehaviour
{
    public Camera cam;
    public Camera screenResCam;

    RoboCamera roboCamera;
    Client client;

    Phugiod7 p7;
    public GameObject phugoid7Prefab;

    public GameObject target;

    // All have default values but can be changed through Unity editor
    [Header("Simulation Parameters")]
    public string ADDRESS = "127.0.0.1";
    public int PORT = 65432;

    public int RES_WIDTH = 512;
    public int RES_HEIGHT = 512;

    public float camXPos = 0;
    public float camYPos = 0;
    public float camZPos = 0;

    public float camYaw = 0;
    public float camPitch = 0;
    public float camRoll = 0;

    public int NUM_ROBOTS = 1;

    public float link1Rot = 0;
    public float link2Rot = 0;
    public float link3Rot = 0;

    float link1Cmd;
    float link2Cmd;
    float link3Cmd;

    byte[] currImage;

    // Start is called before the first frame update
    void Awake()
    {
        client = new Client( ADDRESS, PORT);
        client.StartClient();
        client.OnDataReceived += HandleDataReceived;

        roboCamera = new RoboCamera(
            cam,
            camXPos,
            camYPos,
            camZPos,
            camYaw,
            camPitch,
            camRoll,
            RES_WIDTH,
            RES_HEIGHT
        );

        screenResCam.transform.position = new Vector3(camXPos, camYPos, camZPos);
        screenResCam.transform.eulerAngles = new Vector3(camYaw, camPitch, camRoll);

        p7 = new Phugiod7(
            "Robot1",
            phugoid7Prefab,

            0f,
            0f,
            0f,

            0f,
            0f,
            0f
        );
    }


    // This method will be called whenever data is received
    private void HandleDataReceived(LinkData linkData)
    {
        link1Cmd = linkData.Link1;
        link2Cmd = linkData.Link2;
        link3Cmd = linkData.Link3;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            client.SendStateVector( p7 );
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currImage = roboCamera.TakePicture();
            client.SendImage(currImage);
        }
    }


    private void FixedUpdate()
    {
        p7.Update( link1Cmd, link2Cmd, link3Cmd );
    }
}
