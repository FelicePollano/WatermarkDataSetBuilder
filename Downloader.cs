using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;



namespace WatermarkDataSetBuilder
{
    public class Downloader
    {
        private const int PAGE_SIZE = 4096;
        private const string NO_WATERMARK = "no-watermark";
        private const string WATERMARK = "watermark";
        private const int RANDOM_SEED = 1000;
        private readonly string train;
        private readonly string valid;
        private readonly string[] queries;
        ConcurrentBag<Photo> imagePool;
        Random random;
        ConcurrentDictionary<string,int> pages;
        public Downloader(string train,string valid,string[] queries)
        {
            this.train = train;
            this.valid = valid;
            this.queries = queries;
            this.imagePool = new ConcurrentBag<Photo>();
            this.pages=new ConcurrentDictionary<string, int>();
            
            copyrightGenerator = new RandomCopyrightGenerator(RANDOM_SEED);
            random = new Random(RANDOM_SEED);
        }
        public async Task Download()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization",Program.APIKEY);
            try{
                await RestoreCheckPoint();
            }
            catch{
                //silently accept an invalid checkpoint
            }
            while(true)
            {
                await MakePool(client);

                var downloads = imagePool.Select(k=>HandleImage(k.src.medium,client));
                await Task.WhenAll(downloads);
                imagePool.Clear();
                await SaveCheckPoint();
            }
        }

        private async Task SaveCheckPoint()
        {
            var checkFile = Path.Combine(Program.DATASETPATH,".checkpoint");
            using(var sr = new StreamWriter(checkFile))
            {
                foreach( var k in pages.Keys )
                {
                    await sr.WriteLineAsync($"{k}:{pages[k]}");
                }
                await sr.FlushAsync();
            }
        }
        private async Task RestoreCheckPoint()
        {
            var checkFile = Path.Combine(Program.DATASETPATH,".checkpoint");
            using(var sr = new StreamReader(checkFile))
            {
               string line;
               while(null!=(line=await sr.ReadLineAsync()))
               {
                   var tokens = line.Split(':');
                   int count;
                   if(int.TryParse(tokens[1],out count))
                   {
                        pages[tokens[0]] = count;   
                   }
               }
            }
        }

        RandomCopyrightGenerator copyrightGenerator;
        private async Task MakePool(HttpClient client)
        {
            int i=0;
            var poolTasks = queries.Select(k=>AddToPool(k,client,i++));
            await Task.WhenAll(poolTasks);
        }
        int downloadCount;
        private async Task HandleImage(string url,HttpClient client)
        {
            var seg = new Uri(url).Segments.Last();
            var s = await client.GetStreamAsync(url);
            Interlocked.Increment(ref downloadCount);

            var wm = downloadCount%2 == 0? WATERMARK:NO_WATERMARK;

            var fileName = Path.Combine(downloadCount%5==0?Path.Combine(valid, wm):Path.Combine(train, wm),seg);

            if(downloadCount% 1000 == 0)
            {
                Console.WriteLine("downloaded {0} images so far",downloadCount);
            }
            byte[] buffer = new byte[PAGE_SIZE];
            using( var wr = new FileStream(fileName,FileMode.Create,FileAccess.ReadWrite) )
            {
                int nread;
                while( 0<(nread=await s.ReadAsync(buffer,0, PAGE_SIZE)) )
                {
                    await wr.WriteAsync(buffer,0,nread);
                }
                await wr.FlushAsync();
            }
            if(wm == WATERMARK )
            {
                var name = copyrightGenerator.GetRandom();
                ApplyWatermark(fileName,name);
            }
            
        }
        double GetRandomNormal(double mean,double stdDev)
        {
            double u1 = 1.0-random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0-random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                        Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                        mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }
        double GetRandomNormalCap(double mean,double stdDev)
        {
            var n = GetRandomNormal(mean,stdDev);
            if(n>mean)
                return mean*2-n;
            else
                return mean;
        }
        private void ApplyWatermark(string fileName, string name)
        {
            var temp = GetTempName(fileName);
            File.Move(fileName,temp);
            try{
                var stream = new FileStream(temp,FileMode.Open,FileAccess.Read);
                using (var img = Image.FromStream(stream))
                {
                    using (var graphic = Graphics.FromImage(img))
                    {
                        var font = new Font(FontFamily.GenericSansSerif, (int)Math.Max(GetRandomNormal(30,20),15), FontStyle.Bold, GraphicsUnit.Pixel);
                        var color = GetRandomColor();
                        var brush = new SolidBrush(color);
                        var ss = graphic.MeasureString(name,font);
                        var point = RandomPos(img.Width,img.Height,ss);

                        graphic.DrawString(name, font, brush, point);
                        img.Save(fileName);
                    }
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine($"can't apply watermark to {fileName}:{e}");
            }
            File.Delete(temp);
        }

        private Point RandomPos(int width, int height, SizeF ss)
        {
            var pos = random.Next(5);
            switch(pos)
            {
                case 0://ne
                    return new Point(0,0);
                case 1://nw
                    return new Point(width-(int)ss.Width,0);
                case 2://center
                default:
                    return new Point((int)(width-ss.Width)/2,(int)(height-ss.Height)/2);
                case 3://se
                    return new Point(0,height-(int)ss.Height);
                case 4://sw
                    return new Point(width-(int)ss.Width,height-(int)ss.Height);
            }
        
        }

        Color GetRandomColor()
        {
            var alpha = GetRandomNormal(128,30);
            if(alpha > 255)
                alpha = 510-alpha;
            if( alpha < 0 )
                alpha = 255+alpha;
            
            if( random.Next(2) == 1 )
            return Color.FromArgb((int)Math.Max(100,alpha),
                (int)GetRandomNormalCap(255,30),
                (int)GetRandomNormalCap(255,30),
                (int)GetRandomNormalCap(255,30));
            else
                return Color.FromArgb((int)alpha,
                (int)Math.Abs(GetRandomNormalCap(0,30)),
                (int)Math.Abs(GetRandomNormalCap(0,30)),
                (int)Math.Abs(GetRandomNormalCap(0,30)));
            
        }


        private string GetTempName(string fileName)
        {
            var dir = Path.GetDirectoryName(fileName);
            return Path.Combine(dir,Path.ChangeExtension(Path.GetFileNameWithoutExtension(fileName)+"tmp",Path.GetExtension(fileName)));
        }

        private async Task AddToPool(string k,HttpClient client,int pindex)
        {
            var page = pages.ContainsKey(k)?pages[k]:1;
            var sr = await JsonSerializer.DeserializeAsync<SearchRoot>(
                await client.GetStreamAsync(string.Format("https://api.pexels.com/v1/search?query={0}&per_page=80&page={1}",k,page))
            );
            foreach( var res in sr.photos )
            {
                imagePool.Add(res);
            }
            pages.AddOrUpdate(k,k=>1,(k,i)=>i+1);
            
        }
    }
}