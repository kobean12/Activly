namespace ActivelyServer.Models
{
    public class Pin
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Author { get; set; }
        public string Time { get; set; }  
        public string Activity { get; set; }
        public List<string> InterestedPeople { get; set; }
        public string Date { get; set; }

        public Pin()
        {
            InterestedPeople = new List<string>();
        }
        public string ToFileFormat()
        {
            return $"{Latitude}/{Longitude}/{Author}/{Time}/{Date}/{Activity}/{string.Join(",", InterestedPeople)}";
        }
    }
}
