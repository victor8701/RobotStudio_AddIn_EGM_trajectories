using ABB.Robotics.RobotStudio;
using TFG_offline.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_offline.Files
{
    internal class CreateFile
    {
        private static StreamWriter _csvWriter;

        private static string _csvFilePath;

        private static string CsvDirectoryPath = @"C:\AAAA";
        private static string CsvFileNamePrefix = "Path_Station_";
        private static string CsvFormat = ".csv";
        private static bool _csvHeaderWritten = false;
        private static int fileSuffixCounter = 0; // Empezará en 0, el primer archivo será _10

        public static void Create(List<Target> targetList)
        {
            if (targetList == null || !targetList.Any())
            {
                Debug.WriteLine("CreateFile.Create: targetList está vacía o es null. No se creará archivo.");
                return;
            }

            fileSuffixCounter += 10;
            string currentCsvFileName = $"{CsvFileNamePrefix}{fileSuffixCounter}{CsvFormat}";
            string fullCsvPath = Path.Combine(CsvDirectoryPath, currentCsvFileName);

            Debug.WriteLine($"CreateFile.Create: Intentando crear archivo: {fullCsvPath}");

            StreamWriter localCsvWriter = null; 
            bool headerWrittenForThisFile = false; // Cabecera por archivo

            try
            {
                //Asegurarse de que el directorio exista
                if (!Directory.Exists(CsvDirectoryPath))
                {
                    Directory.CreateDirectory(CsvDirectoryPath);
                    Debug.WriteLine($"CreateFile.Create: Directorio '{CsvDirectoryPath}' creado.");
                }

                // Abrir el archivo para esta llamada específica
                localCsvWriter = new StreamWriter(fullCsvPath, false, Encoding.UTF8); // false para sobrescribir si existe

                foreach (var target in targetList)
                {
                    if (!headerWrittenForThisFile)
                    {
                        localCsvWriter.WriteLine("x,y,z,qw,qx,qy,qz,t,v,z"); //cabecera
                        headerWrittenForThisFile = true;
                    }

                    string csvLine = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                                    "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                                                   target.x, target.y, target.z,
                                                   target.qw, target.qx, target.qy, target.qz,
                                                   motType.DeleteMove(target), // DeleteMove devuelve string (quita "Move" de "MoveL")
                                                   target.speed.ToString(),
                                                   target.zone.ToString());
                    localCsvWriter.WriteLine(csvLine);
                }
                Debug.WriteLine($"CreateFile.Create: Datos escritos en '{fullCsvPath}'.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateFile.Create: ERROR al escribir en CSV '{fullCsvPath}': {ex.Message}");
            }
            finally
            {
                if (localCsvWriter != null)
                {
                    try
                    {
                        localCsvWriter.Flush();
                        localCsvWriter.Close();
                        localCsvWriter.Dispose();
                        Debug.WriteLine($"CreateFile.Create: Archivo '{fullCsvPath}' cerrado.");
                    }
                    catch (Exception exClose)
                    {
                        Debug.WriteLine($"CreateFile.Create: ERROR al cerrar CSV '{fullCsvPath}': {exClose.Message}");
                    }
                }
            }
        }
    }
}
