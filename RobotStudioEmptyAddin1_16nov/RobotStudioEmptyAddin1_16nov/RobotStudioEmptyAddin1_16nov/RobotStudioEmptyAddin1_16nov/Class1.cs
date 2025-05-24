using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using RobotStudioEmptyAddin1_16nov;
using RobotStudioEmptyAddin1_16nov.Buttons;
using RobotStudioEmptyAddin1_16nov.Paths;
using RobotStudioEmptyAddin1_16nov.Targets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;

namespace RobotStudioEmptyAddin_16nov
{
    public class Class1
    {
        // This is the entry point which will be called when the Add-in is loaded
        public static void AddinMain()
        {
            Logger.AddMessage(new LogMessage("Se ha abierto el Add-In RobotStudioEmptyAddin_16nov!"));

            //Ruta en la que se creara el .txt con las coordenadas de los targets creados por teclado
            CustomBtn.rutaArchivo = @"C:\AAAA\targets_teclado.txt";

            //Por defecto la Lista de targets num 1 es la seleccionada
            CustomBtn_2.selectedList = 1;
            CustomBtn_2.numOfLists = 1;
            CustomBtn_2.ñapa = 0;
            CustomBtn_2.ñapa2 = 0;
            CustomBtn_2.newList = true;

            // Asegurar que CreatedTargets[0] existe
            CreatePath.numPath = 0;
            while (CreateTarget.CreatedTargets.Count <= 0)
            {
                CreateTarget.CreatedTargets.Add(new List<RsTarget>());
            }

            SimpleBtn.CreateButton(); //Al pulsar Boton1 se llama a CreateTarget.CreateTargets()
            SplitBtn.SplitButton();
            PopUpBtn.PopUpButtons();
            CustomBtn.CustomControl();
            CustomBtn_2.CustomControl_2();
            //DocumentWindowBtn.AddDocumentWindow();
            DocumentWindowCatBtn.AddCategoryDocumentWindow();
            //NewViewBtn.AddNewView();

            PopUpBtn2.PopUpButtons();

            RsMoveInst.robName = " ";
            //RsMoveInst.RsMoveInstructionCreate();
        }
    }
}