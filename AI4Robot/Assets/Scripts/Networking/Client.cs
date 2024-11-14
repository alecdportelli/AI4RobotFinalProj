using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Networking
{
    public class Client
    {
        private TcpClient client;
        private NetworkStream stream;

        private string address;
        private int port;

        public Client( string address, int port )
        {
            this.address = address;
            this.port = port;
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
            }
            catch (Exception e)
            {
                Debug.LogError("Socket connection failed: " + e.Message);
            }
        }

        public void SendImage( byte[] imageBytes )
        {
            // Send image size first (header)
            byte[] sizeBytes = BitConverter.GetBytes(imageBytes.Length);
            stream.Write(sizeBytes, 0, sizeBytes.Length);

            // Send the actual image data
            stream.Write(imageBytes, 0, imageBytes.Length);
        }

        void OnApplicationQuit()
        {
            stream?.Close();
            client?.Close();
        }
    }
}
