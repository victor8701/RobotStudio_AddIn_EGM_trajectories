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
//using static System.Net.Mime.MediaTypeNames;

public class PopUpBtn
//POPUP BUTTON (al ser presionado, abre un cuadro de diálogo o ventana emergente (popup))
{
    private static bool _copy;
    public static bool copy
    {
        get { return _copy; }
        set { _copy = value; }
    }
    private static bool _deleteList;
    public static bool deleteList
    {
        get { return _deleteList; }
        set { _deleteList = value; }
    }
    private static bool _deletePath;
    public static bool deletePath
    {
        get { return _deletePath; }
        set { _deletePath = value; }
    }
    public static void PopUpButtons()
    {
        Project.UndoContext.BeginUndoStep("PopUpButtons");

        try
        {
            _copy = false;
            _deleteList = false;

            // Create a new group in the ribbon for our buttons
            RibbonGroup ribbonPopUpGroup = new RibbonGroup("MyPopupButtonsGroup", "My Popup Button");

            // Create a new popup control
            CommandBarPopup popup = new CommandBarPopup("Copy and cambiarnombre", "Copy and cambiarnombre");
            popup.Enabled = CommandBarPopupEnableMode.Enabled;
            popup.Image = Image.FromFile("C:\\AAAA\\PopUp_Button.jpg");
            popup.HelpText = "Click here for more options.";
            ribbonPopUpGroup.Controls.Add(popup);

            // Create buttons that will later be added to the popup
            CommandBarButton buttonCopy = new CommandBarButton("Copy", "Copy");
            buttonCopy.DisplayAsCheckBox = true;
            buttonCopy.DefaultEnabled = true;
            buttonCopy.DefaultChecked = false; // Initially UNchecked
            buttonCopy.HelpText = "Toggle this to enable/disable this feature.";

            // Create another button
            CommandBarButton buttonTarget = new CommandBarButton("deleteList", "deleteList");
            buttonTarget.DisplayAsCheckBox = true;
            buttonTarget.DefaultEnabled = true;
            buttonTarget.DefaultChecked = false; // Initially UNchecked
            buttonTarget.HelpText = "Toggle this to enable/disable this feature.";

            CommandBarButton buttonPth = new CommandBarButton("deletePath", "deletePath");
            buttonPth.DisplayAsCheckBox = true;
            buttonPth.DefaultEnabled = true;
            buttonPth.DefaultChecked = false; // Initially UNchecked
            buttonPth.HelpText = "Toggle this to enable/disable this feature.";

            // Attach event handler for command execution to buttonPath
            buttonCopy.ExecuteCommand += (sender, e) => Button_ExecuteCommand(sender, e, "Copy");
            buttonTarget.ExecuteCommand += (sender, e) => Button_ExecuteCommand(sender, e, "deleteList");
            buttonPth.ExecuteCommand += (sender, e) => Button_ExecuteCommand(sender, e, "deletePath");

            // Add buttons to popup control
            popup.Controls.Add(buttonCopy);
            popup.Controls.Add(buttonTarget);
            popup.Controls.Add(buttonPth);

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
            Logger.AddMessage(new LogMessage(s+" has been activated"));
            /*var instance = new PopUpBtn(); //si no static
            instance.copy = true;*/
            if (s == "Copy") _copy = true;
            else if (s == "deleteList") _deleteList = true;
            else if (s == "deletePath") _deletePath = true;
            else Logger.AddMessage(new LogMessage("Error con string s en Button_ExecuteCommand de PopUpBtn"));
        }
        else
        {
            Logger.AddMessage(new LogMessage(s+" has been deactivated"));
            if (s == "Copy") _copy = false;
            else if (s == "deleteList") _deleteList = false;
            else if (s == "deletePath") _deletePath = false;
            else Logger.AddMessage(new LogMessage("Error con string s en Button_ExecuteCommand de PopUpBtn"));
        }
    }
/*
    // Handler for MyTarget button state change
    private static void ButtonTarget_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
    {
        CommandBarButton btn = sender as CommandBarButton;

        // Toggle the checked state of the button upon execution
        btn.DefaultChecked = !btn.IsChecked;

        // Log different messages based on the state
        if (btn.IsChecked)
        {
            Logger.AddMessage(new LogMessage("MyTarget has been activated"));
        }
        else
        {
            Logger.AddMessage(new LogMessage("MyTarget has been deactivated"));
        }
    }*/
}
