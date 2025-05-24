using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotStudioEmptyAddin1_16nov.Targets
{
    internal class PropertiesTarget
    {
        public static void UpdateTargetProperties(List<RsTarget> existingTargets, bool visibility) //Antes el tipo de existingTargets era RsTargetCollection
        {
            //Begin UndoStep
            Project.UndoContext.BeginUndoStep("UpdateProperties");
            try
            {
                Station station = Project.ActiveProject as Station;

                foreach (RsTarget target in existingTargets)
                {
                    //Setting the Visible property of target would show or hide the targets
                    //SHOW targets
                    target.Visible = visibility;
                    /*
                    //Set target to LOCAL
                    target.RobTarget.Local = true;
                    //When making target to LOCAL, target module name and PathProcedure module name must be same 
                    target.RobTarget.ModuleName = "Module1";

                    // Jump to the target. Requires an active tool in the station and a running VC.
                    target.JumpTo(station.ActiveTask.ActiveTool);

                    //Setting the Visible property of target would show or hide the targets
                    //HIDING targets
                    target.Visible = false;*/
                }
            }
            catch (Exception ex)
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage(ex.Message.ToString()));
            }
            finally
            {
                //End UndoStep
                Project.UndoContext.EndUndoStep();
            }
        }
    }
}
