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
using EGM_VS;

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
namespace egmtest
{
    class Program
    {
        // listen on this port for inbound messages
        public static int _ipPortNumber = 6511;
        static void Main(string[] args)
        {
            LoadFile.directory = @"C:\AAAA\Path_EGM_10.csv"; // VMP EGM_CSV

            Sensor s = new Sensor();
            s.Start();

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }
    }

    class Sensor
    {
        private Thread _sensorThread = null;
        private UdpClient _udpServer = null;
        private bool _exitThread = false;
        private uint _seqNumber = 0;
        private int _currentPathPointIndex = 0; //ÍNDICE PARA LA TRAYECTORIA

        public void SensorThread() /// MODIFIED MODIFICADO VMP
        {
            _udpServer = new UdpClient(Program._ipPortNumber);
            var remoteEP = new IPEndPoint(IPAddress.Any, Program._ipPortNumber);

            while (!_exitThread)
            {
                try
                {
                    var data = _udpServer.Receive(ref remoteEP);
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
                catch (SocketException se)
                {
                    // Si el socket se cierra o hay un error de red
                    if (!_exitThread) // Solo muestra error si no estamos intentando salir
                    {
                        Console.WriteLine($"SocketException en SensorThread: {se.Message} (Código: {se.SocketErrorCode})");
                        Thread.Sleep(100); // Pequeña pausa antes de reintentar
                    }
                }
            }
            _udpServer?.Close();
            Console.WriteLine("SensorThread finalizado.");
        }

        // Display message from robot
        void DisplayInboundMessage(EgmRobot robot) /// MODIFIED MODIFICADO VMP
        {
            if (robot.Header != null)
            {
                Console.WriteLine($"Seq={robot.Header.Seqno} tm={robot.Header.Tm}");
            }
            else
            {
                Console.WriteLine("No header in robot message");
            }
        }
        //VMP EGM_CSV
        void GetValues()
        {
            if (!string.IsNullOrEmpty(LoadFile.directory))
            {
                Console.WriteLine($"Intentando cargar archivo desde: {LoadFile.directory}");
                LoadFile.Load(LoadFile.directory); 
            }
            else
            {
                Console.WriteLine("Error: La ruta del archivo (LoadFile.directory) no ha sido especificada.");
            }
        }
        
        // Create a sensor message to send to the robot
        void CreateSensorMessage(MemoryStream memoryStream) //VMP COMANDOS_EGM
        {
            /* VMP EGM_CSV
            // --- Lógica para cambiar el objetivo ---
            targetX += incrementX; // Aumenta la X
                                   // Si llega al límite (o vuelve al inicio), cambia de dirección
            if (targetX > limitX || targetX < 590.0) // Usa el mismo valor inicial aquí
            {
                incrementX *= -1.0; // Invierte la dirección
                targetX += incrementX; // Da el primer paso en la nueva dirección
            }
            VMP EGM_CSV */

            Target_EGM target = new Target_EGM();
            if (_currentPathPointIndex < LoadFile.ListTargetsEGM.Count)
            {
                target = LoadFile.ListTargetsEGM[_currentPathPointIndex];
            }
            else //hemos alcanzado o superado el final, Usamos el último punto válido.
            {
                target = LoadFile.ListTargetsEGM[LoadFile.ListTargetsEGM.Count - 1];
                Console.WriteLine("Fin de trayectoria alcanzado. Manteniendo último punto.");
            }

            // Imprime en la consola qué estás enviando
            Console.WriteLine($"Enviando Objetivo EGM -> X: {target.x:F1}, Y: {target.y:F1}, Z: {target.z:F1}"); // VMP EGM_CSV
            // Crea el mensaje EGM
            var sensorMessage = new EgmSensor
            {
                Header = new EgmHeader
                {
                    Seqno = _seqNumber++,
                    Tm = (uint)Environment.TickCount, // Timestamp simple en milisegundos
                    Mtype = EgmHeader.Types.MessageType.MsgtypeCorrection // Tipo correcto para guiar al robot
                },
                Planned = new EgmPlanned // Aquí va el OBJETIVO que calculaste arriba
                {
                    Cartesian = new EgmPose
                    {
                        Pos = new EgmCartesian
                        {
                            X = target.x,
                            Y = target.y,
                            Z = target.z
                        },
                        Orient = new EgmQuaternion
                        {
                            U0 = target.qw,
                            U1 = target.qx, 
                            U2 = target.qy,
                            U3 = target.qz
                        }
                    }
                }
            };
            // Convierte el mensaje a bytes y lo escribe en el stream
            using (var codedOutput = new CodedOutputStream(memoryStream, true))
            {
                sensorMessage.WriteTo(codedOutput);
                codedOutput.Flush();
            }

            /*if (_currentPathPointIndex < LoadFile.ListTargetsEGM.Count) */_currentPathPointIndex++;
        }
        /*void CreateSensorMessage(MemoryStream memoryStream) //MODIFIED MODIFICADO VMP
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
        // Start a thread to listen on inbound messages
        public void Start()
        {
            //{ VMP EGM_CSV
            /*
            _sensorThread = new Thread(new ThreadStart(SensorThread));
            _sensorThread.Start();
            */
            GetValues();

            _currentPathPointIndex = 0;

            _exitThread = false;
            _sensorThread = new Thread(new ThreadStart(SensorThread));
            _sensorThread.IsBackground = true;
            _sensorThread.Start();
            //}
        }

        // Stop and exit thread
        public void Stop()
        {
            //{ VMP EGM_CSV
            /*
            _exitThread = true;
            _sensorThread.Abort();
            */
            if (_sensorThread != null && _sensorThread.IsAlive)
            {
                if (!_sensorThread.Join(TimeSpan.FromSeconds(1))) // Espera a que el hilo termine
                {
                    Console.WriteLine("Advertencia: El hilo del sensor no terminó limpiamente.");
                }
            }
            Console.WriteLine("Sensor detenido.");
        }
    }
}




