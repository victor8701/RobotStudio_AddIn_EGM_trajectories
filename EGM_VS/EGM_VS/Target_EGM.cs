using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGM_VS
{
    internal class Target_EGM
    {
        private double _x;
        private double _y;
        private double _z;
        private double _qw;
        private double _qx;
        private double _qy;
        private double _qz;
        public double x { get => _x; set => _x = value; }
        public double y { get => _y; set => _y = value; }
        public double z { get => _z; set => _z = value; }
        public double qw { get => _qw; set => _qw = value; }
        public double qx { get => _qx; set => _qx = value; }
        public double qy { get => _qy; set => _qy = value; }
        public double qz { get => _qz; set => _qz = value; }
    }
}
