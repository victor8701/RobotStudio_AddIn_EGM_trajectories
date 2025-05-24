using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using RobotStudio.API.Internal;

namespace RobotStudioEmptyAddin1_16nov.Paths
{
    internal class RsMoveInst
    {
        private static Station station;
        private static GraphicComponentLibrary mechLib;
        private static Mechanism mechGfx;

        private static RsWorkObject myWobj = new RsWorkObject();
        private static RsToolData myTool;
        private static RsRobTarget toPoint;
        private static RsPathProcedure myPath;
        private static NetworkScanner scanner;

        private static string _robotName;
        public static string robName
        {
            get { return _robotName; }
            set { _robotName = value; }
        }
        public static void RsMoveStart()
        {
            Logger.AddMessage(new LogMessage("Hola"));

            station = Station.ActiveStation;
            // Crear un escáner de red para buscar controladores
            scanner = new NetworkScanner();
            scanner.Scan(); // Realiza la búsqueda

            var controllers = scanner.Controllers;

            if (controllers.Count > 0)
            {
                Logger.AddMessage(new LogMessage("Hay controller"));
                if (_robotName == " ") CreateWorkObj();
                else CreateRobot();
            }
            else
            {
                Logger.AddMessage(new LogMessage("No Hay controller"));
            }
        }
        public static void CreateRobot()
        {
            // Obtener la estación activa
            station = Station.ActiveStation;
            if (station == null)
            {
                throw new InvalidOperationException("No hay una estación activa en RobotStudio.");
            }

            // Cargar la librería gráfica del robot
            mechLib = GraphicComponentLibrary.Load((_robotName + ".rslib"), true);
            if (mechLib == null)
            {
                throw new InvalidOperationException("No se pudo cargar la librería del robot.");
            }

            // Crear la instancia del mecanismo y agregarlo a la estación
            mechGfx = (Mechanism)mechLib.RootComponent.CopyInstance();
            mechGfx.Name = _robotName;
            station.GraphicComponents.Add(mechGfx);

            station.ActiveTask = mechGfx.Task;

            //Create a RsWorkObject object and add it to the ActiveTask.
            //RsWorkObject myWobj = new RsWorkObject();
            myWobj.Name = station.ActiveTask.GetValidRapidName("myWobj", "_", 1);
            station.ActiveTask.DataDeclarations.Add(myWobj);

            //Create a RsToolData object and add it to the ActiveTask.
            //RsToolData myTool = new RsToolData();
            myTool = new RsToolData();
            myTool.Name = station.ActiveTask.GetValidRapidName("myTool", "_", 1);
            station.ActiveTask.DataDeclarations.Add(myTool);


            //Create a new RsRobTarget and add it to the ActiveTask.
            //RsRobTarget toPoint = new RsRobTarget();
            toPoint = new RsRobTarget();
            toPoint.Name = station.ActiveTask.GetValidRapidName("myRobTarget", "_", 1);
            station.ActiveTask.DataDeclarations.Add(toPoint);

            //Create a graphic representation of the RobTarget and RsTarget.
            RsTarget myRsTarget = new RsTarget(myWobj, toPoint);
            myRsTarget.Name = toPoint.Name;
            station.ActiveTask.Targets.Add(myRsTarget);

            myPath = new RsPathProcedure("myPath");

            addDefaultPaths();
        }
        public static void CreateWorkObj()
        {
            myWobj.Name = station.ActiveTask.GetValidRapidName("myWobj", "_", 1);
            station.ActiveTask.DataDeclarations.Add(myWobj);
            station.ActiveTask.ActiveWorkObject = myWobj;
        }

