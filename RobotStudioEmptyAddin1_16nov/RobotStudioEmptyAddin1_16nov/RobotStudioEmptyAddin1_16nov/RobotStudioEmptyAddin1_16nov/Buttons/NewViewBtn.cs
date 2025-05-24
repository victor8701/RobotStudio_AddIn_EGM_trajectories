using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio.Stations.Forms;
using System;

namespace RobotStudioEmptyAddin1_16nov
{
    internal class NewViewBtn
    {
        // Método para crear y agregar una nueva vista
        public static void AddNewView()
        {
            Project.UndoContext.BeginUndoStep("AddNewView");

            try
            {
                // Obtén la estación activa
                Station station = Station.ActiveStation;
                if (station == null)
                {
                    throw new InvalidOperationException("No active station found.");
                }

                // Crear un nuevo control gráfico
                using (GraphicControl gc = new GraphicControl())
                {
                    // Copiar configuraciones desde el control gráfico activo
                    GraphicControl.CopySettings(GraphicControl.ActiveGraphicControl, gc);

                    // Establecer la estación como el objeto raíz para el control gráfico
                    gc.RootObject = station;

                    // Crear una cámara para la vista
                    Camera cam = new Camera
                    {
                        // Establecer la posición de la cámara
                        LookFrom = new Vector3(1000, 1000, 1000)
                    };

                    // Asignar la cámara al control gráfico
                    gc.Camera = cam;

                    // Crear una nueva ventana de documento con el control gráfico
                    DocumentWindow dw = new DocumentWindow("Window ID", gc, "Window Caption");

                    // Agregar la ventana al entorno de RobotStudio
                    UIEnvironment.Windows.Add(dw);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage($"Error: {ex.Message}"));
                throw;
            }
            finally
            {
                // Finalizar el paso de deshacer
                Project.UndoContext.EndUndoStep();
            }
        }
    }
}
