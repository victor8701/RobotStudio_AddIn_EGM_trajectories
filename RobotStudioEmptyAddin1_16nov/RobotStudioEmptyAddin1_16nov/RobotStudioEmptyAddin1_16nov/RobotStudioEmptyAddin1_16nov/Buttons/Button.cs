using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio;
using RobotStudioEmptyAddin1_16nov;

namespace RobotStudioEmptyAddin_Dia0
{
    public abstract class CommandBarControl
    {
        private string _id;
        private string _caption;

        protected CommandBarControl(string id, string caption)
        {
            _id = id;
            _caption = caption;
        }
        public string Caption { get; set; }
        public string Id { get; set; }
        public Image Image { get; set; }
        public object Tag { get; set; }

        private void Button_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
        {
            Console.WriteLine("¡Botón pulsado!!");
        }
    }
}