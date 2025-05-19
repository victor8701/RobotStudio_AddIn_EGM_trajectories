using ABB.Robotics.Controllers.MotionDomain;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio.Stations;
using System;

using TFG_offline.MATHEMATICS;
using ABB.Robotics.RobotStudio;
using RobotStudio.API.Internal;
using System.Linq;

namespace  TFG_offline.Targets
{
    public class Target
    {
        private string _name = " ";

        public enum motion_type { MoveJ, MoveL, MoveJDO, MoveLDO, MoveJAO, MoveLAO, MoveJGO, MoveLGO, TriggJ, TriggL, TriggJIOs, TriggLIOs, MoveAbsJ, MoveExtJ , empty};
        public enum speed_data { v5, v10, v20, v30, v40, v50, v60, v80, v100, v150, v200, v300, v400, v500, v600, v800, v1000, v1500, v2000, v2500, v3000, v4000, v5000, v6000, v7000 };
        public enum zone_data { fine, z0, z5, z10, z15, z20, z30, z40, z50, z60, z80, z100, z150, z200 };

        private double _x;
        private double _y;
        private double _z;
        private double _qw;
        private double _qx;
        private double _qy;
        private double _qz;
        private motion_type _type;
        private speed_data _speed;
        private zone_data _zone;

        public string name { get => _name; set => _name = value; }
        public double x { get => _x; set => _x = value; }
        public double y { get => _y; set => _y = value; }
        public double z { get => _z; set => _z = value; }
        public double qw { get => _qw; set => _qw = value; }
        public double qx { get => _qx; set => _qx = value; }
        public double qy { get => _qy; set => _qy = value; }
        public double qz { get => _qz; set => _qz = value; }
        public motion_type type { get => _type; set => _type = value; }
        public speed_data speed { get => _speed; set => _speed = value; }
        public zone_data zone { get => _zone; set => _zone = value; }

        public void GetTargetFromRsRobTarget(RsRobTarget RsTarget)
        {
            _name = RsTarget.Name;
            _x = RsTarget.Frame.X * 1000;
            _y = RsTarget.Frame.Y * 1000;
            _z = RsTarget.Frame.Z * 1000;

            double rx = RsTarget.Frame.RX;
            double ry = RsTarget.Frame.RY;
            double rz = RsTarget.Frame.RZ;

            double a = Globals.RadToDeg(rx);
            double b = Globals.RadToDeg(ry);
            double c = Globals.RadToDeg(rz);
            Vector3 vector3 = new Vector3(rx, ry, rz);
            Quaternion quaternion = Transforms.ToQuaternion(vector3);

            _qw = Approximation.ToZero(quaternion.q1);
            _qx = Approximation.ToZero(quaternion.q2);
            _qy = Approximation.ToZero(quaternion.q3);
            _qz = Approximation.ToZero(quaternion.q4);
        }

        public void GetEnums(RsMoveInstruction moveInstruction)
        {
            _type = motType.UsingString(moveInstruction.GetMotionType().ToString());

            var speedArg = moveInstruction.GetArgumentsByDataType("speeddata").SingleOrDefault();
            string speedStringValue = speedArg.Value; // v10, v100, etc
            if (Enum.TryParse(speedStringValue, true, out speed_data parsedSpeed))
            {
                _speed = parsedSpeed;
            }
            else
            {
                Logger.AddMessage(new LogMessage($"Target.GetEnums: SpeedData '{speedStringValue}' no reconocido para el enum speed_data. Target: {_name}", LogMessageSeverity.Warning));
            }

            var zoneArg = moveInstruction.GetArgumentsByDataType("zonedata").SingleOrDefault();
            string zoneStringValue = zoneArg.Value; // fine, z10, etc
            if (Enum.TryParse(zoneStringValue, true, out zone_data parsedZone))
            {
                _zone = parsedZone;
            }
            else
            {
                Logger.AddMessage(new LogMessage($"Target.GetEnums: ZoneData '{zoneStringValue}' no reconocido para el enum zone_data. Target: {_name}", LogMessageSeverity.Warning));
            }
        }
    }
}
