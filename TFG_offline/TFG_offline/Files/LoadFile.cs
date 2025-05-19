using ABB.Robotics.RobotStudio;
using TFG_offline.Targets;
using System.IO;
using System;
using System.Globalization;
using TFG_offline.PATHS;

namespace  TFG_offline.Files
{
    internal class LoadFile
    {
        static string _directory;
        static string _parameters;
        static Target _currentTarget; // Target actual temporal

        public static void Load(string s)
        {
            _directory = s;
            if (File.Exists(_directory))
            {
                using (StreamReader reader = new StreamReader(_directory))
                {
                    Read(reader);
                }
            }
            else
            {
                Logger.AddMessage(new LogMessage(_directory + " not found"));
            }
        }

        public static void Read(StreamReader reader)
        {
            _parameters = reader.ReadLine(); // Leer encabezados, si es necesario

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    // Saltar líneas vacías
                    continue;
                }
                string[] values = line.Split(',');
                try
                {
                    // Crear un nuevo Target y asignar valores
                    _currentTarget = new Target
                    {
                        x = double.Parse(values[0], CultureInfo.InvariantCulture) / 1000, //CultureInfo.InvariantCulture para que lea correctamente los decimales con "."
                        y = double.Parse(values[1], CultureInfo.InvariantCulture) / 1000,
                        z = double.Parse(values[2], CultureInfo.InvariantCulture) / 1000,
                        qw = double.Parse(values[3]),
                        qx = double.Parse(values[4]),
                        qy = double.Parse(values[5]),
                        qz = double.Parse(values[6]),
                        // Convirtiendo valores a enums apropiados
                        type = (Target.motion_type)Enum.Parse(typeof(Target.motion_type),"Move" + values[7], ignoreCase: true),
                        speed = (Target.speed_data)Enum.Parse(typeof(Target.speed_data), values[8], ignoreCase: true),
                        zone = (Target.zone_data)Enum.Parse(typeof(Target.zone_data), values[9], ignoreCase: true)
                        };
                    // Pasar el Target a CreateTarget para crearlo en el entorno
                    CreateTarget.MyTargets.Add(_currentTarget);
                    CreateTarget.Create(CreateTarget.MyTargets.Count-1);
                }
                catch (Exception ex)
                {
                    Logger.AddMessage(new LogMessage("Error parsing line: " + line + "\n" + ex.Message));
                }
            }
            CreatePath.Create(CreateTarget.MyTargets);

            Logger.AddMessage(new LogMessage("Processing finished. Targets data:"));
            foreach (var target in CreateTarget.MyTargets)
            {
                Logger.AddMessage(new LogMessage($"Target - x: {target.x}, y: {target.y}, z: {target.z}, " +
                                                  $"qw: {target.qw}, qx: {target.qx}, qy: {target.qy}, qz: {target.qz}, " +
                                                  $"type: {target.type}, speed: {target.speed}, zone: {target.zone}"));
            }
        }
    }
}
