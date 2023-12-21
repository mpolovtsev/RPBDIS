namespace WebAPI.ViewModels
{
    public class OrganizationViewModel
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? OwnershipFormName { get; set; }

        public int? OwnershipFormId { get; set; }

        public string? Address { get; set; }

        public string? ManagerSurname {  get; set; }

        public int? ManagerId {  get; set; }
    }
}