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

    // All have default values but can be changed through Unity editor
    // TODO: Build config file and launch script
    public string ADDRESS = "127.0.0.1";
    public int PORT = 65432;

    public int resWidth = 512;
    public int resHeight = 512;

    public float camXPos = 0;
    public float camYPos = 0;
    public float camZPos = 0;

    public float camYaw = 0;
    public float camPitch = 0;
    public float camRoll = 0;

    public int NUM_ROBOTS = 0;

    public float link1Rot = 0;
    public float link2Rot = 0;
    public float link3Rot = 0;

    byte[] currImage;
            
    // Start is called before the first frame update
    void Start()
    {
        client = new Client( ADDRESS, PORT );
        client.StartClient();

        roboCamera = new RoboCamera(
            cam,
            camXPos,
            camYPos,
            camZPos,
            camYaw,
            camPitch,
            camRoll,
            resWidth,
            resHeight
        );

        screenResCam.transform.position = new Vector3(camXPos, camYPos, camZPos);
        screenResCam.transform.eulerAngles = new Vector3(camYaw, camPitch, camRoll);
        p7 = new Phugiod7($"Robot1", phugoid7Prefab, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currImage = roboCamera.TakePicture();
            client.SendImage(currImage);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            link1Rot -= 1;
            link1Rot = p7.SetLink1Rotation(link1Rot);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            link1Rot += 1;
            link1Rot = p7.SetLink1Rotation(link1Rot);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            link2Rot -= 1;
            link2Rot = p7.SetLink2Rotation(link2Rot);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            link2Rot += 1;
            link2Rot = p7.SetLink2Rotation(link2Rot);
        }

        if (Input.GetKey(KeyCode.K))
        {
            link3Rot -= 1;
            link3Rot = p7.SetLink3Rotation(link3Rot);
        }

        if (Input.GetKey(KeyCode.M))
        {
            link3Rot += 1;
            link3Rot = p7.SetLink3Rotation(link3Rot);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currImage = roboCamera.TakePicture();
            client.SendImage(currImage);
        }
    }
}
