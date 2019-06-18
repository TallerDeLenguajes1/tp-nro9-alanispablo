using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Helpers
{
    class SoporteParaConfiguracion
    {
        public static void CrearArchivoDeConfiguracion()
        {
            if (!Directory.Exists("Program Data"))
            {
                Directory.CreateDirectory("Program Data");
            }
            string nameFile = "configuracion.dat";
            if (!File.Exists("configuracion.dat"))
            {
                FileStream configuracion = File.Create(nameFile);
                using (BinaryWriter configWriter = new BinaryWriter(configuracion))
                {
                    configWriter.Write("Program Data");
                    configWriter.Close();
                }
            }
            return;
        }

        public static string LeerConfiguracion() 
        {
            string readedLine;
            FileStream configuracion = new FileStream("configuracion.dat", FileMode.Open);
            using (BinaryReader configReader = new BinaryReader(configuracion))
            {
                readedLine = configReader.ReadString();
                configReader.Close();
            }
            string destFile;
            string direct = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(direct);
            foreach (string f in files)
            {
                string fileName = Path.GetFileName(f);
                if (Path.GetExtension(f) == ".mp3" || Path.GetExtension(f) == ".txt")
                {
                    destFile = readedLine + "\\" + fileName;
                    File.Move(f, destFile);
                }
            }
            return readedLine;
        }
    }

}