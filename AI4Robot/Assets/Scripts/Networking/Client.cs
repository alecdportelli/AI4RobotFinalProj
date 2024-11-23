using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;
using Robots;
using Utils;

namespace Networking
{
    public class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private StateBuilder stateBuilder;

        private string address;
        private int port;

        private Thread receiveThread;
        private bool isRunning;

        public event Action<LinkData> OnDataReceived;


        public Client( string address, int port )
        {
            this.address = address;
            this.port = port;

            stateBuilder = new StateBuilder();
        }


        public void StartClient()
        {
            ConnectToPython();
        }


        void ConnectToPython()
        {
            try
            {
                client = new TcpClient(this.address, this.port);
                stream = client.GetStream();
                Debug.Log("Connected to Python!");

                // Start receiving data from Python asynchronously
                isRunning = true;
                receiveThread = new Thread(ReceiveData);
                receiveThread.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("Socket connection failed: " + e.Message);
            }
        }


        private void ReceiveData()
        {
            while (isRunning)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        // Process received data (e.g., if response from Python)
                        string response = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        // Deserialize the JSON string into a LinkData object
                        LinkData linkData = JsonUtility.FromJson<LinkData>(response);

                        // Trigger the event with the received data
                        OnDataReceived?.Invoke(linkData);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error while receiving data: " + e.Message);
                    break; // Exit the loop if there is an error
                }
            }
        }


        public void SendImage( byte[] imageBytes )
        {
            // Send header byte for image (1)
            byte[] header = new byte[] { Constants.IMAGE_DATA_TYPE };

            // Send the header (image data type) first
            stream.Write(header, 0, header.Length);

            // Send image size
            byte[] sizeBytes = BitConverter.GetBytes(imageBytes.Length);
            stream.Write(sizeBytes, 0, sizeBytes.Length);
            stream.Write(imageBytes, 0, imageBytes.Length);
        }


        public void SendStateVector( Phugiod7 p7 )
        {
            // Get the state bytes from the state builder
            byte[] stateBytes = stateBuilder.CollectRobotState( p7 );

            // Send header byte for state vector (2)
            byte[] header = new byte[] { Constants.STATE_VEC_DATA_TYPE };
            stream.Write(header, 0, header.Length);

            // Send size of state vector JSON
            byte[] sizeBytes = BitConverter.GetBytes(stateBytes.Length);
            stream.Write(sizeBytes, 0, sizeBytes.Length);
            stream.Write(stateBytes, 0, stateBytes.Length);
        }


        public void SendTargetPosition( Vector3 targetPos, int xPixel, int yPixel )
        {
            // Get the state bytes from the state builder
            byte[] stateBytes = stateBuilder.CollectTargetPosition(targetPos, xPixel, yPixel);

            // Send header byte for target position (3)
            byte[] header = new byte[] { Constants.TARGET_POSITION_TYPE };
            stream.Write(header, 0, header.Length);

            // Send size of state vector JSON
            byte[] sizeBytes = BitConverter.GetBytes(stateBytes.Length);
            stream.Write(sizeBytes, 0, sizeBytes.Length);
            stream.Write(stateBytes, 0, stateBytes.Length);
        }


        public void SendDataType( byte type )
        {
            // Send just the data type as a single byte
            byte[] header = new byte[] { type };
            stream.Write(header, 0, header.Length);
        }


        void OnApplicationQuit()
        {
            isRunning = false;
            receiveThread?.Join(); // Ensure the thread finishes before closing the socket
            stream?.Close();
            client?.Close();
        }
    }
}
