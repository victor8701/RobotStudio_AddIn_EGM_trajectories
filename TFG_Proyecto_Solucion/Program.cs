// <copyright file="Program.cs" company="3Dconnexion">
// -------------------------------------------------------------------------------------------------
// Copyright (c) 2020 3Dconnexion. All rights reserved.
//
// This file and source code are an integral part of the "3Dconnexion Software Developer Kit",
// including all accompanying documentation, and is protected by intellectual property laws. All use
// of the 3Dconnexion Software Developer Kit is subject to the License Agreement found in the
// "LicenseAgreementSDK.txt" file.
// All rights not expressly granted by 3Dconnexion are reserved.
// -------------------------------------------------------------------------------------------------
// </copyright>

namespace TDx.GettingStarted
{
    using System;
    using System.Windows.Forms;
    using TFG_Proyecto_Solucion;

    using ABB.Robotics.RobotStudio;
    using ABB.Robotics.Controllers;
    using ABB.Robotics.Math;
    using Abb.Egm;

    /// <summary>
    /// The <see cref="Program"/>.
    /// </summary>
    public static class Program
    {
        public static EgmCommunicator egmServerInstance;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // Inicialización de MyTarget
            MyTarget.xprevious = 0;
            MyTarget.yprevious = 0;
            MyTarget.zprevious = 0;

            MyTarget.x = 600;
            MyTarget.y = -6.5;
            MyTarget.z = 740.0;

            MyTarget.quaternion.U0 = 0.0;
            MyTarget.quaternion.U1 = 0.0;
            MyTarget.quaternion.U2 = 1.0;
            MyTarget.quaternion.U3 = 0.0;

            if (MyTarget.quaternion == null) // Solo si no se inicializa en el constructor de MyTarget
            {
                MyTarget.quaternion = new EgmQuaternion
                {
                    U0 = MyTarget.quaternion.U0,
                    U1 = MyTarget.quaternion.U1,
                    U2 = MyTarget.quaternion.U2,
                    U3 = MyTarget.quaternion.U3
                };
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Diagnostics.Debug.WriteLine("Program.Main: Creando e iniciando EgmCommunicator...");
            // VMP TFG_Proyecto - Crear, guardar e iniciar la instancia de EgmCommunicator
            egmServerInstance = new EgmCommunicator();
            egmServerInstance.Start(); // Inicia el hilo de escucha UDP
            System.Diagnostics.Debug.WriteLine("Program.Main: EgmCommunicator iniciado.");

            Application.Run(new Form1());

            // Esta parte solo se ejecutará DESPUÉS de que Form1 se cierre completamente.
            System.Diagnostics.Debug.WriteLine("Program.Main: Application.Run(Form1) ha finalizado. Intentando detener EGM...");
            StopEgmSensor();
            System.Diagnostics.Debug.WriteLine("Program.Main: Finalizado.");
        }

        // Función para detener el sensor EGM
        public static void StopEgmSensor()
        {
            System.Diagnostics.Debug.WriteLine("Program.StopEgmSensor: Intentando detener EGM Communicator...");
            if (egmServerInstance != null)
            {
                try
                {
                    // Comprobar si el hilo está vivo antes de intentar detenerlo, aunque el método Stop() de EgmCommunicator debería manejar esto.
                    egmServerInstance.Stop();
                    System.Diagnostics.Debug.WriteLine("Program.StopEgmSensor: Llamada a EgmCommunicator.Stop() completada.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Program.StopEgmSensor: Error al detener EGM Communicator: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Program.StopEgmSensor: egmServerInstance es null, no se puede detener o ya fue detenido.");
            }
        }
    }
}
