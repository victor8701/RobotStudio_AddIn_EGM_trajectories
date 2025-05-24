using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RobotStudioEmptyAddin1_16nov.Paths
{
    internal class CreatePath
    {
        private static int _numPath;
        public static int numPath
        {
            get { return _numPath; }
            set { _numPath = value; }
        }
        public static List<RsPathProcedure> CreatedPaths { get; private set; } = new List<RsPathProcedure>();
        public static void createPath(List<RsTarget> ListOfTargets)
        {
            // Begin UndoStep
            Project.UndoContext.BeginUndoStep("RsPathProcedure Create");
            try
            {
                if (ListOfTargets != null && ListOfTargets.Count > 0)
                {
                    // Get the active station
                    Station station = Station.ActiveStation;

                    // Create a path procedure
                    RsPathProcedure myPath = new RsPathProcedure("myPath_" + (_numPath + 1));

                    // Add the path to the active task and configure it
                    station.ActiveTask.PathProcedures.Add(myPath);

                    //Station.ActiveStation.ActiveTask.PathProcedures.Add(myPath);
                    myPath.ModuleName = "module1";
                    myPath.ShowName = true;
                    myPath.Synchronize = true;
                    myPath.Visible = true;

                    // Create path through all targets in the active task
                    foreach (RsTarget target in ListOfTargets)
                    {
                        RsMoveInstruction moveInstruction = new ABB.Robotics.RobotStudio.Stations.RsMoveInstruction(
                        station.ActiveTask,
                        "Move",
                        "Default",
                        MotionType.Linear,
                        station.ActiveTask.ActiveWorkObject.Name,
                        target.Name,
                        station.ActiveTask.ActiveTool.Name);

                        // Add each move instruction to the path procedure
                        myPath.Instructions.Add(moveInstruction);
                    }
                    //CreateTarget.CreatedTargets.Add(new List<RsTarget>());
                    CreatedPaths.Add(myPath);

                    /////////
                    if (/*myPath*/ CreatedPaths[_numPath] != null && Station.ActiveStation.ActiveTask.PathProcedures.Contains(/*myPath*/ CreatedPaths[_numPath]))
                    {
                        Logger.AddMessage(new LogMessage(CreatedPaths[_numPath].Name + $" Existe"));
                    }
                    else
                    {
                        Logger.AddMessage(new LogMessage($"No existe"));
                    }
                    /////////

                    CustomBtn_2.addCmbBox2(/*myPath*/);
                    _numPath++;
                    Logger.AddMessage(new LogMessage("Se creo el path " + (_numPath)));
                }
                else Logger.AddMessage(new LogMessage("The selected List is empty"));
            }
            catch
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                throw;
            }
            finally
            {
                Project.UndoContext.EndUndoStep();
            }
        }
    }
}
