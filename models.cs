using System.Collections.Generic;

namespace WatermarkDataSetBuilder
{
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Src    {
        public string original { get; set; } 
        public string large2x { get; set; } 
        public string large { get; set; } 
        public string medium { get; set; } 
        public string small { get; set; } 
        public string portrait { get; set; } 
        public string landscape { get; set; } 
        public string tiny { get; set; } 
    }

    public class Photo    {
        public int id { get; set; } 
        public int width { get; set; } 
        public int height { get; set; } 
        public string url { get; set; } 
        public string photographer { get; set; } 
        public string photographer_url { get; set; } 
        public int photographer_id { get; set; } 
        public Src src { get; set; } 
        public bool liked { get; set; } 
    }

    public class SearchRoot    {
        public int total_results { get; set; } 
        public int page { get; set; } 
        public int per_page { get; set; } 
        public List<Photo> photos { get; set; } 
        public string next_page { get; set; } 
    }
}
