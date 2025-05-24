using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio.Stations.Forms;
using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotStudioEmptyAddin1_16nov.Targets;
using RobotStudioEmptyAddin1_16nov.Paths;

namespace RobotStudioEmptyAddin1_16nov
{
    internal class PickTarget
    {
        public static void PickTargets(bool p)
        {
            //Begin UndoStep
            Project.UndoContext.BeginUndoStep("MultipleTarget");
            try
            {
                    if (p)
                    {
                        //Initialize GraphicPicker
                        Logger.AddMessage("Selecciona donde quieres crear targets");
                        GraphicPicker.GraphicPick += new GraphicPickEventHandler(GraphicPicker_GraphicPick);
                    }
                    else
                    {
                        //delete GraphicPicker
                        Logger.AddMessage("Opción Pick Targets no seleccionada");
                        GraphicPicker.GraphicPick -= new GraphicPickEventHandler(GraphicPicker_GraphicPick);
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
        static void GraphicPicker_GraphicPick(object sender, GraphicPickEventArgs e)
        {
            //Begin UndoStep
            Station station = Project.ActiveProject as Station;
            string stepName = station.ActiveTask.GetValidRapidName("Target", "_", 10);
            Project.UndoContext.BeginUndoStep(stepName);

            try
            {
                ShowTarget(e.PickedPosition);
            }
            catch (Exception exception)
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage(exception.Message.ToString()));
            }
            finally
            {
                //End UndoStep
                Project.UndoContext.EndUndoStep();
            }
        }
        public static void ShowTarget(Vector3 position)
        {
            try
            {
                //get the active station
                Station station = Project.ActiveProject as Station;

                //create robtarget
                RsRobTarget robTarget = new RsRobTarget();
                robTarget.Name = station.ActiveTask.GetValidRapidName("Target", "_", 10);

                //translation
                robTarget.Frame.Translation = position;

                //add robtargets to datadeclaration
                station.ActiveTask.DataDeclarations.Add(robTarget);

                //create target
                RsTarget target = new RsTarget(station.ActiveTask.ActiveWorkObject, robTarget);
                //CAMBIOOO :
                /*ListOfTargets.addTarget(target);*/ CreateTarget.CreatedTargets[CustomBtn_2.selectedList-1].Add(target);
                target.Name = robTarget.Name;
                target.Attributes.Add(target.Name, true);

                //add targets to active task
                station.ActiveTask.Targets.Add(target);

                bool cop = PopUpBtn.copy;

                if(cop) CopyTarget.CopiedTargets(target);
            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.Message.ToString()));
            }
        }
    }
}
