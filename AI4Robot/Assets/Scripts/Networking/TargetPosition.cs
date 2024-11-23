using System;
using UnityEngine;

[Serializable]
public class TargetPosition
{
    public UnityPosition UnityPosition;
    public PixelPosition PixelPosition;

    public TargetPosition(Vector3 unityPosition, int pixelX, int pixelY)
    {
        this.UnityPosition = new UnityPosition(
            unityPosition.x,
            unityPosition.y,
            unityPosition.z
        );

        this.PixelPosition = new PixelPosition(
            pixelX,
            pixelY
        );
    }
}


[Serializable]
public class UnityPosition
{
    public float x;
    public float y;
    public float z;

    public UnityPosition(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}


[Serializable]
public class PixelPosition
{
    public int x;
    public int y;

    public PixelPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
