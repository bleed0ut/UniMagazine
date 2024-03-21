namespace UniMagazine.Models.ViewModels
{
    public class ConMaVM
    {
        public Contribution Contribution { get; set; }
        public IEnumerable<MaterialContribution> MaterialContribution { get; set; }
    }
}
