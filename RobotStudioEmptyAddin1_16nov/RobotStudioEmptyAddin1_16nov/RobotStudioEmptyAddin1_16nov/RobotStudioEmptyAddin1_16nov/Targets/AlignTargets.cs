using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotStudio.API.Internal;

namespace RobotStudioEmptyAddin1_16nov
{
    internal class AlignTargets
    {
        public static void AlignTarget(List<RsTarget> existingTargets)
        {
            Project.UndoContext.BeginUndoStep("AlignTarget");

            try
            {
                if (existingTargets.Count > 1)
                {
                    Station station = Project.ActiveProject as Station;

                    // Obtener las posiciones de los Targets
                    Vector3 firstTargetVector = existingTargets[0].Transform.GlobalMatrix.Translation;
                    Vector3 secondTargetVector = existingTargets[existingTargets.Count - 1].Transform.GlobalMatrix.Translation;

                    // Crear vector normal
                    Vector3 normalVector = new Vector3(
                        existingTargets[0].Transform.GlobalMatrix.z.x,
                        existingTargets[0].Transform.GlobalMatrix.z.y,
                        existingTargets[0].Transform.GlobalMatrix.z.z);

                    // Dirección entre p1 y p2
                    Vector3 dir_p1p2 = secondTargetVector.Subtract(firstTargetVector);
                    dir_p1p2.Normalize();

                    // Producto cruz entre la dirección y el vector normal
                    Vector3 y = dir_p1p2.Cross(normalVector);
                    y.Normalize();

                    // Asignar valores nuevos a los Targets
                    foreach (RsTarget target in existingTargets)
                    {
                        Matrix4 normalMatrix = new Matrix4(y, dir_p1p2, normalVector, target.Transform.GlobalMatrix.Translation);
                        target.RobTarget.Frame.Matrix = normalMatrix;
                    }
                }
            }
            catch (Exception ex)
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage(ex.Message.ToString()));
            }
            finally
            {
                Project.UndoContext.EndUndoStep();
            }
        }

        public static void AlignToWorkObj(List<RsTarget> existingTargets)
        {
            Station station = Project.ActiveProject as Station;

            // Obtener las posiciones de los Targets
            Vector3 firstTargetVector = existingTargets[0].Transform.GlobalMatrix.Translation;
            //Vector3 secondTargetVector = existingTargets[existingTargets.Count - 1].Transform.GlobalMatrix.Translation;            

            double toolRx = station.ActiveTask.ActiveTool.Frame.RX;
            double toolRy = station.ActiveTask.ActiveTool.Frame.RY;
            double toolRz = station.ActiveTask.ActiveTool.Frame.RZ;

            Logger.AddMessage(new LogMessage("x: " + toolRx + " y: " + toolRy + " z: " + toolRz));

            if (existingTargets.Count > 1)
            {
                for (int i = 0; i < existingTargets.Count - 1; i++)
                {
                    Logger.AddMessage(new LogMessage(existingTargets[i].Name + "-> x: " + Globals.RadToDeg(existingTargets[i].Transform.RX) + " y: " + Globals.RadToDeg(existingTargets[i].Transform.RY) + " z: " + Globals.RadToDeg(existingTargets[i].Transform.RZ)));

                    existingTargets[i].Transform.RX = toolRx;
                    existingTargets[i].Transform.RY = toolRy;
                    existingTargets[i].Transform.RZ = toolRz;                   
                }
            }

            Logger.AddMessage(new LogMessage(station.ActiveTask.ActiveTool.Name));
            Logger.AddMessage(new LogMessage("x: " + Globals.RadToDeg(toolRx) + " y: " + Globals.RadToDeg(toolRy) + " z: " + Globals.RadToDeg(toolRz)));
        }
    }
}
