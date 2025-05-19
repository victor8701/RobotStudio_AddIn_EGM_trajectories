using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Collections.Generic;
using System.Drawing;
using TFG_offline.Buttons;

namespace  TFG_offline.Buttons
{
    internal class SimpleButton
    {
        public static List<RibbonTab> MyTabs { get; private set; } = new List<RibbonTab>();

        private static int _numTab = 0; // static para que sea el mismo _numTab para todos los botones
        
        private bool _newTab; //no para que sean distintos para cada boton
        private int _numSimpButt;
        private string _functionality;

        public SimpleButton(bool newTab, int numSimpButt, string funcionality)
        {
            _newTab = newTab;
            _numSimpButt = numSimpButt;
            _functionality = funcionality;
        }
        public void CreateTab()
        {
            try
            {
                Project.UndoContext.BeginUndoStep("CreateTab");

                _numTab++;

                // Create a new tab
                RibbonTab ribbonTab = new RibbonTab("MyTab" + _numTab, "MyTab" + _numTab);
                UIEnvironment.RibbonTabs.Add(ribbonTab);
                MyTabs.Add(ribbonTab);

                // Set ribbonTab as the active tab
                UIEnvironment.ActiveRibbonTab = ribbonTab;
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
        public void CreateButton()
        {
            try
            {
                Project.UndoContext.BeginUndoStep("CreateButton");

                if (_newTab || MyTabs.Count == 0) CreateTab();

                // Create a group for buttons
                RibbonGroup ribbonGroup = new RibbonGroup(_functionality, _functionality);

                // Create simple button
                CommandBarButton buttonFirst = new CommandBarButton(_functionality, _functionality);
                buttonFirst.HelpText = _functionality;
                buttonFirst.Image = Image.FromFile("C:\\AAAA\\Boton1.jpg"); // Set the image of the button
                buttonFirst.DefaultEnabled = true;

                // Aquí guardo el número de botón dentro del 'Tag' para poder usar la variable no estatica en Button_ExecuteCommand
                buttonFirst.Tag = _numSimpButt;

                ribbonGroup.Controls.Add(buttonFirst);

                // Include seperator between buttons
                CommandBarSeparator separator = new CommandBarSeparator();
                ribbonGroup.Controls.Add(separator);

                // Set the size of the buttons
                RibbonControlLayout[] ribbonControlLayout = {
                    RibbonControlLayout.Large, //uno por cada boton creado
                };
                ribbonGroup.SetControlLayout(buttonFirst, ribbonControlLayout[0]);

                // Add ribbon group to ribbon tab
                MyTabs[_numTab-1].Groups.Add(ribbonGroup);

                // Attach event handlers to the first button to handle user actions  
                buttonFirst.ExecuteCommand += new ExecuteCommandEventHandler(Button_ExecuteCommand);

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
        private static void Button_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
        {

            if (sender is CommandBarButton button && button.Tag is int numButton)
            {
                Logger.AddMessage(new LogMessage("Button " + numButton + " pressed"));

                if (numButton == 1) SimpleButton1.Pressed();
                if (numButton == 2) SimpleButton2.Pressed();
                if (numButton == 3)
                {
                    SimpleButton3.Pressed();
                    CustomButton.Create();
                }
            }
        }
    }
}
