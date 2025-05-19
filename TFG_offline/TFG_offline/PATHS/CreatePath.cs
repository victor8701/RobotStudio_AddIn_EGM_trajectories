using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;
using TFG_offline.Controller;
using TFG_offline.Paths;
using TFG_offline.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace  TFG_offline.PATHS
{
    internal class CreatePath
    {
        private static Station station = Station.ActiveStation;
        private static int numPaths = 0;
        public static void Create(List <Target> listOfTargets)
        {
            numPaths++;

            // Create a path procedure
            RsPathProcedure myPath = new RsPathProcedure("myPath_" + numPaths*10);

            // Add the path to the active task and configure it
            station.ActiveTask.PathProcedures.Add(myPath);

            myPath.ModuleName = "module1";
            myPath.ShowName = true;
            myPath.Synchronize = true;
            myPath.Visible = true;

            // Create path through all targets in the active task
            foreach (Target target in listOfTargets)
            {
                string motionType = motType.UsingTarget(target);
                if ( motionType == "Linear")
                {
                    RsMoveInstruction moveInstruction = new ABB.Robotics.RobotStudio.Stations.RsMoveInstruction(
                    station.ActiveTask,
                    "Move",
                    "Default",
                    MotionType.Linear,
                    station.ActiveTask.ActiveWorkObject.Name,
                    target.name,
                    station.ActiveTask.ActiveTool.Name);

                    // Add each move instruction to the path procedure
                    myPath.Instructions.Add(moveInstruction); // Añadir la instruccion ANTES de copiar velocidad y zona

                    //Copy velocity from Target
                    var speed = moveInstruction.GetArgumentsByDataType("speeddata").SingleOrDefault();
                    speed.Value = target.speed.ToString();

                    //Copy zone from Target
                    var zone = moveInstruction.GetArgumentsByDataType("zonedata").SingleOrDefault();
                    zone.Value = target.zone.ToString();
                }
                else if (motionType == "Joint")
                {
                    RsMoveInstruction moveInstruction = new ABB.Robotics.RobotStudio.Stations.RsMoveInstruction(
                    station.ActiveTask,
                    "Move",
                    "Default",
                    MotionType.Joint,
                    station.ActiveTask.ActiveWorkObject.Name,
                    target.name,
                    station.ActiveTask.ActiveTool.Name);

                    // Add each move instruction to the path procedure
                    myPath.Instructions.Add(moveInstruction); // Añadir la instruccion ANTES de copiar velocidad y zona

                    //Copy velocity from Target
                    var speed = moveInstruction.GetArgumentsByDataType("speeddata").SingleOrDefault();
                    speed.Value = target.speed.ToString();

                    //Copy zone from Target
                    var zone = moveInstruction.GetArgumentsByDataType("zonedata").SingleOrDefault();
                    zone.Value = target.zone.ToString();
                }
                else Logger.AddMessage(new LogMessage("Error with Motion Type in CreatePath.cs"));
            }
            }
        }
}
