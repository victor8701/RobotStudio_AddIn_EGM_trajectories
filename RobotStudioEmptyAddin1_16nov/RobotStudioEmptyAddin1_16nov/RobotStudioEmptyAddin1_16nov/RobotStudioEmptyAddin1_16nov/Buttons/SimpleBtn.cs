using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using RobotStudioEmptyAddin1_16nov.Paths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;
namespace RobotStudioEmptyAddin1_16nov
{
    internal class SimpleBtn
    {
        private static int _clicking_counter;


        private static bool _clicking;
        public static bool clicking
        {
            get { return _clicking; }
            set { _clicking = value; }
        }

        /*private static int _copy_counter;
        public static bool _copy
        {
            get { return _copy; }
            set { _copy = value; }
        }*/

        // BUTTON(boton simple, al pulsarlo salta la interrupcion)
        public static void CreateButton()
        {
            _clicking_counter = 0;
            //_copy_counter = 0;
            _clicking = true;
            //_copy = false;

            //Begin UndoStep
            Project.UndoContext.BeginUndoStep("Add Buttons");

            try
            {
                // Create a new tab
                RibbonTab ribbonTab = new RibbonTab("MyTab", "MyTab");
                UIEnvironment.RibbonTabs.Add(ribbonTab);

                // Set ribbonTab as the active tab
                UIEnvironment.ActiveRibbonTab = ribbonTab;

                // Create a group for buttons
                RibbonGroup ribbonGroup = new RibbonGroup("My Simple Buttons", "My Simple Buttons");

                // Create first button
                CommandBarButton buttonFirst = new CommandBarButton("Create a path", "Create a path");
                buttonFirst.HelpText = "Create a path with the previous created targets";
                buttonFirst.Image = Image.FromFile("C:\\AAAA\\Boton1.jpg"); // Set the image of the button
                buttonFirst.DefaultEnabled = true;
                ribbonGroup.Controls.Add(buttonFirst);

                // Include seperator between buttons
                CommandBarSeparator separator = new CommandBarSeparator();
                ribbonGroup.Controls.Add(separator);

                // Create second button
                CommandBarButton buttonSecond = new CommandBarButton("Targets by Clicking", "Targets by Clicking");
                buttonSecond.HelpText = "Click to lock/unlock the cration of targets by clickng at any point";
                buttonSecond.Image = Image.FromFile("C:\\AAAA\\Boton2.jpg"); // Set the image of the button
                buttonSecond.DefaultEnabled = true;
                ribbonGroup.Controls.Add(buttonSecond);

                // Create third button
                CommandBarButton buttonThird = new CommandBarButton("Create a Robot", "Create a Robot");
                buttonThird.HelpText = "Create a the previously selected Robot.";
                buttonThird.Image = Image.FromFile("C:\\AAAA\\Boton3.jpg"); // Set the image of the button
                buttonThird.DefaultEnabled = true;
                ribbonGroup.Controls.Add(buttonThird);

                // Include seperator between buttons
                CommandBarSeparator separator2 = new CommandBarSeparator();
                ribbonGroup.Controls.Add(separator2);

                // Set the size of the buttons
                RibbonControlLayout[] ribbonControlLayout = {
                    RibbonControlLayout.Large, //uno de estos por cada boton creado
                    RibbonControlLayout.Large, 
                    RibbonControlLayout.Large,
                };
                ribbonGroup.SetControlLayout(buttonFirst, ribbonControlLayout[0]);
                ribbonGroup.SetControlLayout(buttonSecond, ribbonControlLayout[1]);
                ribbonGroup.SetControlLayout(buttonThird, ribbonControlLayout[2]);

                // Add ribbon group to ribbon tab
                ribbonTab.Groups.Add(ribbonGroup);

                // Attach event handlers to the first button to handle user actions  
                buttonFirst.ExecuteCommand += new ExecuteCommandEventHandler(Button1_ExecuteCommand);

                // Attach event handlers to the second button to handle user actions  
                buttonSecond.ExecuteCommand += new ExecuteCommandEventHandler(Button2_ExecuteCommand);

                // Attach event handlers to the third button to handle user actions  
                buttonThird.ExecuteCommand += new ExecuteCommandEventHandler(Button3_ExecuteCommand);
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
        private static void Button1_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
        {
            //CreatePath.createPath(CreateTarget.CreatedTargets[CreatePath.numPath]);
            CreatePath.createPath(CreateTarget.CreatedTargets[CustomBtn_2.selectedList-1]);
        }

        private static void Button2_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
        {
            _clicking_counter++;
            if (_clicking_counter % 2 == 1)
            {
                Logger.AddMessage(new LogMessage("Creation of targets by clicking ON"));
                _clicking = true;
                PickTarget.PickTargets(_clicking);
            }
            else if (_clicking_counter % 2 == 0)
            {
                Logger.AddMessage(new LogMessage("Creation of targets by clicking OFF"));
                _clicking = false;
                PickTarget.PickTargets(_clicking);
            }
            /*Logger.AddMessage(new LogMessage("Boton 2"));
            bool pick = false;
            PickTarget.PickTargets(pick);*/
        }
        private static void Button3_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
        {
            /*if (RsMoveInst.robName != " ") */RsMoveInst.RsMoveStart(); 
            //else Logger.AddMessage(new LogMessage("robName in RsMoveInst = " + RsMoveInst.robName));
        }
    }
}
