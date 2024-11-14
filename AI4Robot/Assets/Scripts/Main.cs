using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object;
using Networking;

public class Main : MonoBehaviour
{
    public Camera cam;

    RoboCamera roboCamera;
    Client client;

    string ADDRESS = "127.0.0.1";
    int PORT = 65432;

    byte[] currImage;

    // Start is called before the first frame update
    void Start()
    {
        client = new Client( ADDRESS, PORT );
        client.StartClient();

        roboCamera = new RoboCamera( cam, 0, 10, 0, 90, 0, 0 );
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
    }
}
