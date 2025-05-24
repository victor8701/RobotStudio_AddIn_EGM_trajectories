using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using RobotStudioEmptyAddin1_16nov;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotStudioEmptyAddin1_16nov
{
    internal class DocumentWindowCatBtn
    {
        //DOCUMENT WNDW WITH CATEGORIES (Los documentos no están organizados en una única lista o ventana. En lugar de eso, se agrupan en categorías)
        public static void AddCategoryDocumentWindow()
        {
            // Begin UndoStep
            Project.UndoContext.BeginUndoStep("AddDocumentWindow");

            try
            {
                // Define a category name for the document windows.
                string categoryName = "MyCategory";

                // Create the first control (like a button or label inside the window) and document window, set its category, and add it to the UI.  
                Control firstControl = new Control();
                firstControl.BackColor = Color.LightBlue; // Set background color for the control inside the window
                firstControl.Width = 200;
                firstControl.Height = 200;

                // Create a button control inside the first window
                Button button1 = new Button
                {
                    Text = "Click Me",
                    Location = new Point(50, 80), // Position the button inside the control
                    Width = 100,
                    Height = 30
                };

                // Add event for the button click inside the first window
                button1.Click += (sender, e) =>
                {
                    MessageBox.Show("Button in DocumentWindow1 clicked!");
                };

                firstControl.Controls.Add(button1); // Add the button to the control

                DocumentWindow firstWindow = new DocumentWindow(Guid.NewGuid(), firstControl, "MyDocumentWindow1");
                firstWindow.Category = categoryName;
                firstWindow.Caption = "Document 1";
                UIEnvironment.Windows.Add(firstWindow); // Add to the UI

                // Create the second control and document window, set its category, and add it to the UI.  
                Control secondControl = new Control();
                secondControl.BackColor = Color.LightGreen; // Set background color for the control inside the window
                secondControl.Width = 200;
                secondControl.Height = 200;

                // Create a label control inside the second window
                Label label2 = new Label
                {
                    Text = "Hello from DocumentWindow2!",
                    Location = new Point(50, 80), // Position the label inside the control
                    Width = 400
                };

                secondControl.Controls.Add(label2); // Add the label to the control

                DocumentWindow secondWindow = new DocumentWindow(Guid.NewGuid(), secondControl, "MyDocumentWindow2");
                secondWindow.Category = categoryName;
                secondWindow.Caption = "Document 2";
                UIEnvironment.Windows.Add(secondWindow); // Add to the UI
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage($"Error: {ex.Message}"));
                throw;
            }
            finally
            {
                // End the UndoStep
                Project.UndoContext.EndUndoStep();
            }
        }
    }
}
