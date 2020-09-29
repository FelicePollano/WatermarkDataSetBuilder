using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
namespace WatermarkDataSetBuilder
{
    public class Downloader
    {
        private readonly string train;
        private readonly string valid;
        private readonly string[] queries;
        ConcurrentBag<string> imagePool;
        int[] pages;
        public Downloader(string train,string valid,string[] queries)
        {
            this.train = train;
            this.valid = valid;
            this.queries = queries;
            this.imagePool = new ConcurrentBag<string>();
            this.pages=new int[queries.Length];
            for(int i=1;i<pages.Length;++i)
                this.pages[i]=1;
        }
        public async Task Download()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization",Program.APIKEY);
            while(true)
            {
                await MakePool(client);
            }
        }

        private async Task MakePool(HttpClient client)
        {
            int i=0;
            var poolTasks = queries.Select(k=>AddToPool(k,client,i++));
            await Task.WhenAll(poolTasks);
        }

        private async Task AddToPool(string k,HttpClient client,int pindex)
        {
            var s = await client.GetStringAsync(string.Format("https://api.pexels.com/v1/search?query={0}&per_page=80&page={2}",k,pages[pindex]));
            pages[pindex]++;
        }
    }
}