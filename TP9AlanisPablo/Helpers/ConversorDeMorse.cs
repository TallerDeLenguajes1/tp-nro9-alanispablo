using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Helpers
{
    class ConversorDeMorse
    {
        public static string[] morse = { " ", ".- ", "-... ", "-.-. ", "-.. ", ". ", "..-. ", "--. ", ".... ", ".. ", ".--- ", "-.- ", ".-.. ", "-- ", "-. ", "--- ", ".--. ", "--.- ", ".-. ", "... ", "- ", "..- ", "...- ", ".-- ", "-..- ", "-.-- ", "--.. " };
        public static char[] letras = { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public static string[] TextoAMorse(string toConvert)
        {
            string[] traducido = new string[toConvert.Length];
            for (int i = 0; i < toConvert.Length; i++)
            {
                for (int j = 0; j < letras.Length; j++)
                {
                    if (toConvert[i] == letras[j])
                    {
                        traducido[i] = morse[j];
                    }
                }
            }
            return traducido;
        }

        public static string MorseATexto(string[] toConvert)
        {
            string traducido = "";
            for (int i = 0; i < toConvert.Length; i++)
            {
                for (int j = 0; j < morse.Length; j++)
                {
                    if (toConvert[i] == morse[j])
                    {
                        traducido = traducido + letras[j];
                    }
                }
            }
            return traducido;
        }

        public static string CrearMorseTxt(string[] morse, string destDirectory)
        {
            string morseDirectory = destDirectory + @"\Morse";
            if (!Directory.Exists(morseDirectory))
            {
                Directory.CreateDirectory(morseDirectory);
            }

            string fecha = DateTime.Now.ToString("dd_MM_yyyy_h_m_s");
            string morseFileName = morseDirectory+@"\morse_" + fecha + ".txt";
            FileStream morseFile = File.Create(morseFileName);
            using (StreamWriter morseWriter = new StreamWriter(morseFile))
            {
                foreach (string m in morse)
                {
                    morseWriter.WriteLine(m);
                }
                morseWriter.Close();
            }
            return fecha;
        }

        public static void CrearTextoTxt(string destDirectory, string fecha)
        {
            string textDirectory = destDirectory + @"\Morse";
            if (!Directory.Exists(textDirectory))
            {
                Directory.CreateDirectory(textDirectory);
            }
            string textFileName = textDirectory + @"\texto_" + fecha + ".txt";
            string morseFileName = textDirectory + @"\morse_" + fecha + ".txt";

            FileStream toConvert = new FileStream(morseFileName, FileMode.Open);
            string line;
            string morseContent = "";
            using (StreamReader morseReader = new StreamReader(toConvert))
            {
                while ((line=morseReader.ReadLine()) != null)
                {
                    morseContent = morseContent+";"+line;
                }
            }
            string[] morseArray = morseContent.Split(';');
            string MorseToText = MorseATexto(morseArray);
            FileStream textFile = File.Create(textFileName);
            using (StreamWriter textWriter = new StreamWriter(textFile))
            {
                textWriter.WriteLine(MorseToText);
                textWriter.Close();
            }
        }

        public static byte[] FullBinaryReader(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream()) 
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static void MorseToMp3(string[] toConvert, string destDirectory, string fecha)
        {
            string directory = destDirectory + @"\Morse";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string mp3FileName = directory + @"\audio_" + fecha + ".mp3";
            string puntoPath = destDirectory + @"\punto.mp3";
            string rayaPath = destDirectory + @"\raya.mp3";
            string silencioPath = destDirectory + @"\silencio.mp3"; 
            string result = String.Concat(toConvert);
            char[] charMorse = result.ToArray();

            Stream punto = File.OpenRead(puntoPath);
            Stream raya = File.OpenRead(rayaPath);
            Stream silencio = File.OpenRead(silencioPath);

            byte[] puntoBuffer;
            byte[] rayaBuffer;
            byte[] silencioBuffer;

            puntoBuffer = FullBinaryReader(punto);
            punto.Close();
            rayaBuffer = FullBinaryReader(raya);
            raya.Close();
            silencioBuffer = FullBinaryReader(silencio);
            silencio.Close();

            FileStream mp3File = new FileStream(mp3FileName,FileMode.Create);
            Stream mp3File = File.OpenWrite(mp3FileName);

            foreach (char c in charMorse)
            {
                if (c == '.')
                {
                    punto.CopyTo(mp3File);
                    mp3File.Write(puntoBuffer, 0, puntoBuffer.Length);
                }

                if (c == '-')
                {
                    raya.CopyTo(mp3File);
                    mp3File.Write(rayaBuffer, 0, rayaBuffer.Length);
                }
                if (c == ' ')
                {
                    silencio.CopyTo(mp3File);
                    mp3File.Write(silencioBuffer, 0, silencioBuffer.Length);
                }
            }
            mp3File.Close();
        }
    }
}