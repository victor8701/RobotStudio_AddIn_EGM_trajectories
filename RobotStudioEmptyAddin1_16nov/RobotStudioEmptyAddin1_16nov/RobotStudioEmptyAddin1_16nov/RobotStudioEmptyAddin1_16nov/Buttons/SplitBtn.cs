using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using RobotStudioEmptyAddin1_16nov;
using RobotStudioEmptyAddin1_16nov.Paths;
using RobotStudioEmptyAddin1_16nov.Targets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;

namespace RobotStudioEmptyAddin1_16nov
{
    internal class SplitBtn
    {
        //SPLIT BUTTON (Boton doble, es como 2 BUTTON en uno)
        public static void SplitButton()
        {
            // Begin UndoStep
            Project.UndoContext.BeginUndoStep("SplitButtons");

            try
            {
                // Crear un grupo de cinta para botones
                RibbonGroup ribbonGroup = new RibbonGroup("MySplitButtonsGroup", "My Split Button");

                // Crear el botón principal
                CommandBarButton button = new CommandBarButton("MyButton", "Mi botón principal");
                button.HelpText = "Haz clic aquí para la acción principal";
                button.DefaultEnabled = true;

                // Manejador para el botón principal
                button.ExecuteCommand += (sender, e) =>
                {
                    Logger.AddMessage(new LogMessage("Botón principal\nSe crearon 2 targets predeterminados."));

                    /*
                    for(int i = 0; i < CreateTarget.CreatedTargets.Count; i++)
                    {
                        Logger.AddMessage(new LogMessage("----------------\nList " + (i+1) + "\n---------------------"));
                        for (int j = 0; j < CreateTarget.CreatedTargets[i].Count; j++)
                        {
                            Logger.AddMessage(new LogMessage(CreateTarget.CreatedTargets [i] [j] .Name));
                        }
                    }
                    */

                    CreateTarget.CreateTargets();
                    //AlignTargets.AlignToWorkObj(CreateTarget.CreatedTargets[CustomBtn_2.selectedList - 1]);
                    RsMoveInst.MoveTo();
                };

                // Crear menú desplegable (galería)
                CommandBarGalleryPopup galleryPopup = new CommandBarGalleryPopup("MyGalleryPopup", "Opciones adicionales");
                galleryPopup.Image = Image.FromFile("C:\\AAAA\\Split_Button.jpg");
                galleryPopup.Enabled = CommandBarPopupEnableMode.Enabled;
                galleryPopup.HelpText = "Elige una opción";

                // Asignar el botón principal al SplitButton
                galleryPopup.ClickButton = button;

                // Crear y agregar un botón secundario
                CommandBarButton secondaryButton = new CommandBarButton("SecondaryButton", "Botón secundario");
                secondaryButton.HelpText = "Opción secundaria";
                secondaryButton.DefaultEnabled = true;

                // Manejador para el botón secundario
                secondaryButton.ExecuteCommand += (sender, e) =>
                {
                    Logger.AddMessage(new LogMessage("Alinear Targets"));
                    if (CreateTarget.CreatedTargets[CustomBtn_2.selectedList-1].Count > 1)
                    {
                        Logger.AddMessage(new LogMessage("Targets alineados correctamente."));
                    }
                    else
                    {
                        Logger.AddMessage(new LogMessage("No hay suficientes Targets para alinear."));
                    }
                };

                // Agregar el botón secundario a la galería
                galleryPopup.GalleryControls.Add(secondaryButton);

                // Añadir la galería al grupo de cinta
                ribbonGroup.Controls.Add(galleryPopup);

                // Agregar el grupo de cinta a la pestaña activa
                UIEnvironment.ActiveRibbonTab.Groups.Add(ribbonGroup);
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
