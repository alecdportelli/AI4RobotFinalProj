using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.IO;

namespace SimulationObject
{
    public class RoboCamera
    {
        Camera cam;
        float x;
        float y;
        float z;

        float yaw;
        float pitch;
        float roll;

        int resWidth;
        int resHeight;

        public RoboCamera(
            Camera cam,
            float x,
            float y,
            float z,

            float yaw,
            float pitch,
            float roll,

            int resWidth,
            int resHeight
        )
        {
            this.resHeight = resHeight;
            this.resWidth = resWidth;
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);

            this.cam = cam;
            this.cam.targetTexture = rt;

            this.x = x;
            this.y = y;
            this.z = z;

            this.yaw = yaw;
            this.pitch = pitch;
            this.roll = roll;

            this.cam.transform.position = new Vector3(x, y, z);
            this.cam.transform.eulerAngles = new Vector3(yaw, pitch, roll);
        }

        public byte[] TakePicture()
        {
            // Capture image from the camera
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;

            cam.Render();

            Texture2D texture = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            texture.Apply();
            RenderTexture.active = activeRenderTexture;

            // Encode the image to PNG
            byte[] imageBytes = texture.EncodeToPNG();
            RenderTexture.active = null;  // Reset active texture
            return imageBytes;
        }


        public Tuple<int, int> GetPixelPosOfTarget(GameObject target)
        {
            // Get the cube's screen-space position
            Vector3 targetPosition = target.transform.position;
            Vector3 screenPos = this.cam.WorldToScreenPoint(targetPosition);

            int pixelX = Mathf.RoundToInt(screenPos.x);
            int pixelY = Mathf.RoundToInt(screenPos.y);
            
            int xPNG = pixelX;              // X-coordinate stays the same
            int yPNG = resHeight - pixelY;  // Convert Y-coordinate to top-left origin
 
            return new Tuple<int, int>(xPNG, yPNG);
        }
    }
}