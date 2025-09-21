using MeuClienteApp.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace MeuClienteApp.Services;

public class CustomerService : ICustomerService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _opts = new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ILogger<CustomerService> logger, string? filePath = null)
    {
        _logger = logger;
        _filePath = filePath ?? Path.Combine(FileSystem.AppDataDirectory, "customers.json");
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (!File.Exists(_filePath)) return new();
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Customer>>(json, _opts) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ler {file}", _filePath);
            return new();
        }
        finally { _lock.Release(); }
    }

    public async Task<Customer?> GetByIdAsync(Guid id) => (await GetAllAsync()).FirstOrDefault(c => c.Id == id);

    public async Task AddOrUpdateAsync(Customer customer)
    {
        await _lock.WaitAsync();
        try
        {
            var list = await GetAll_NoLockAsync();
            var idx = list.FindIndex(c => c.Id == customer.Id);
            if (idx >= 0) list[idx] = customer; else list.Add(customer);
            await Save_NoLockAsync(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar cliente {id}", customer.Id);
            throw;
        }
        finally { _lock.Release(); }
    }

    public async Task DeleteAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            var list = await GetAll_NoLockAsync();
            list.RemoveAll(c => c.Id == id);
            await Save_NoLockAsync(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir cliente {id}", id);
            throw;
        }
        finally { _lock.Release(); }
    }

    private async Task<List<Customer>> GetAll_NoLockAsync()
    {
        if (!File.Exists(_filePath)) return new();
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Customer>>(json, _opts) ?? new();
    }

    private async Task Save_NoLockAsync(List<Customer> list)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var json = JsonSerializer.Serialize(list, _opts);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
