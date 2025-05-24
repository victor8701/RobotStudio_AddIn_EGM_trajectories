using ABB.Robotics.Math;
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

    internal class CreateTarget
    {
        // Lista estática para almacenar los Targets creados
        //public static List<RsTarget> CreatedTargets { get; private set; } = new List<RsTarget>(); //antes de hacer vector de Lists of targets
        public static List<List<RsTarget>> CreatedTargets { get; private set; } = new List<List<RsTarget>>();

        /*public static void AddList()
        {
            CreatedTargets.Add(new List<RsTarget>());
        }*/

        public static void CreateTargets()
        {
            Project.UndoContext.BeginUndoStep("CreateTarget");
            try
            {
                // Crear el primer target y añadirlo a la lista
                RsTarget target1 = ShowTarget(new Vector3(-0.50629, -3, 0.67950));
                //CAMBIO :
                /*if (target1 != null) ListOfTargets.addTarget(target1);*/ CreatedTargets[CustomBtn_2.selectedList-1].Add(target1);

                // Crear el segundo target y añadirlo a la lista
                RsTarget target2 = ShowTarget(new Vector3(0.500, 0, 0.700));
                //CAMBIO :
                /*if (target2 != null) ListOfTargets.addTarget(target2);*/ CreatedTargets[CustomBtn_2.selectedList-1].Add(target2);

                //PropertiesTarget.UpdateTargetProperties(CreatedTargets);
            }
            catch (Exception exception)
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage(exception.Message.ToString()));
            }
            finally
            {
                Project.UndoContext.EndUndoStep();
            }
        }
        public static RsTarget ShowTarget(Vector3 position)
        {
            try
            {
                // Get the active station
                Station station = Project.ActiveProject as Station;

                // Create robtarget
                RsRobTarget robTarget = new RsRobTarget();
                robTarget.Name = station.ActiveTask.GetValidRapidName("Targetttttttt", "_", 10);

                // Translation
                robTarget.Frame.Translation = position;

                // Add robtarget to datadeclaration
                station.ActiveTask.DataDeclarations.Add(robTarget);

                // Create target
                RsTarget target = new RsTarget(station.ActiveTask.ActiveWorkObject, robTarget);
                target.Name = robTarget.Name;
                target.Attributes.Add(target.Name, true);

                // Add target to active task
                station.ActiveTask.Targets.Add(target);

                return target;
            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.Message.ToString()));
                return null;
            }
        }
    }
}
