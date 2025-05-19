using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;
//using TFG_offline.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_offline.Targets;
using ABB.Robotics.Controllers.RapidDomain;


namespace  TFG_offline.Paths
{
    public class Path_list
    {
        public static List<RsPathProcedure> MyPaths { get; private set; } = new List<RsPathProcedure>();
        public static List<List<Target>> TargetsFromPath { get; private set; } = new List<List<Target>>();
        //public static List<List<RsRobTarget>> RSTargetsFromPath { get; private set; } = new List<List<RsRobTarget>>();
        public static List<List<RsTarget>> RSTargetsFromPath { get; private set; } = new List<List<RsTarget>>();
        public static Station station = Station.ActiveStation;
        public static void GetStationPaths()
        {
            for (int i = 0; i < station.ActiveTask.PathProcedures.Count; i++)
            {
                RsPathProcedure path = station.ActiveTask.PathProcedures[i];
                MyPaths.Add(path);
                List<Target> targetList = new List<Target>();

                string pathName = path.Name;

                Logger.AddMessage(new LogMessage("Path " + (i+1) + ": " + pathName + " added."));

                if (path != null)
                {
                    // Get RsTask object reference from path procedure
                    if (path.Parent is RsTask task)
                    {
                        // Iterate over every instructions in the path procedure 
                        foreach (var instruction in path.Instructions)
                        {
                            if (instruction is RsMoveInstruction moveInstruction)
                            {
                                // Get robtarget from move instruction
                                string strToPoint = moveInstruction.GetToPointArgument().Value;
                                RsRobTarget robTarget =
                                    task.FindDataDeclarationFromModuleScope(strToPoint, path.ModuleName)
                                    as RsRobTarget;

                                if (robTarget != null) {

                                    Target target = new Target();

                                    target.GetTargetFromRsRobTarget(robTarget);

                                    target.GetEnums(moveInstruction);

                                    targetList.Add(target);
                                }
                                else Logger.AddMessage(new LogMessage("target is null for: " + strToPoint + " in module: " + path.ModuleName));
                            }
                        }
                    }
                }
                TargetsFromPath.Add(targetList);
            }
            /*int a = 0;
            foreach (var targetList in TargetsFromPath)
            {
                a++;
                Logger.AddMessage(new LogMessage("//////////////////////"));
                Logger.AddMessage(new LogMessage("List " + a ));
                foreach (var target in targetList)
                {
                    Logger.AddMessage(new LogMessage(target.name + " -> x: " + target.x + ", y: " + target.y + ", z: " + target.z + 
                        ", qw: " + target.qw + ", qx: " + target.qx + ", qy: " + target.qy + ", qz: " + target.qz +
                        ", type: " + target.type + ", speed: " + target.speed + ", zone: " + target.zone));
                }
                Logger.AddMessage(new LogMessage("####################"));
            }*/
        }
    }
}
