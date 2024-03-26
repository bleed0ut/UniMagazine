namespace UniMagazine.Models.ViewModels
{
    public class FeedBackVM
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public Contribution Contribution { get; set; }
        public string Comment { get; set; }
    }
}
