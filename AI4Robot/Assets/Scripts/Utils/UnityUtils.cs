using System;
using UnityEngine;

namespace Utils
{
    public static class UnityUtils
    {
        private static float innerRadius = 5f;
        private static float outerRadius = 10f;


        public static Vector2[] GenerateTargets( int numTargets )
        {
            Vector2[] points = new Vector2[numTargets];

            for (int i = 0; i < numTargets; i++)
            {
                points[i] = GeneratePointInAnnularRegion(innerRadius, outerRadius);
            }

            return points;
        }


        private static Vector2 GeneratePointInAnnularRegion(float innerRadius, float outerRadius)
        {
            // Generate a random angle in radians
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

            // Generate a random radius between innerRadius and outerRadius
            float radius = Mathf.Sqrt(UnityEngine.Random.Range(innerRadius * innerRadius, outerRadius * outerRadius));

            // Convert polar coordinates to Cartesian coordinates
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            return new Vector2(x, y);
        }
    }
}
