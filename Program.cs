using System;
using System.IO;
using System.Threading.Tasks;

namespace WatermarkDataSetBuilder
{
    class Program
    {
        public static string APIKEY;
        static async Task Main(string[] args)
        {
            
            if(args.Length<3)
            {
                Console.WriteLine("use dotnet WatermarakDataSetBuilder.dll <APIKEY> <OUTFOLDER> <query1> query2 ...queryn");
                Environment.Exit(1);
            }
            APIKEY=args[0];
            (string train,string valid) = MakeFolders(args[1]);
            var downloader = new Downloader(train,valid,args[2..]);
            await downloader.Download();

        }

        private static (string trainFolder,string validationFolder) MakeFolders(string datasetPath)
        {
            Directory.CreateDirectory(datasetPath);
            var train = Path.Combine(datasetPath,"train");
            Directory.CreateDirectory(train);
            var valid = Path.Combine(datasetPath,"valid");
            return (train,valid);
        }
    }
}
