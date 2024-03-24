namespace UniMagazine.Models.ViewModels
{
    public class MagazineDetailVM
    {
        public Magazine Magazine { get; set; }
        public IEnumerable<Contribution> Contributions { get; set; }
    }
}
