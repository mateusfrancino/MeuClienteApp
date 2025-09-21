using MeuClienteApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuClienteApp.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task AddOrUpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
    }
}
