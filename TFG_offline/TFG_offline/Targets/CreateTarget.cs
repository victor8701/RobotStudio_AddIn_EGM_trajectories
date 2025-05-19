using ABB.Robotics.Controllers.MotionDomain;
using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;
using RobotStudio.API.Internal;
using TFG_offline.Controller;
using TFG_offline.Paths;
using TFG_offline.MATHEMATICS;
using System;
using System.Collections.Generic;
using System.Numerics;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using TFG_offline.PATHS;

namespace  TFG_offline.Targets
{
    class CreateTarget
    {
        private static Station station;
        public static List<Target> MyTargets { get; private set; } = new List<Target>();
        public static List<RsRobTarget> MyRsRobTargets { get; private set; } = new List<RsRobTarget>();
        public static void Create(int position)
        {
            station = LoadController.station_public;

            //Create a new RobTarget
            RsRobTarget target = new RsRobTarget();

            target.Name = station.ActiveTask.GetValidRapidName("myRobTarget", "_", 10);

            //Copy RSRobtarg name to Target name
            MyTargets[position].name = target.Name;

            target.Frame.X = MyTargets[position].x;
            target.Frame.Y = MyTargets[position].y;
            target.Frame.Z = MyTargets[position].z;

            Quaternion q = new Quaternion();
            q.q1 = MyTargets[position].qw;
            q.q2 = MyTargets[position].qx;
            q.q3 = MyTargets[position].qy;
            q.q4 = MyTargets[position].qz;

            Vector3 v3 = Transforms.ToEulerAngles(q);            
            target.Frame.RX = v3.x;
            target.Frame.RY = v3.y;
            target.Frame.RZ = v3.z;

            station.ActiveTask.DataDeclarations.Add(target);
            MyRsRobTargets.Add(target);

            //Create a graphic representation of the RobTarget and RsTarget.
            RsTarget myRsTarget = new RsTarget(LoadController.MyWobj_public, target);
            myRsTarget.Name = target.Name;
            station.ActiveTask.Targets.Add(myRsTarget);
        }
    }
}
