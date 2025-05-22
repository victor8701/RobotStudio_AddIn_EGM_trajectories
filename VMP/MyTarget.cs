using ABB.Robotics.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abb.Egm;
namespace TFG_Proyecto_Solucion
{
    public static class MyTarget
    {
        private static double _x, _xprevious, _xactual;
        private static double _y, _yprevious, _yactual;
        private static double _z, _zprevious, _zactual;
        private static double _qw;
        private static double _qx;
        private static double _qy;
        private static double _qz;
 
        private static EgmQuaternion _quaternion = new EgmQuaternion();
        private static EgmQuaternion _quatPrevious = new EgmQuaternion();

        public static double xprevious { get => _xprevious; set => _xprevious = value; }
        public static double yprevious { get => _yprevious; set => _yprevious = value; }
        public static double zprevious { get => _zprevious; set => _zprevious = value; }
        public static double xactual { get => _xactual; set => _xactual = value; }
        public static double yactual { get => _yactual; set => _yactual = value; }
        public static double zactual { get => _zactual; set => _zactual = value; }
        public static double x { get => _x; set => _x = value; }
        public static double y { get => _y; set => _y = value; }
        public static double z { get => _z; set => _z = value; }
        public static double qw { get => _qw; set => _qw = value; }
        public static double qx { get => _qx; set => _qx = value; }
        public static double qy { get => _qy; set => _qy = value; }
        public static double qz { get => _qz; set => _qz = value; }

        public static EgmQuaternion quaternion { get => _quaternion; set => _quaternion = value; }
    }
}
