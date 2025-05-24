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
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms; //PARA EL POPUP Y CUSTOM NECESITAMOS ESTE

namespace RobotStudioEmptyAddin1_16nov
{
    internal class CustomBtn_2
    {
        private static int _ñapa;
        public static int ñapa
        {
            get { return _ñapa; }
            set { _ñapa = value; }
        }
        private static int _ñapa2;
        public static int ñapa2
        {
            get { return _ñapa2; }
            set { _ñapa2 = value; }
        }
        private static CommandBarComboBox buttonComboBox;
        private static CommandBarComboBox buttonComboBox2;
        private static int _numOfLists;
        public static int numOfLists
        {
            get { return _numOfLists; }
            set { _numOfLists = value; }
        }
        private static int _numOfPaths;
        public static int numOfPaths
        {
            get { return _numOfPaths; }
            set { _numOfPaths = value; }
        }
        private static int _selectedPath;
        public static int selectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value; }
        }
        private static int _selectedList;
        public static int selectedList
        {
            get { return _selectedList; }
            set { _selectedList = value; }
        }
        private static bool _newList;
        public static bool newList
        {
            get { return _newList; }
            set { _newList = value; }
        }
        private static bool _newPath;
        public static bool newPath
        {
            get { return _newPath; }
            set { _newPath = value; }
        }
        public static void CustomControl_2()
        {
            Project.UndoContext.BeginUndoStep("AddCustomControlButtons");
            try
            {
                RibbonGroup customControlGroup = new RibbonGroup("MyCustomControls_2", "MyCustom Control 2");

                buttonComboBox = new CommandBarComboBox("MyModule");
                buttonComboBox.Caption = "List of Targets         .";
                buttonComboBox.Image = Image.FromFile("C:\\AAAA\\Boton1.jpg");
                buttonComboBox.HelpText = "Select a List of Targets";

                buttonComboBox.DropDown += BtnComboBox_DropDown;

                CommandBarComboBoxItem cmbBoxItem0 = new CommandBarComboBoxItem("New List");
                buttonComboBox.Items.Add(cmbBoxItem0);

                CommandBarComboBoxItem cmbBoxItem1 = new CommandBarComboBoxItem("List 1");
                buttonComboBox.Items.Add(cmbBoxItem1);

                buttonComboBox.SelectionChanged += BtnComboBox_SelectionChanged;
                buttonComboBox.SelectedIndex = 0;

                //////////
                buttonComboBox2 = new CommandBarComboBox("MyModule2");
                buttonComboBox2.Caption = "List of Paths                       .";
                buttonComboBox2.Image = Image.FromFile("C:\\AAAA\\Boton2.jpg");
                buttonComboBox2.HelpText = "Select a List of Paths";

                buttonComboBox2.DropDown += BtnComboBox_DropDown;

                CommandBarComboBoxItem cmbBoxItem0_2 = new CommandBarComboBoxItem("New Path");
                buttonComboBox2.Items.Add(cmbBoxItem0_2);

                /*CommandBarComboBoxItem cmbBoxItem1_2 = new CommandBarComboBoxItem("Path 1");
                buttonComboBox2.Items.Add(cmbBoxItem1_2);*/

                buttonComboBox2.SelectionChanged += BtnComboBox2_SelectionChanged;
                buttonComboBox2.SelectedIndex = 0;
                ///////////

                customControlGroup.Controls.Add(buttonComboBox);
                customControlGroup.Controls.Add(buttonComboBox2);

                RibbonControlLayout[] comboBoxLayout = new RibbonControlLayout[] { RibbonControlLayout.Small, RibbonControlLayout.Small };
                customControlGroup.SetControlLayout(buttonComboBox, comboBoxLayout);

                RibbonControlLayout[] comboBoxLayout2 = new RibbonControlLayout[] { RibbonControlLayout.Small, RibbonControlLayout.Small };
                customControlGroup.SetControlLayout(buttonComboBox2, comboBoxLayout2);

                UIEnvironment.ActiveRibbonTab.Groups.Add(customControlGroup);
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
            if (sender is CommandBarComboBox comboBox)
            {
                string selectedItem = comboBox.SelectedItem?.Text;

                if (selectedItem != "New List")
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (selectedItem == "List " + (i + 1)) _selectedList = i + 1;
                    }
                    Logger.AddMessage(new LogMessage("List " + _selectedList + " selected"));
                    deleteList(PopUpBtn.deleteList, _selectedList);
                }
                if (selectedItem == "New List")
                {
                    if(_newList)
                    {
                        Logger.AddMessage(new LogMessage("New List created"));
                        addCmbBox();
                    }
                    else _newList = true;
                }
            }
        }
        static void BtnComboBox_DropDown(object sender, EventArgs e)
        {
        }
        public static void addCmbBox()
        {
            _ñapa++;
            if (_ñapa > 1)
            {
                numOfLists++;
                CommandBarComboBoxItem cmbBoxItem = new CommandBarComboBoxItem("List " + numOfLists);
                buttonComboBox.Items.Add(cmbBoxItem);
                CreateTarget.CreatedTargets.Add(new List<RsTarget>());
            }
        }
        public static void deleteList(bool a, int b)
        {
            if (a)
            {
                Logger.AddMessage(new LogMessage("List " + b + " deleted"));
                /*for(int i = 0; i < CreateTarget.CreatedTargets[b - 1].Count - 1; i++)
                {
                    Logger.AddMessage(new LogMessage("Target " + (i+1) + ": " + CreateTarget.CreatedTargets [b - 1] [i] .Name));
                }*/

                if (b - 1 >= 0 && b - 1 < CreateTarget.CreatedTargets.Count)
                {
                    //////////////{
                    if (CreateTarget.CreatedTargets.Count !=0)
                    {
                        Station station = Station.ActiveStation;
                        var targets = station.ActiveTask.Targets.ToArray();
                        if (CreateTarget.CreatedTargets[b - 1].Count != 0)
                        {
                            for (int i = 0; i < CreateTarget.CreatedTargets[b - 1].Count - 1; i++)
                            {
                                if (CreateTarget.CreatedTargets[b - 1][i] != null)
                                {
                                    Logger.AddMessage(new LogMessage("El target "+ i + " existe"));
                                    // Get referencing instructions of target
                                    RsInstruction[] instructions = CreateTarget.CreatedTargets[b - 1][i].RobTarget.GetReferencingInstructions();

                                    // Check that the target is not referenced in any instructions
                                    if (instructions.Length == 0)
                                    {
                                        // Delete the target from data declarations
                                        station.ActiveTask.DataDeclarations.Remove(CreateTarget.CreatedTargets[b - 1][i].RobTarget);
                                        // Delete the target from the station
                                        station.ActiveTask.Targets.Remove(CreateTarget.CreatedTargets[b - 1][i]);
                                    }
                                }
                            }
                        }
                    }
                    else Logger.AddMessage(new LogMessage("Vacio"));
                    //////////////}

                    CreateTarget.CreatedTargets[b - 1] = null;
                }

                string itemText = "List " + b;
                CommandBarComboBoxItem itemToRemove = null;

                foreach (CommandBarComboBoxItem item in buttonComboBox.Items)
                {
                    if (item.Text == itemText)
                    {
                        itemToRemove = item;
                        break;
                    }
                }
                if (itemToRemove != null)
                {
                    buttonComboBox.Items.Remove(itemToRemove);
                }
                try
                {
                    _newList = false;
                    buttonComboBox.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Logger.AddMessage(new LogMessage($"No option selected: {ex.Message}"));
                }
            }
        }
        static void BtnComboBox2_SelectionChanged(object sender, EventArgs e)
        {
            if (sender is CommandBarComboBox comboBox)
            {
                string selectedItem = comboBox.SelectedItem?.Text;

                for (int i = 0; i < 1000; i++)
                {
                    if (selectedItem == "Path " + (i + 1)) _selectedPath = i + 1;
                }
                Logger.AddMessage(new LogMessage("Path " + _selectedPath + " selected"));
                deletePath(PopUpBtn.deletePath, _selectedPath);
            }
        }
        public static void addCmbBox2(/*RsPathProcedure myPath*/)
        {
            _numOfPaths++;
            CommandBarComboBoxItem cmbBoxItem = new CommandBarComboBoxItem("Path " + _numOfPaths);
            buttonComboBox2.Items.Add(cmbBoxItem);
        }
        public static void deletePath(bool a, int b)
        {
            if (a)
            {
                Logger.AddMessage(new LogMessage("Path " + b + " deleted"));

                if (b - 1 >= 0 && b - 1 < CreatePath.CreatedPaths.Count)
                {
                    //Station.ActiveStation.ActiveTask.PathProcedures.Remove(CreatePath.CreatedPaths[b - 1]);
                    ///////////////////////////////////////
                    /*int xd = 0;
                    Logger.AddMessage(new LogMessage("Lista de paths actuales en la tarea:"));
                    foreach (var path in Station.ActiveStation.ActiveTask.PathProcedures)
                    {
                        xd++;
                        Logger.AddMessage(new LogMessage($" -Path: " + xd + "\n"));
                    }*/
                    Logger.AddMessage(new LogMessage("AAAAA: " + CreatePath.CreatedPaths.Count));

                    RsPathProcedure pathToDelete = null;
                    pathToDelete = CreatePath.CreatedPaths[b-1];

                    if (CreatePath.CreatedPaths[b - 1] == null)
                    {
                        Logger.AddMessage(new LogMessage($"Error: CreatePath.CreatedPaths[{b - 1}] es null. No se puede eliminar."));
                    }
                    else
                    {
                        Logger.AddMessage(new LogMessage($"Intentando eliminar: {CreatePath.CreatedPaths[b - 1].Name}"));
                    }

                    if (pathToDelete != null && Station.ActiveStation.ActiveTask.PathProcedures.Contains(pathToDelete))
                    {
                        Station.ActiveStation.ActiveTask.PathProcedures.Remove(pathToDelete);
                        Logger.AddMessage(new LogMessage($"Path {b} eliminado correctamente."));
                    }
                    else
                    {
                        Logger.AddMessage(new LogMessage($"Path {b} no encontrado en la lista de PathProcedures."));
                    }
                    //////////////////////////////////////
                    CreatePath.CreatedPaths[b - 1] = null;
                }

                string itemText = "Path " + b;
                CommandBarComboBoxItem itemToRemove = null;

                foreach (CommandBarComboBoxItem item in buttonComboBox2.Items)
                {
                    if (item.Text == itemText)
                    {
                        itemToRemove = item;
                        break;
                    }
                }
                if (itemToRemove != null)
                {
                    buttonComboBox2.Items.Remove(itemToRemove);
                }
                try
                {
                    buttonComboBox2.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Logger.AddMessage(new LogMessage($"No option selected: {ex.Message}"));
                }
            }
        }
    }
}