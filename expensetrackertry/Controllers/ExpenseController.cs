using expensetrackertry.DTO;
using expensetrackertry.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace expensetrackertry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseTrackerContext DBContext;

        public ExpenseController (ExpenseTrackerContext dBContext)
        {
            DBContext = dBContext;
        }


        [HttpGet("GetExpenses")] // showing of expense
        public async Task<ActionResult<List<ExpenseDTO>>> Get()
        {
            var List = await DBContext.Expenses.Select(
                s => new ExpenseDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    Description = s.Description,
                    Price = s.Price,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = DateTime.Now
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }


        [HttpPost("InsertExpenses")] //adding expense to db
        public async Task<HttpStatusCode> InsertUser(ExpenseDTO Expense)
        {
            var entity = new Expense()
            {
                Name = Expense.Name,
                Category = Expense.Category,
                Description = Expense.Description,
                Price = Expense.Price,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };
            DBContext.Expenses.Add(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.Created;
        }

        [HttpGet("ShowExpensesByCategory")] // show expenses based on category
        public async Task<ActionResult<List<ExpenseDTO>>> GetExpensesByCategory(string category)
        {
            List<ExpenseDTO> expenses;

            if (string.IsNullOrEmpty(category) || category.ToLower() == "all")
            {
                expenses = await DBContext.Expenses
                    .Select(s => new ExpenseDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Category = s.Category,
                        Description = s.Description,
                        Price = s.Price,
                        CreatedAt = s.CreatedAt
                    }).ToListAsync();
            }
            else
            {
                expenses = await DBContext.Expenses
                    .Where(s => s.Category == category)
                    .Select(s => new ExpenseDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Category = s.Category,
                        Description = s.Description,
                        Price = s.Price,
                        CreatedAt = s.CreatedAt
                    }).ToListAsync();
            }

            if (expenses.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return expenses;
            }
        }


        [HttpPut("UpdateExpenseById/{id}")]
        public async Task<HttpStatusCode> UpdateUser(int id, ExpenseDTO expense)
        {
            var entity = await DBContext.Expenses.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
            {
                return HttpStatusCode.NotFound;
            }
            entity.Name = expense.Name;
            entity.Category = expense.Category;
            entity.Description = expense.Description;
            entity.Price = expense.Price;
            entity.CreatedAt = expense.CreatedAt;
            entity.UpdatedAt = DateTime.Now;
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }


        [HttpGet("GetExpenseById/{id}")]
        public async Task<ActionResult<ExpenseDTO>> GetExpensesById(int id)
        {
            ExpenseDTO expense = await DBContext.Expenses.Select(a => new ExpenseDTO
            {
                Id = a.Id,
                Name = a.Name,
                Category = a.Category,
                Description = a.Description,
                Price = a.Price,
                CreatedAt = a.CreatedAt,
                UpdatedAt = DateTime.Now

            }).FirstOrDefaultAsync(a => a.Id == id);

            if (expense == null)
            {
                return NotFound();
            }
            else
            {
                return expense;
            }

        }


        [HttpDelete("DeleteExpense/{id}")]//delete based on id
        public async Task<HttpStatusCode> DeleteExpense(int id)
        {
            var entity = new Expense()
            {
                Id = id
            };
            DBContext.Expenses.Attach(entity);
            DBContext.Expenses.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }


    }
}
