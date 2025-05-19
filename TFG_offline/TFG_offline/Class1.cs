using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using TFG_offline.Buttons;
using TFG_offline.Controller;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RobotStudioEmptyAddin_16nov
{
    public class Class1
    {
        // This is the entry point which will be called when the Add-in is loaded
        public static void AddinMain()
        {
            Logger.AddMessage(new LogMessage("Se ha abierto el Add-In TFG_offline!"));

            SimpleButton SimpB1 = new SimpleButton(true, 1, "Load .csv"); // (newTab, numButton, functionality)
            SimpB1.CreateButton();

            SimpleButton SimpB2 = new SimpleButton(false, 2, "Load Controller"); // (newTab, numButton, functionality)
            SimpB2.CreateButton();

            SimpleButton SimpB3 = new SimpleButton(false, 3, "Get paths from station"); // (newTab, numButton, functionality)
            SimpB3.CreateButton();

            //LoadController.robName = "IRB140_6_81_C_G_03.rslib";
            LoadController.robName = " ";
        }
    }
}