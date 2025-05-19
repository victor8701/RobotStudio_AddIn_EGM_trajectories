using ABB.Robotics.RobotStudio;
using System.IO;
using System;
using System.Globalization;
using EGM_VS;

namespace egmtest
{
    internal class LoadFile
    {
        static string _directory;
        static string _parameters;
        public static List<Target_EGM> ListTargetsEGM = new List<Target_EGM>();
        public static string directory { get => _directory; set => _directory = value; }
        //{ VMP EGM_CSV
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
                    Target_EGM target = new Target_EGM();

                    // Crear un nuevo Target y asignar valores
                    target.x = double.Parse(values[0], CultureInfo.InvariantCulture); //CultureInfo.InvariantCulture para que lea correctamente los decimales con "."
                    target.y = double.Parse(values[1], CultureInfo.InvariantCulture);
                    target.z = double.Parse(values[2], CultureInfo.InvariantCulture);
                    target.qw = double.Parse(values[3], CultureInfo.InvariantCulture);
                    target.qx = double.Parse(values[4], CultureInfo.InvariantCulture);
                    target.qy = double.Parse(values[5], CultureInfo.InvariantCulture);
                    target.qz = double.Parse(values[6], CultureInfo.InvariantCulture);

                    ListTargetsEGM.Add(target);

                    Logger.AddMessage(new LogMessage("File created!"));
                }
                catch (Exception ex)
                {
                    Logger.AddMessage(new LogMessage("Error parsing line: " + line + "\n" + ex.Message));
                }
            }
            Console.WriteLine($"Se cargaron {ListTargetsEGM.Count} puntos en LoadFile.");
        }
    }
}
