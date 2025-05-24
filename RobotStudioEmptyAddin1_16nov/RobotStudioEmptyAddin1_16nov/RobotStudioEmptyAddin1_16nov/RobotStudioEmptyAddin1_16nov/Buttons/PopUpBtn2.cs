using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using RobotStudioEmptyAddin1_16nov;
using RobotStudioEmptyAddin1_16nov.Paths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RobotStudioEmptyAddin1_16nov.Buttons
{
    internal class PopUpBtn2
    {
        public static void PopUpButtons()
        {
            Project.UndoContext.BeginUndoStep("PopUpButtons");

            try
            {
                // Create a new group in the ribbon for our buttons
                RibbonGroup ribbonPopUpGroup = new RibbonGroup("MyPopupButtonsGroup2", "My Popup Button 2");

                // Create a new popup control
                CommandBarPopup popup = new CommandBarPopup("Select the Robot", "Select the Robot");
                popup.Enabled = CommandBarPopupEnableMode.Enabled;
                popup.Image = Image.FromFile("C:\\AAAA\\PopUp_Button.jpg");
                popup.HelpText = "Click here for selecting an available robot.";
                ribbonPopUpGroup.Controls.Add(popup);

                // Create buttons that will later be added to the popup
                CommandBarButton buttonIRB140 = new CommandBarButton("IRB140", "IRB140");
                buttonIRB140.DisplayAsCheckBox = true;
                buttonIRB140.DefaultEnabled = true;
                buttonIRB140.DefaultChecked = false; // Initially UNchecked
                buttonIRB140.HelpText = "IRB140_6_81_C_G_03";

                // Create another button
                CommandBarButton buttonIRB1010 = new CommandBarButton("IRB1010", "IRB1010");
                buttonIRB1010.DisplayAsCheckBox = true;
                buttonIRB1010.DefaultEnabled = true;
                buttonIRB1010.DefaultChecked = false; // Initially UNchecked
                buttonIRB1010.HelpText = "IRB1010_1.5_37__01";


                // Attach event handler for command execution to buttonPath
                buttonIRB140.ExecuteCommand += (sender, e) => Button_ExecuteCommand(sender, e, "IRB140");
                buttonIRB1010.ExecuteCommand += (sender, e) => Button_ExecuteCommand(sender, e, "IRB1010");

                // Add buttons to popup control
                popup.Controls.Add(buttonIRB140);
                popup.Controls.Add(buttonIRB1010);

                // Add the entire group to the active ribbon tab
                UIEnvironment.ActiveRibbonTab.Groups.Add(ribbonPopUpGroup);
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
        // Handler for MyPath button state change
        private static void Button_ExecuteCommand(object sender, ExecuteCommandEventArgs e, String s)
        {
            CommandBarButton btn = sender as CommandBarButton;

            // Toggle the checked state of the button upon execution
            btn.DefaultChecked = !btn.IsChecked;

            // Log different messages based on the state
            if (btn.IsChecked)
            {
                Logger.AddMessage(new LogMessage(s + " has been selected"));

                if (s == "IRB140") RsMoveInst.robName = "IRB140_6_81_C_G_03";
                else if (s == "IRB1010") RsMoveInst.robName = "IRB1010_1.5_37__01";
                else Logger.AddMessage(new LogMessage("Error con string s en Button_ExecuteCommand de PopUpBtn2"));
            }
             else
            {
                if (s == "IRB140" || s == "IRB1010") RsMoveInst.robName = " ";
                else Logger.AddMessage(new LogMessage("Error con string s en Button_ExecuteCommand de PopUpBtn2"));
            }
        }

    }
}
