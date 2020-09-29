using System;
using System.IO;
using System.Threading.Tasks;

namespace WatermarkDataSetBuilder
{
    class Program
    {
        public static string APIKEY;
         public static string DATASETPATH;
        static async Task Main(string[] args)
        {
            
            if(args.Length<3)
            {
                Console.WriteLine("use dotnet WatermarakDataSetBuilder.dll <APIKEY> <OUTFOLDER> <query1> query2 ...queryn");
                Environment.Exit(1);
            }
            APIKEY=args[0];
            DATASETPATH = args[1];
            (string train,string valid) = MakeFolders(DATASETPATH);
            var downloader = new Downloader(train,valid,args[2..]);
            await downloader.Download();

        }

        private static (string trainFolder,string validationFolder) MakeFolders(string datasetPath)
        {
            Directory.CreateDirectory(datasetPath);
            var train = Path.Combine(datasetPath,"train");
            Directory.CreateDirectory(train);
            var nowm = Path.Combine(train,"no-watermark");
            Directory.CreateDirectory(nowm);
            var wm = Path.Combine(train,"watermark");
            Directory.CreateDirectory(wm);
            var valid = Path.Combine(datasetPath,"valid");
             Directory.CreateDirectory(valid);
            nowm = Path.Combine(valid,"no-watermark");
            Directory.CreateDirectory(nowm);
            wm = Path.Combine(valid,"watermark");
            Directory.CreateDirectory(wm);
            return (train,valid);
        }
    }
}
