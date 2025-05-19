using ABB.Robotics.Controllers.Configuration;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.MotionDomain;
using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  TFG_offline.Controller
{
    internal class LoadController
    {
        private static Station station;
        public static Station station_public
        {
            get { return station; }
        }
        private static GraphicComponentLibrary mechLib;
        private static Mechanism mechGfx;
        private static RsWorkObject myWobj = new RsWorkObject();
        public static RsWorkObject MyWobj_public
        {
            get { return myWobj; }
        }
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
        public static void Load()
        {
            Logger.AddMessage(new LogMessage("Loading charger..."));

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
            mechLib = GraphicComponentLibrary.Load((_robotName), true);
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

            //addDefaultPaths();
        }
        public static void CreateWorkObj()
        {
            myWobj.Name = station.ActiveTask.GetValidRapidName("myWobj", "_", 1);
            station.ActiveTask.DataDeclarations.Add(myWobj);
            station.ActiveTask.ActiveWorkObject = myWobj;
        }
    }
}
