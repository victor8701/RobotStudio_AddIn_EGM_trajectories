using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RobotStudioEmptyAddin1_16nov.Paths;


namespace RobotStudioEmptyAddin1_16nov
{
    internal class CopyTarget
    {
        public static void CopiedTargets(RsTarget target)
        {
            Project.UndoContext.BeginUndoStep("Copy Target");
            try
            {
                Logger.AddMessage(new LogMessage("Se copiarán los targets"));

                // Instance active station.
                Station stn = Station.ActiveStation;

                // Get the active workobject.
                RsWorkObject wobj = stn.ActiveTask.ActiveWorkObject;

                // Create a new RobTarget and add it to the ActiveTask.
                RsRobTarget robTarget = new RsRobTarget();
                robTarget.Name = stn.ActiveTask.GetValidRapidName("MyTarget", "_", 10);
                stn.ActiveTask.DataDeclarations.Add(robTarget);

                // Create an RsTarget from wobj and RobTarget.
                /*RsTarget target = new RsTarget(wobj, robTarget);

                // Set the name of the target.
                target.Name = robTarget.Name;

                // Add RsTarget to ActiveTask.
                stn.ActiveTask.Targets.Add(target);
                */

                // Copy the target.
                RsTarget targetCopy = (RsTarget)target.Copy();

                // Set the new name to the copied targets.
                targetCopy.Name = target.Name + "_Copy";

                // Add the copied target, after the original target, to the ActiveTask.
                stn.ActiveTask.Targets.Add(targetCopy, target);

                // Move the new target 0.5 meters along the X-axis.
                targetCopy.Transform.X = targetCopy.Transform.X + 0.5;

                // Set the highlight color of the target.
                target.Highlight(Color.Green);

                // Reset the highlight color of the target copy.
                targetCopy.ResetHighlight();

                // Remove the original target.
                //stn.ActiveTask.Targets.Remove(target);

                CreateTarget.CreatedTargets[CustomBtn_2.selectedList-1].Add(targetCopy);
            }
            catch (Exception)
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
