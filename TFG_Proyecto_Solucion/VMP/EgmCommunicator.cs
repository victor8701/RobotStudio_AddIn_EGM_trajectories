//ARCHIVO COPIADO DE EGM_VS -> Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Abb.Egm;
using Google.Protobuf;
using ABB.Robotics.RobotStudio;
using System.Diagnostics;

//////////////////////////////////////////////////////////////////////////
// Sample program using protobuf-csharp-port 
// (http://code.google.com/p/protobuf-csharp-port/wiki/GettingStarted)
//
// 1) Download protobuf-csharp binaries from https://code.google.com/p/protobuf-csharp-port/
// 2) Unpack the zip file
// 3) Copy the egm.proto file to a sub catalogue where protobuf-csharp was un-zipped, e.g. ~\protobuf-csharp\tools\egm
// 4) Generate an egm C# file from the egm.proto file by typing in a windows console: protogen .\egm\egm.proto --proto_path=.\egm
// 5) Create a C# console application in Visual Studio
// 6) Install Nuget, in Visual Studio, click Tools and then Extension Manager. Goto to Online, find the NuGet Package Manager extension and click Download.
// 7) Install protobuf-csharp via NuGet, select in Visual Studio, Tools Nuget Package Manager and then Package Manager Console and type PM>Install-Package Google.ProtocolBuffers
// 8) Add the generated file egm.cs to the Visual Studio project (add existing item)
// 9) Copy the code below and then compile, link and run.
//
// Copyright (c) 2014, ABB
// All rights reserved.
//
// Redistribution and use in source and binary forms, with
// or without modification, are permitted provided that 
// the following conditions are met:
//
//    * Redistributions of source code must retain the 
//      above copyright notice, this list of conditions 
//      and the following disclaimer.
//    * Redistributions in binary form must reproduce the 
//      above copyright notice, this list of conditions 
//      and the following disclaimer in the documentation 
//      and/or other materials provided with the 
//      distribution.
//    * Neither the name of ABB nor the names of its 
//      contributors may be used to endorse or promote 
//      products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF 
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
namespace TFG_Proyecto_Solucion
{
    class Execute
    {
        // listen on this port for inbound messages
        public static int _ipPortNumber = 6511;

        static void Begin(string[] args)
        {
            EgmCommunicator s = new EgmCommunicator();
            s.Start();

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }
    }

    public class EgmCommunicator
    {
        private Thread _sensorThread = null;
        private UdpClient _udpServer = null;
        private bool _exitThread = false;
        private uint _seqNumber = 0;

        private CsvFileWriter _miEscritorCsv; // VMP TFG_CSV

        // --- VMP TFG_CSV - CONSTRUCTOR ---
        public EgmCommunicator()
        {

            _seqNumber = 0;

            // VMP TFG_CSV - Crear instancia de CsvFileWriter
            try
            {

                _miEscritorCsv = new CsvFileWriter();
                Debug.WriteLine("EgmCommunicator: CsvFileWriter instanciado.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EgmCommunicator: ERROR creando CsvFileWriter: {ex.Message}");
                _miEscritorCsv = null;
            }
        }

        public void SensorThread() /// MODIFIED MODIFICADO VMP
        {
            // VMP TFG_CSV _udpServer = new UdpClient(Execute._ipPortNumber);
            var remoteEP = new IPEndPoint(IPAddress.Any, Execute._ipPortNumber);

            while (!_exitThread)
            {
                var data = _udpServer.Receive(ref remoteEP); //EXCEPTION
                if (data != null)
                {
                    // Parse del mensaje usando Google.Protobuf
                    EgmRobot robot = EgmRobot.Parser.ParseFrom(data);
                    DisplayInboundMessage(robot);

                    using (var memoryStream = new MemoryStream())
                    {
                        CreateSensorMessage(memoryStream);

                        int bytesSent = _udpServer.Send(memoryStream.ToArray(), (int)memoryStream.Length, remoteEP);
                        if (bytesSent < 0)
                        {
                            Console.WriteLine("Error al enviar al robot");
                        }
                    }
                }
            }
        }

        // Display message from robot
        void DisplayInboundMessage(EgmRobot robot) /// MODIFIED MODIFICADO VMP
        {
            if (robot.Header != null)
            {
                //Console.WriteLine($"Seq={robot.Header.Seqno} tm={robot.Header.Tm}");
            }
            else
            {
                Console.WriteLine("No header in robot message");
            }
        }
        // Create a sensor message to send to the robot
        void CreateSensorMessage(MemoryStream memoryStream) //VMP VS_3D_RS
        {
            // --- LEER DATOS ACTUALES DEL SPACEMOUSE (Desde MyTarget) ---
            double currentTargetX = /*600.0;*/MyTarget.x;
            double currentTargetY = /*-6.5;*/MyTarget.y;
            double currentTargetZ = /*740.0;*/MyTarget.z;

            Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine($"--- Preparando para Enviar Datos EGM ---");
            //Console.WriteLine($"  Target Pos: X={currentTargetX:F3}, Y={currentTargetY:F3}, Z={currentTargetZ:F3}");
            //Console.WriteLine($"  Target Orient: U0={MyTarget.quaternion.U0:F5}, U1={MyTarget.quaternion.U1:F5}, U2={MyTarget.quaternion.U2:F5}, U3={MyTarget.quaternion.U3:F5}");
            Console.ResetColor();

            // --- Crear el mensaje EGM ---
            var sensorMessage = new EgmSensor
            {
                Header = new EgmHeader
                {
                    Seqno = _seqNumber++,
                    Tm = (uint)Environment.TickCount, // Timestamp simple
                    Mtype = EgmHeader.Types.MessageType.MsgtypeCorrection
                },
                Planned = new EgmPlanned
                {
                    Cartesian = new EgmPose
                    {
                        Pos = new EgmCartesian
                        {
                            X = currentTargetX,
                            Y = currentTargetY,
                            Z = currentTargetZ
                        },
                        Orient = MyTarget.quaternion // Usar el EgmQuaternion directamente desde MyTarget
                        /*Orient = new EgmQuaternion
                        {
                            U0 = 0.0,
                            U1 = 0.0,
                            U2 = 1.0,
                            U3 = 0.0,
                        }*/
                    }
                }
            };


            //{ VMP TFG_CSV 
            double qw = MyTarget.quaternion.U0;
            double qx = MyTarget.quaternion.U1;
            double qy = MyTarget.quaternion.U2;
            double qz = MyTarget.quaternion.U3;

            if (_miEscritorCsv != null) // _miEscritorCsv es la variable miembro de EgmCommunicator
            {
                _miEscritorCsv.WriteEgmMotionData(currentTargetX, currentTargetY, currentTargetZ, qw, qx, qy, qz);
            }
            //} VMP TFG_ CSV

            // Serializar y escribir en el stream
            using (var codedOutput = new CodedOutputStream(memoryStream, true))
            {
                sensorMessage.WriteTo(codedOutput);
                codedOutput.Flush();
            }
        }

