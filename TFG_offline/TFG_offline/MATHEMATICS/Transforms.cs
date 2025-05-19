using ABB.Robotics.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  TFG_offline.MATHEMATICS
{
    internal class Transforms
    {
        public static Vector3 ToEulerAngles(Quaternion q) //https://stackoverflow.com/questions/70462758/c-sharp-how-to-convert-quaternions-to-euler-angles-xyz
        {
            Vector3 angles = new Vector3();

            // roll / x
            double sinr_cosp = 2 * (q.q1 * q.q2 + q.q3 * q.q4);
            double cosr_cosp = 1 - 2 * (q.q2 * q.q2 + q.q3 * q.q3);
            angles.x = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.q1 * q.q3 - q.q4 * q.q2);
            if (Math.Abs(sinp) >= 1)
            {
                angles.y = (float)(Math.Sign(sinp) * (Math.PI / 2)); //angles.y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.q1 * q.q4 + q.q2 * q.q3);
            double cosy_cosp = 1 - 2 * (q.q3 * q.q3 + q.q4 * q.q4);
            angles.z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }
        public static Quaternion ToQuaternion(Vector3 v) //https://stackoverflow.com/questions/70462758/c-sharp-how-to-convert-quaternions-to-euler-angles-xyz
        {

            float cy = (float)Math.Cos(v.x * 0.5);
            float sy = (float)Math.Sin(v.z * 0.5);
            float cp = (float)Math.Cos(v.y * 0.5);
            float sp = (float)Math.Sin(v.y * 0.5);
            float cr = (float)Math.Cos(v.x * 0.5);
            float sr = (float)Math.Sin(v.x * 0.5);

            return new Quaternion
            {
                q1 = (cr * cp * cy + sr * sp * sy), //W
                q2 = (sr * cp * cy - cr * sp * sy), //X
                q3 = (cr * sp * cy + sr * cp * sy), //Y
                q4 = (cr * cp * sy - sr * sp * cy)  //Z
            };

        }
    }
}
