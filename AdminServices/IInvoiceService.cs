namespace API_Project.AdminServices
{
    using API_Project.Models.DTOs;

    public interface IInvoiceService
    {
        Task<List<InvoiceDetailDto>> GetAllInvoicesAsync();
    }

}
