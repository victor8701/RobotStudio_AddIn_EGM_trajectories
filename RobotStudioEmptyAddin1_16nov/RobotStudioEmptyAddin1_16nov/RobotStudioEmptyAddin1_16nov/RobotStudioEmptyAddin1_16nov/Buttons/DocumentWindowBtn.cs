using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using RobotStudioEmptyAddin1_16nov;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace RobotStudioEmptyAddin1_16nov
{
    internal class DocumentWindowBtn
    //DOCUMENTWINDOW (muestra contenido personalizado en tu proyecto, gráficos, reportes, vistas personalizadas...)
    {
        public static void AddDocumentWindow()
        {
            Project.UndoContext.BeginUndoStep("AddDocumentWindow");

            try
            {
                // Panel to contain all controls
                Panel panel = new Panel
                {
                    BackColor = Color.White,
                    Dock = DockStyle.Fill
                };

                // TextBox
                TextBox textBox = new TextBox
                {
                    Multiline = true,
                    Dock = DockStyle.Top,
                    Height = 100,
                    Text = "Introduce datos aquí."
                };
                panel.Controls.Add(textBox);

                // Button
                Button button = new Button
                {
                    Text = "Confirmar",
                    Dock = DockStyle.Top
                };
                button.Click += (sender, e) =>
                {
                    MessageBox.Show($"Texto confirmado: {textBox.Text}");
                };
                panel.Controls.Add(button);

                // Create Document Window
                DocumentWindow window = new DocumentWindow(Guid.NewGuid(), panel, "Mi Ventana Personalizada");
                UIEnvironment.Windows.Add(window);
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