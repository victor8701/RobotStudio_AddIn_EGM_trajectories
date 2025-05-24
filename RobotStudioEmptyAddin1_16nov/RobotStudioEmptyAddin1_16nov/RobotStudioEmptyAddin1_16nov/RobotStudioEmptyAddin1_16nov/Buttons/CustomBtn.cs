using ABB.Robotics.Controllers.Configuration;
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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;

using System.Windows.Forms; //PARA EL POPUP Y CUSTOM NECESITAMOS ESTE


namespace RobotStudioEmptyAddin1_16nov
{
    internal class CustomBtn
    //CUSTOM BUTTON (puede estar vinculado a un script en un lenguaje como RobotWare o Rapid)
    {
        private static string _rutaArchivo;
        public static string rutaArchivo
        {
            get { return _rutaArchivo; }
            set { _rutaArchivo = value; }
        }
        private static string[] _suffixes;
        private static List <List<string>> targetsByKeyboard = new List<List<string>>();
        private static int _numTargetsKb;
        public static void CustomControl()
        {
            // Begin UndoStep
            Project.UndoContext.BeginUndoStep("AddCustomControlButtons");
            //Text Box
            try
            {
                // Create a new group in the ribbon for our buttons
                RibbonGroup customControlGroup = new RibbonGroup("MyCustomControls", "MyCustom Control");

                _numTargetsKb = 0;

                string[] suffixes = { "X", "Y", "Z", "RX", "RY", "RZ" };

                int count = 0;
                foreach (string suffix in suffixes)
                {
                    suffixes[count] = suffix;
                    count++;

                    TextBox dataInputTextBox = new TextBox();
                    dataInputTextBox.Enabled = true;
                    dataInputTextBox.ReadOnly = false;
                    dataInputTextBox.BackColor = Color.Black;
                    dataInputTextBox.ForeColor = Color.SpringGreen;
                    dataInputTextBox.TextAlign = HorizontalAlignment.Right;
                    dataInputTextBox.Text = "Type";
                    dataInputTextBox.Width = 50;

                    dataInputTextBox.Tag = suffix; // Guardar el sufijo aquí

                    CommandBarCustomControl buttonTextBox = new CommandBarCustomControl($"MyTextBox_{suffix}", dataInputTextBox);
                    buttonTextBox.Caption = $"Data {suffix}";
                    buttonTextBox.Image = Image.FromFile("C:\\AAAA\\Boton2.jpg");
                    buttonTextBox.HelpText = "Displays data typed";

                    customControlGroup.Controls.Add(buttonTextBox);

                    RibbonControlLayout[] textBoxLayout = new RibbonControlLayout[] { RibbonControlLayout.Small, RibbonControlLayout.Small };
                    customControlGroup.SetControlLayout(buttonTextBox, textBoxLayout);

                    dataInputTextBox.TextChanged += dataInputTextBox_TextChanged;
                    dataInputTextBox.KeyDown += dataInputTextBox_KeyDown;
                }
                // Create combobox button control
                CommandBarComboBox buttonComboBox = new CommandBarComboBox("MyModule");
                buttonComboBox.Caption = "Visibility";
                buttonComboBox.Image = Image.FromFile("C:\\AAAA\\Boton1.jpg");
                buttonComboBox.HelpText = "Select Visibility.";
                buttonComboBox.DropDown += new EventHandler(BtnComboBox_DropDown);

                CommandBarComboBoxItem cmbBoxItem1 = new CommandBarComboBoxItem("Visible");
                CommandBarComboBoxItem cmbBoxItem2 = new CommandBarComboBoxItem("Non Visible");
                buttonComboBox.Items.Add(cmbBoxItem1);
                buttonComboBox.Items.Add(cmbBoxItem2);

                buttonComboBox.SelectionChanged += new EventHandler(BtnComboBox_SelectionChanged);
                buttonComboBox.SelectedIndex = 0;

                customControlGroup.Controls.Add(buttonComboBox);

                RibbonControlLayout[] comboBoxLayout = new RibbonControlLayout[] { RibbonControlLayout.Small, RibbonControlLayout.Small };
                customControlGroup.SetControlLayout(buttonComboBox, comboBoxLayout);


                //Simple button
                // Create first button
                CommandBarButton button= new CommandBarButton("Create a target", "Create a target");
                button.HelpText = "Create a target by keyboard";
                button.Image = Image.FromFile("C:\\AAAA\\Boton3.jpg"); // Set the image of the button
                button.DefaultEnabled = true;
                customControlGroup.Controls.Add(button);

                /*// Set the size of the buttons
                RibbonControlLayout[] ribbonControlLayout = {
                    RibbonControlLayout.Large,
                };*/

                // Attach event handlers to the button to handle user actions  
                button.ExecuteCommand += new ExecuteCommandEventHandler (Button_ExecuteCommand);

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
            //Called when index in comboxbox changes

            if (sender is CommandBarComboBox comboBox)
            {
                // Obtener el elemento seleccionado
                string selectedItem = comboBox.SelectedItem?.Text;

                // Verificar el valor seleccionado y mostrar el mensaje correspondiente
                if (selectedItem == "Visible")
                {
                    Logger.AddMessage(new LogMessage("Se cambio a Visible"));
                    //PropertiesTarget.UpdateTargetProperties(ListOfTargets.targets, true);
                    PropertiesTarget.UpdateTargetProperties(CreateTarget.CreatedTargets[CustomBtn_2.selectedList - 1], true);
                }
                else if (selectedItem == "Non Visible")
                {
                    Logger.AddMessage(new LogMessage("Se cambio a Non Visible"));
                    //PropertiesTarget.UpdateTargetProperties(ListOfTargets.targets, false);
                    PropertiesTarget.UpdateTargetProperties(CreateTarget.CreatedTargets[CustomBtn_2.selectedList - 1], false);
                }
            }
        }
        static void BtnComboBox_DropDown(object sender, EventArgs e)
        {
            //Called when combobox is clicked
            //Logger.AddMessage(new LogMessage("Combobox was clicked"));
        }

        private static void dataInputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Gestiona el cambio de texto, por ejemplo:
                //Logger.AddMessage(new LogMessage($"El contenido del TextBox ha cambiado: {textBox.Text}"));
            }
        }

