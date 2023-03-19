using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Linq;

namespace ChangeWP
{
    class Program
    {
        private const int SPI_SETDESKWALLPAPER = 20;

        static void Main(string[] args)
        {
            bool createdNew;
            Mutex mutex = new Mutex(true, "2eab3dae-b36a-11ed-afa1-0242ac120002", out createdNew);
            if (!createdNew) return;

            var logger = new Logger("log.txt");
            try
            {
                string directory = ConfigurationManager.AppSettings["WallPaperDirectory"];
                int cycleTime = int.Parse(ConfigurationManager.AppSettings["CycleTime"]);
                if (cycleTime < 5) cycleTime = 5;
                logger.Log("Directory :" + directory + ", Cycle time : " + cycleTime.ToString() + " seconds");
                
                FileCycle fileCycle = new FileCycle(directory);
                while (true)
                {
                    SetDesktopWallpaper(fileCycle.GetNextFile());
                    logger.Log("Changing wallpaper...");
                    Thread.Sleep(cycleTime * 1000);
                }
            }
            catch (Exception ex)
            {
                logger.Log("Error occurred while cycling through files: " + ex.Message);
            }

            mutex.ReleaseMutex();
        }


        static void SetDesktopWallpaper(string filePath)
        {
            SPI spi = new SPI();
            spi.SystemParametersInfoEx(SPI_SETDESKWALLPAPER, 0, filePath, 0);
        }
    }

    class SPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public void SystemParametersInfoEx(int uAction, int uParam, string lpvParam, int fuWinIni)
        {
            SystemParametersInfo(uAction, uParam, lpvParam, fuWinIni);
        }
    }

    public class FileCycle
    {
        private string[] fileNames;
        private int currentIndex;

        public FileCycle(string directoryPath)
        {
            try
            {
                // Get all JPEG, BMP and PNG files in the directory
                string[] fileExtensions = { ".jpg", ".bmp", ".png" };

                fileNames = Directory.GetFiles(directoryPath)
                                  .Where(file => fileExtensions.Contains(Path.GetExtension(file)))
                                  .ToArray();

                ShuffleFilesList(ref fileNames);
                currentIndex = 0;
            }
            catch (Exception ex)
            {
                // Handle the exception by throwing a custom exception with a descriptive message
                throw new Exception("Error occurred while initializing FileCycle: " + ex.Message);
            }
        }

        public static void ShuffleFilesList(ref string[] array)
        {
            // Get the current system time in ticks for random seed
            long ticks = DateTime.Now.Ticks;
            Random random = new Random((int)ticks);

            for (int i = array.Length - 1; i > 0; i--)
            {
                // Generate a random index between 0 and i (inclusive)
                int j = random.Next(i + 1);

                // Swap the values at positions i and j
                string temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }


        public string GetNextFile()
        {
            // Get the fully qualified path of the current file
            string filePath = Path.GetFullPath(fileNames[currentIndex]);

            // Move to the next file index, cycling back to the start if necessary
            currentIndex = (currentIndex + 1) % fileNames.Length;

            return filePath;
        }
    }

    public class Logger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            try
            {
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
                }
            }
            catch { }
        }
    }
}
