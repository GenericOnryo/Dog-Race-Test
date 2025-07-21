using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class DogVectors
    {
        public static Vector2 GetRandomVector2RestrictByNormal(float targetMagnitude, Vector2 collideNormal)
        {
            var randDirect = Random.Range(-85, 85);
            return RotateVector2ByDegrees(collideNormal.normalized, randDirect) * targetMagnitude;
        }

        // Helper function to rotate a Vector2 by an angle in degrees
        public static Vector2 RotateVector2ByDegrees(Vector2 vector, float angleInDegrees)
        {
            float angleRadians = angleInDegrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRadians);
            float sin = Mathf.Sin(angleRadians);
            return new Vector2(
                vector.x * cos - vector.y * sin,
                vector.x * sin + vector.y * cos
            );
        }

        public static Vector2 GetRandomVector2(float targetMagnitude)
        {
            Vector2 randDirect = Random.insideUnitCircle;

            Vector2 randDirectnormalized = randDirect.normalized;

            return randDirectnormalized * targetMagnitude;
        }



    }
}
