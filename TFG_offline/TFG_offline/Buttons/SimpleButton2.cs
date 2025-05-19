using TFG_offline.Controller;
using TFG_offline.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  TFG_offline.Buttons
{
    internal class SimpleButton2
    {
        public static void Pressed()
        {
            LoadController.Load();
        }
    }
}
