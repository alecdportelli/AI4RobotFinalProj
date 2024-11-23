using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SimulationObject;
using Robots;
using Networking;
using Utils;

public class GenerateImgs : MonoBehaviour
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

    public int NUM_POINTS = 1000;

    byte[] currImage;

    // Start is called before the first frame update
    void Awake()
    {
        client = new Client(ADDRESS, PORT);
        client.StartClient();

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


    private void Start()
    {
        StartCoroutine(GenerateAndSendImages(NUM_POINTS));
    }


    private IEnumerator GenerateAndSendImages(int numPoints)
    {
        // Generate all target points in advance
        Vector2[] points = UnityUtils.GenerateTargets(numPoints);

        for (int i = 0; i < points.Length; i++)
        {
            // Update target position
            target.transform.position = new Vector3(points[i].x, 0.5f, points[i].y);

            // Take a picture
            currImage = roboCamera.TakePicture();
            Tuple<int, int> pixels = roboCamera.GetPixelPosOfTarget(target);

            try
            {
                client.SendImage(currImage);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to send image {i}: {ex.Message}");
            }

            try
            {
                client.SendTargetPosition(target.transform.position, pixels.Item1, pixels.Item2);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to send target: {ex.Message}");
            }

            // Log progress
            if (i % 100 == 0)
            {
                Debug.Log($"Processed {i}/{numPoints} images...");
            }

            System.Threading.Thread.Sleep(1000);

            yield return null; // Wait until the next frame
        }

        Debug.Log("All images processed and sent!");
    }
}