        /*public static void AddCreatedPaths()
        {
            // Escanear controladores disponibles
            NetworkScanner scanner = new NetworkScanner();
            scanner.Scan();
            ControllerInfo controllerInfo = scanner.Controllers.FirstOrDefault();

            if (controllerInfo == null)
            {
                Logger.AddMessage(new LogMessage("No se encontró un controlador disponible en la red."));
                return;
            }

            // Conectar al controlador
            using (Controller controller = ControllerFactory.CreateFrom(controllerInfo))
            {
                controller.Logon(UserInfo.DefaultUser); // Iniciar sesión en el controlador

                // Obtener la tarea RAPID del controlador (T_ROB1)
                ABB.Robotics.Controllers.RapidDomain.Task rapidTask = controller.Rapid.GetTask("T_ROB1");

                if (rapidTask == null)
                {
                    throw new InvalidOperationException("No se encontró la tarea en el controlador.");
                }

                //Obtener la variable RAPID en lugar de todo el módulo
                RapidData pathData = rapidTask.GetRapidData("MainModule", "pathData");  // Reemplaza "pathData" por el nombre correcto

                if (pathData == null)
                {
                    throw new InvalidOperationException("No se encontró la variable RAPID en el módulo.");
                }

                // Obtener el path creado en RobotStudio
                myPath = CreatePath.CreatedPaths[CustomBtn_2.selectedPath - 1];

                if (myPath == null)
                {
                    throw new InvalidOperationException("No se encontró el path seleccionado.");
                }

                // Convertir el path en instrucciones RAPID
                StringBuilder rapidCode = new StringBuilder();
                foreach (RsMoveInstruction instruction in myPath.Instructions)
                {
                    if (instruction.GetMotionType().ToString() == "MoveJ")
                    {
                        rapidCode.AppendLine($"MoveJ {instruction.GetAllRobTargets()}, v100, fine, {instruction.GetToolArgument()};");
                    }
                    else if (instruction.GetMotionType().ToString() == "MoveL")
                    {
                        rapidCode.AppendLine($"MoveL {instruction.GetAllRobTargets()}, v100, fine, {instruction.GetToolArgument()};");
                    }
                }

                //Escribir en RAPID usando WriteRapid()
                using (Mastership m = Mastership.Request(controller.Rapid))
                {
                    pathData.Value = new ABB.Robotics.Controllers.RapidDomain.String("PROC myPath()\n" + rapidCode.ToString() + "ENDPROC");
                }

                Logger.AddMessage(new LogMessage($"Path '{myPath.Name}' agregado al controlador en la tarea '{rapidTask.Name}'"));
            }
        }*/
        public static void addDefaultPaths()
        {
            //Create a new RobTarget to use as cirPoint and add it to the ActiveTask.
            RsRobTarget cirPoint = new RsRobTarget();
            cirPoint.Name = station.ActiveTask.GetValidRapidName("myRobTarget", "_", 1);
            cirPoint.Frame.X = cirPoint.Frame.X + 0.10;
            cirPoint.Frame.Z = cirPoint.Frame.Z + 0.10;
            station.ActiveTask.DataDeclarations.Add(cirPoint);

            //Create a graphic representation of the RobTarget and RsTarget.
            RsTarget myRsTarget_2 = new RsTarget(myWobj, cirPoint);
            myRsTarget_2.Name = cirPoint.Name;
            station.ActiveTask.Targets.Add(myRsTarget_2);

            //Create a circular move instruction.
            RsMoveInstruction myMoveCirc = new RsMoveInstruction(station.ActiveTask, "Move", "Default", myWobj.Name, cirPoint.Name, toPoint.Name, myTool.Name);
            myPath.Instructions.Add(myMoveCirc);

            //Create a joint target and add it to the ActiveTask.
            RsJointTarget myJointTarget = new RsJointTarget();
            myJointTarget.Name = station.ActiveTask.GetValidRapidName("myJointTarget", "_", 1);
            station.ActiveTask.DataDeclarations.Add(myJointTarget);

            //Set the robot axis values.
            RobotAxisValues rbAxis = new RobotAxisValues();
            rbAxis.Rax_1 = 70.0000000000001;
            rbAxis.Rax_2 = -30;
            rbAxis.Rax_3 = 30;
            rbAxis.Rax_4 = -55.0000000000001;
            rbAxis.Rax_5 = 40;
            rbAxis.Rax_6 = 10;
            myJointTarget.SetRobotAxes(rbAxis, false);

            //Create a MoveAbsJ move instruction passing the joint target object in step 13 as a parameter (this only makes sense if there is a mechanism in the station).
            RsMoveInstruction myMoveAbsJ = new RsMoveInstruction(station.ActiveTask, "MoveAbs", "Default", myJointTarget.Name);
            myPath.Instructions.Add(myMoveAbsJ);
        }

        public static void MoveTo()
        {
            Project.UndoContext.BeginUndoStep("RsMoveInstructionMovementMethods");
            try
            {
                Station station = Station.ActiveStation;
                // Create a PathProcedure to add the move instructions to.
                RsPathProcedure myPath = CreatePath.CreatedPaths[CustomBtn_2.selectedPath];

                // Create a joint target corresponding to the robots home position.
                RsJointTarget myHomeJointTarget = new RsJointTarget();
                myHomeJointTarget.Name = "myHomeJT";
                station.ActiveTask.DataDeclarations.Add(myHomeJointTarget);
                
                // Set the robot axis values.
                RobotAxisValues rbHomeAxis = new RobotAxisValues();
                rbHomeAxis.Rax_1 = 0;
                rbHomeAxis.Rax_2 = 0;
                rbHomeAxis.Rax_3 = 0;
                rbHomeAxis.Rax_4 = 0;
                rbHomeAxis.Rax_5 = 30;
                rbHomeAxis.Rax_6 = 0;
                myHomeJointTarget.SetRobotAxes(rbHomeAxis, false);
                

                // Create another joint target to jump and move to.
                RsJointTarget myJointTarget = new RsJointTarget();
                myJointTarget.Name = "myJointTarget";
                station.ActiveTask.DataDeclarations.Add(myJointTarget);

                // Set the robot axis values.
                RobotAxisValues rbAxis = new RobotAxisValues();
                rbAxis.Rax_1 = 70.0000000000001;
                rbAxis.Rax_2 = -30;
                rbAxis.Rax_3 = 30;
                rbAxis.Rax_4 = -55.0000000000001;
                rbAxis.Rax_5 = 40;
                rbAxis.Rax_6 = 10;
                myJointTarget.SetRobotAxes(rbAxis, false);
                
                // Create a move instruction.
                RsMoveInstruction myMoveAbsJ = new RsMoveInstruction(station.ActiveTask, "MoveAbs", "Default", "myJointTarget");
                myPath.Instructions.Add(myMoveAbsJ);
                
                // Jump to the move instruction 'myMoveAbsJ'.
                if (myMoveAbsJ.JumpTo())
                {
                    Logger.AddMessage(new LogMessage("The robot jumped successfully!"));
                }
                else
                {
                    Logger.AddMessage(new LogMessage("The JumpTo command failed!"));
                }

                // Jump the robot to its home postition.
                myHomeJointTarget.JumpTo();

                // Move to the move instruction 'myMoveAbsJ'
                if (myMoveAbsJ.MoveTo())
                {
                    Logger.AddMessage(new LogMessage("The robot moved successfully!"));
                }
                else
                {
                    Logger.AddMessage(new LogMessage("The MoveTo command failed!"));
                }
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
