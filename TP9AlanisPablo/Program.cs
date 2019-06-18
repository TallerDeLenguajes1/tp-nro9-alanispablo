using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace TP9
{
    class Program
    {
        static void Main(string[] args)
        {
            SoporteParaConfiguracion.CrearArchivoDeConfiguracion();
            string DataDirectory = SoporteParaConfiguracion.LeerConfiguracion();
            Console.WriteLine("Se creo con exito el directorio\n" + DataDirectory);
            Console.Write("Frase que desea traducir a morse: ");
            string aTraducir = Console.ReadLine();
            string[] TextToMorse = ConversorDeMorse.TextoAMorse(aTraducir.ToUpper());
            string fecha = ConversorDeMorse.CrearMorseTxt(TextToMorse, DataDirectory);
            ConversorDeMorse.MorseToMp3(TextToMorse, DataDirectory, fecha);
            ConversorDeMorse.CrearTextoTxt(DataDirectory, fecha);
            Console.WriteLine("\nSe guardaron las correcciones " + DataDirectory);
            Console.ReadKey();
        }
    }
}