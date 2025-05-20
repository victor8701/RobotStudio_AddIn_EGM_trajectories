using ABB.Robotics.RobotStudio;
using TFG_offline.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_offline.Controller;

namespace  TFG_offline.Buttons
{
    internal class SimpleButton1
    {
        private static bool firstTime = true;
        public static void Pressed()
        {
            if (firstTime)
            {
                LoadController.Load();
                firstTime = false;
            }
            Logger.AddMessage(new LogMessage("Button 1 pressed"));
            string s = @"C:\AAAA\path_10.csv";
            LoadFile.Load(s);
        }
    }
}