        /*void CreateSensorMessage(MemoryStream memoryStream)
        {
            var sensor = new EgmSensor
            {
                Header = new EgmHeader
                {
                    Seqno = _seqNumber++,
                    Tm = (uint)DateTime.Now.Ticks,
                    Mtype = EgmHeader.Types.MessageType.MsgtypeCorrection // Verificar este nombre en Egm.cs
                },
                Planned = new EgmPlanned
                {
                    Cartesian = new EgmPose
                    {
                        Pos = new EgmCartesian
                        {
                            X = 10.1,
                            Y = 11.1,
                            Z = 12.2
                        },
                        Orient = new EgmQuaternion
                        {
                            U0 = 1.0,
                            U1 = 0.0,
                            U2 = 0.0,
                            U3 = 0.0
                        }
                    }
                }
            };

            sensor.WriteTo(memoryStream);
        }
        */

        //{ VMP TFG_CSV
        public void Start()
        {
            Debug.WriteLine("EgmCommunicator.Start: Iniciando hilo del sensor...");
            if (_udpServer == null && _miEscritorCsv != null) // Solo inicia si el servidor UDP se pudo crear y el escritor también
            {
                try
                {
                    int ipPortNumber = Execute._ipPortNumber;
                    _udpServer = new UdpClient(ipPortNumber);
                    Debug.WriteLine($"EgmCommunicator.Start: UdpClient escuchando en el puerto {ipPortNumber}.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EgmCommunicator.Start: ERROR creando UdpClient en Start(): {ex.Message}");
                    if (_miEscritorCsv != null) _miEscritorCsv.CloseFile(); // Cerrar CSV si el UDP falla
                    return; // No iniciar el hilo si el UDP falla
                }
            }
            else if (_udpServer == null)
            {
                Debug.WriteLine("EgmCommunicator.Start: _udpServer es null. No se puede iniciar el hilo.");
                if (_miEscritorCsv != null) _miEscritorCsv.CloseFile();
                return;
            }


            _exitThread = false; // Asegurarse de que el hilo puede correr
            _sensorThread = new Thread(new ThreadStart(SensorThread));
            _sensorThread.IsBackground = true; // Para que el hilo no impida cerrar la aplicación
            _sensorThread.Start();
            Debug.WriteLine("EgmCommunicator.Start: Hilo del sensor iniciado.");
        }

        public void Stop()
        {
            Debug.WriteLine("EgmCommunicator.Stop: Solicitud de detención recibida.");
            _exitThread = true;

            // Cerrar el CsvWriter ANTES de intentar cerrar/abortar el hilo,
            if (_miEscritorCsv != null)
            {
                _miEscritorCsv.CloseFile();
                _miEscritorCsv = null;
            }

            // Cerrar el UdpClient para desbloquear el hilo _sensorThread si está en _udpServer.Receive()
            // Esto causará una SocketException en el hilo, que debe ser manejada.
            if (_udpServer != null)
            {
                Debug.WriteLine("EgmCommunicator.Stop: Cerrando UdpClient...");
                _udpServer.Close(); // Esto desbloqueará Receive()
                _udpServer = null; // Liberar la instancia
            }

            if (_sensorThread != null && _sensorThread.IsAlive)
            {
                Debug.WriteLine("EgmCommunicator.Stop: Esperando que SensorThread finalice...");
                if (!_sensorThread.Join(TimeSpan.FromMilliseconds(500)))
                {
                    Debug.WriteLine("EgmCommunicator.Stop: SensorThread no finalizó a tiempo, intentando Abort (no ideal).");
                }
                else
                {
                    Debug.WriteLine("EgmCommunicator.Stop: SensorThread finalizado correctamente.");
                }
            }
            _sensorThread = null; // Liberar la instancia del hilo
            Debug.WriteLine("EgmCommunicator.Stop: Completado.");
        }
        //} TFG_CSV
        /*public void Start()
        {
            _sensorThread = new Thread(new ThreadStart(SensorThread));
            _sensorThread.Start();
        }

        // Stop and exit thread
        public void Stop()
        {
            _exitThread = true;
            _sensorThread.Abort();
        }*/
    }
}