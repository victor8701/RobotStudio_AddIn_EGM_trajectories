using ABB.Robotics.Controllers;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  TFG_offline.Targets
{
    internal class motType
    {
        public static string UsingTarget(Target target)
        {
            string mType = " ";
            
            string a = target.type.ToString();

            if (a == "MoveJ" || a == "MoveJDO" || a == "MoveJAO" || a == "MoveJGO" || a == "MoveAbsJ" || a == "MoveExtJ") mType = "Joint";
            else if (a == "MoveL" || a == "MoveLDO" || a == "MoveLAO" || a == "MoveLGO") mType = "Linear";

            return mType;
        }

        public static Target.motion_type UsingString(string mType)
        {
            Target.motion_type type = Target.motion_type.empty; // Valor para inicializar

            if (mType == "Linear") type = Target.motion_type.MoveL;
            else if (mType == "Joint") type = Target.motion_type.MoveJ;

            return type;
        }

        public static string DeleteMove(Target target) //Quitar la palabra "Move" al principio de speed_data para introducirlo en el .csv
        {
            string typeString = target.type.ToString();
            if (typeString.StartsWith("Move")) 
            {
                typeString = typeString.Substring(4);
            }
            else Logger.AddMessage(new LogMessage("No hay 'Move' en " + typeString));

            return typeString;
        }
    }
}
