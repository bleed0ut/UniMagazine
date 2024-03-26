namespace UniMagazine.Models.ViewModels
{
    public class PendingContributionVM
    {
        public IEnumerable<Contribution> Contributions { get; set; }

        public string SearchByTitle { get; set; }

        public string SearchByContributorEmail { get; set; }
    }
}
