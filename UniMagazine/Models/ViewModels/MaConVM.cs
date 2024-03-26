namespace UniMagazine.Models.ViewModels
{
    public class MaConVM
    {
        public Magazine Magazine { get; set; }
        public IEnumerable<Contribution> Contributions { get; set; }
    }
}
