using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using TFG_offline;
using TFG_offline.Paths;
using TFG_offline.Targets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using TFG_offline.Files; //PARA EL POPUP Y CUSTOM NECESITAMOS ESTE
namespace  TFG_offline.Buttons
{
    internal class CustomButton
    {
        private static CommandBarComboBox buttonComboBox;

        private static int _pathSelected = -1;
        private static bool initializated = false;
        public int pathSelected { get => _pathSelected; set => _pathSelected = value; }

        public static void Create()
        {
            // Begin UndoStep
            Project.UndoContext.BeginUndoStep("Create .csv file by a Path from the Station");

            try
            {
                // Create a new group in the ribbon for our buttons
                RibbonGroup customControlGroup = new RibbonGroup("Create .csv file by a Path from the Station", "Create .csv file by a Path from the Station Control");

                // Create combobox button control
                buttonComboBox = new CommandBarComboBox("Create .csv file by a Path from the Station");
                buttonComboBox.Caption = "Create .csv file by a Path from the Station";
                buttonComboBox.Image = Image.FromFile("C:\\AAAA\\Boton2.jpg");
                buttonComboBox.HelpText = "Create .csv file by a Path from the Station";

                for (int i = 0; i < Path_list.MyPaths.Count-1; i++)
                {
                    CommandBarComboBoxItem cmbBoxItem = new CommandBarComboBoxItem("Path " + (i+1));
                    buttonComboBox.Items.Add(cmbBoxItem);
                }

                // Add event handler triggered by changing the selected item.
                buttonComboBox.SelectionChanged += BtnComboBox_SelectionChanged;
                buttonComboBox.SelectedIndex = -1;

                // Add control to ribbon group
                customControlGroup.Controls.Add(buttonComboBox);

                // Set 'Small' layout for buttonComboBox
                RibbonControlLayout[] comboBoxLayout = new RibbonControlLayout[] { RibbonControlLayout.Small, RibbonControlLayout.Small };
                customControlGroup.SetControlLayout(buttonComboBox, comboBoxLayout);

                // Add ribbon group to the active ribbon tab
                UIEnvironment.ActiveRibbonTab.Groups.Add(customControlGroup);

                initializated = true;
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
        static void BtnComboBox_SelectionChanged(object sender, EventArgs e)
        {
            //Called when index in comboxbox changes 
            if (initializated)
            {
                _pathSelected = buttonComboBox.SelectedIndex;
                Logger.AddMessage(new LogMessage("Path " + (_pathSelected + 1) + " selected"));
                List<Target> targetList = Path_list.TargetsFromPath[_pathSelected + 1];
                CreateFile.Create(targetList);
            }
        }
    }
}
