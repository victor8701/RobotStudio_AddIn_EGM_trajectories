using System;
using System.IO;        
using System.Text;      
using System.Diagnostics; 
using Abb.Egm;

namespace TFG_Proyecto_Solucion
{
    public class CsvFileWriter
    {
        private StreamWriter _csvWriter;
        private string _csvFilePath;
        private bool _csvHeaderWritten = false;

        // VMP TFG_CSV
        private const string CsvDirectoryPath = @"C:\AAAA";
        private const string CsvFileName = "Path_EGM_10.csv";

        // Constructor que toma la ruta del archivo
        public CsvFileWriter(string fileName = "Path_EGM_10.csv", string directoryPath = null)
        {
            _csvFilePath = Path.Combine(CsvDirectoryPath, CsvFileName);

            try
            {
                if (!Directory.Exists(CsvDirectoryPath))
                {
                    Directory.CreateDirectory(CsvDirectoryPath);
                    Debug.WriteLine($"CsvFileWriter: Directorio '{CsvDirectoryPath}' creado.");
                }

                // FileMode.Create sobrescribirá el archivo si ya existe.
                // FileMode.Append para añadir (pero entonces la cabecera se maneja diferente).
                _csvWriter = new StreamWriter(_csvFilePath, false, Encoding.UTF8);
                Debug.WriteLine($"CsvFileWriter: Archivo CSV '{_csvFilePath}' abierto para escritura.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CsvFileWriter: ERROR al abrir el archivo CSV '{_csvFilePath}': {ex.Message}");
                _csvWriter = null; // Importante si falla la apertura
            }
        }

        // Método para escribir una línea de datos
        // Pasamos los componentes individuales para desacoplar de la clase MyTarget directamente
        public void WriteEgmMotionData(double x, double y, double z, double qw, double qx, double qy, double qz)
        {
            if (_csvWriter == null)
            {
                Debug.WriteLine("CsvFileWriter.WriteEgmMotionData: _csvWriter es null, no se puede escribir.");
                return; // No hacer nada si el archivo no se pudo abrir
            }

            try
            {
                if (!_csvHeaderWritten)
                {
                    _csvWriter.WriteLine("x,y,z,qw,qx,qy,qz"); //cabecera
                    _csvHeaderWritten = true;
                }

                string csvLine = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                                "{0},{1},{2},{3},{4},{5},{6}",
                                                x, y, z, qw, qx, qy, qz);
                _csvWriter.WriteLine(csvLine);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CsvFileWriter.WriteEgmMotionData: ERROR al escribir en CSV: {ex.Message}");
            }
        }

        // Método para cerrar el archivo
        public void CloseFile()
        {
            if (_csvWriter != null)
            {
                try
                {
                    Debug.WriteLine($"CsvFileWriter.CloseFile: Intentando cerrar '{_csvFilePath}'...");
                    _csvWriter.Flush();
                    _csvWriter.Close(); // Cierra el stream subyacente también
                    _csvWriter.Dispose(); // Liberar recursos
                    Debug.WriteLine($"CsvFileWriter.CloseFile: Archivo CSV '{_csvFilePath}' cerrado.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CsvFileWriter.CloseFile: ERROR al cerrar archivo CSV: {ex.Message}");
                }
                finally // Asegurarse de que se limpia la referencia y el estado
                {
                    _csvWriter = null;
                    _csvHeaderWritten = false;
                }
            }
            else
            {
                Debug.WriteLine("CsvFileWriter.CloseFile: _csvWriter ya era null.");
            }
        }
    }
}