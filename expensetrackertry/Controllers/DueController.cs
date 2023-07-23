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
    public class DueController : ControllerBase
    {

        private readonly ExpenseTrackerContext DBContext;

        public DueController(ExpenseTrackerContext dbContext)
        {
            DBContext = dbContext;
        }


        [HttpGet("GetDues")]
        public async Task<ActionResult<List<DueDTO>>> Get()
        {
            var List = await DBContext.Dues.Select(
                s => new DueDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    DueDate = s.DueDate,
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


        [HttpPost("InsertDue")] //adding Due to db
        public async Task<HttpStatusCode> InsertDue(DueDTO Due)
        {
            var entity = new Due()
            {
                Name = Due.Name,
                Description = Due.Description,
                Price = Due.Price,
                DueDate = Due.DueDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };
            DBContext.Dues.Add(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.Created;
        }

        [HttpDelete("DeleteDue/{id}")]//delete Due based on id
        public async Task<HttpStatusCode> DeleteDue(int id)
        {
            var entity = new Due()
            {
                Id = id
            };
            DBContext.Dues.Attach(entity);
            DBContext.Dues.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }


        [HttpGet("GetDueById/{id}")]
        public async Task<ActionResult<DueDTO>> GetDueById(int id)
        {
            DueDTO due = await DBContext.Dues.Select(a => new DueDTO
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                DueDate = a.DueDate,
                CreatedAt = a.CreatedAt,
                UpdatedAt = DateTime.Now,
                

            }).FirstOrDefaultAsync(a => a.Id == id);

            if (due == null)
            {
                return NotFound();
            }
            else
            {
                return due;
            }

        }


        [HttpPut("UpdateDueById/{id}")]
        public async Task<HttpStatusCode> UpdateDue(int id, DueDTO due)
        {
            var entity = await DBContext.Dues.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
            {
                return HttpStatusCode.NotFound;
            }
            entity.Name = due.Name;
            entity.Description = due.Description;
            entity.Price = due.Price;
            entity.DueDate = due.DueDate;
            entity.CreatedAt = due.CreatedAt;
            entity.UpdatedAt = DateTime.Now;
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }



    }
}