        private static void dataInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox && e.KeyCode == Keys.Enter)
            {
                string suffix = " ";
                if (textBox.Tag is string suff)
                {
                    MessageBox.Show($"Texto ingresado en {suff}: {textBox.Text}");
                    suffix = suff;
                }
                //MessageBox.Show($"Texto ingresado en {suffix}: {textBox.Text}");

                char[] textChar = textBox.Text.ToCharArray();
                bool num = true;

                for (int i = 0; i < textChar.Length; i++)
                {
                    if (textChar[i] < '0' || textChar[i] > '9')
                    {
                        Logger.AddMessage(new LogMessage("No es un num"));
                        num = false;
                        break;
                    }
                }

                // Asegurar que targetsByKeyboard tenga al menos una lista interna
                if (targetsByKeyboard.Count == 0)
                {
                    targetsByKeyboard.Add(new List<string>());
                    for (int i = 0; i<6; i++)
                    {
                        targetsByKeyboard[targetsByKeyboard.Count - 1].Add(" ");
                    }
                }

                if (num)
                {
                    //targetsByKeyboard[targetsByKeyboard.Count -1 ].Add(textBox.Text);

                    if (textChar.Length > 3 || textChar.Length < 1)
                    {
                        Logger.AddMessage(new LogMessage("Invalid number of characters"));
                    }
                    else
                    {
                        switch (suffix)
                        {
                            case "X":
                                //targetsByKeyboard[targetsByKeyboard.Count - 1]
                                Logger.AddMessage(new LogMessage("x"));
                                targetsByKeyboard[targetsByKeyboard.Count-1][0] = textBox.Text;
                                break;
                            case "Y":
                                Logger.AddMessage(new LogMessage("y"));
                                targetsByKeyboard[targetsByKeyboard.Count - 1][1] = textBox.Text;

                                break;
                            case "Z":
                                Logger.AddMessage(new LogMessage("z"));
                                targetsByKeyboard[targetsByKeyboard.Count - 1][2] = textBox.Text;
                                break;
                            case "RX":
                                Logger.AddMessage(new LogMessage("rx"));
                                targetsByKeyboard[targetsByKeyboard.Count - 1][3] = textBox.Text;
                                break;
                            case "RY":
                                Logger.AddMessage(new LogMessage("ry"));
                                targetsByKeyboard[targetsByKeyboard.Count - 1][4] = textBox.Text;
                                break;
                            case "RZ":
                                Logger.AddMessage(new LogMessage("rz"));
                                targetsByKeyboard[targetsByKeyboard.Count - 1][5] = textBox.Text;
                                break;
                        }
                        int a = 0;
                        for (int i = 0; i<6; i++)
                        {
                            if (targetsByKeyboard[targetsByKeyboard.Count - 1][i] != " ") a++;
                        }
                        if(a==5) _numTargetsKb++;
                        Logger.AddMessage(new LogMessage("num="+_numTargetsKb));
                    }
                }
                
            }
        }


        public static void Button_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(_rutaArchivo, true))
            {
                if (_numTargetsKb == targetsByKeyboard.Count)
                {
                    Logger.AddMessage(new LogMessage("Target almacenado en C:\\AAAA\\targets_teclado.txt"));
                    for (int i = 0; i < 6; i++)
                    {
                        writer.Write(targetsByKeyboard[targetsByKeyboard.Count - 1][i] + " ");
                    }
                    writer.Write("\n");
                }
                else Logger.AddMessage(new LogMessage("Error en Button_ExecuteCommand. _numTargetsKb="+ _numTargetsKb+ " ; targetsByKeyboard.Count]="+ (targetsByKeyboard.Count)));
            }

        }
    }
}
