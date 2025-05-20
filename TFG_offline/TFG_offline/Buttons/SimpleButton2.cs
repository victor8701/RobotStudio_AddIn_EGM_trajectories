using TFG_offline.Buttons;
using TFG_offline.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_offline.Files;
using TFG_offline.Controller;
using ABB.Robotics.RobotStudio;

namespace TFG_offline.Buttons
{
    internal class SimpleButton3
    {
        public static void Pressed()
        {
                Path_list.GetStationPaths();
        }
    }
}
