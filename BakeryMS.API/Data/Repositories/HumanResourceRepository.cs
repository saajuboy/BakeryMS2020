using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.HumanResource;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Data.Repositories
{
    public class HumanResourceRepository : IHumanResourceRepository
    {
        private readonly DataContext _context;
        public HumanResourceRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.AddRangeAsync(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        //
        public async Task<T> Get<T>(object id) where T : class
        {
            T item = null;
            item = await _context.Set<T>().FindAsync(id);

            return item;
        }
        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {

            var item = await _context.Set<T>().ToListAsync();

            return item;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Employee> GetEmployee(int id)
        {
            var employee = await _context.Employees
            .Where(a => a.IsDeleted == false)
            .FirstOrDefaultAsync(a => a.Id == id);

            return employee;
        }
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var employees = await _context.Employees
            .Where(a => a.IsDeleted == false)
            .ToListAsync();
            return employees;
        }
        public async Task CreateEmployee(Employee employee)
        {
            int MaxNo = 0;
            if (!_context.Employees.Any())
            { MaxNo = 1; }
            else
            {
                MaxNo = await _context.Employees.MaxAsync(a => a.EmployeeNumber) + 1;
            }
            employee.EmployeeNumber = MaxNo;

            var result = await _context.Employees.AddAsync(employee);
        }

    }
}