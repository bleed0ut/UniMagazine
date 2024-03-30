namespace UniMagazine.Models.ViewModels
{
    public class MyContributionVM
    {
        public IEnumerable<Contribution> Contributions { get; set; }

        public string? Search { get; set; } = "";

        public string? Status = "";
    }
}
