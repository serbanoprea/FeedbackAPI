using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DBModels;
using Models.DTOs;
using Persistence.Database;

namespace FeedbackApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly FeedbackSystemContext _context;

        public AnswersController()
        {
            _context = new FeedbackSystemContext();
        }

        // GET: api/Answers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answers>>> GetAnswers()
        {
            return await _context.Answers.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Answers>> PostAnswers([FromBody] AnswerDTO answers)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(q => q.Label == answers.Question);
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.DepartmentName == answers.Department);

            if (question == null || department == null)
                return BadRequest("Either the question or the department does not exist.");


            if (answers.Answer > question.MaxPossible)
                return BadRequest("Answer not in range.");
            var answer = new Answers
            {
                DepartmentId = department.Id,
                QuestionId = question.Id,
                Answer = answers.Answer / question.MaxPossible * 100,
                Datetime = DateTime.Now,
                Question = question,
                Department = department
            };

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return Ok(answers);
        }

        [HttpGet]
        [Route("GraphAnswers/{departmentName}")]
        public async Task<ActionResult> GraphAnswers(string departmentName)
        {
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.DepartmentName.ToLower() == departmentName.ToLower());

            if (department == null)
                return NotFound();

            var answers = await _context.Answers.Where(a => a.DepartmentId == department.Id).ToListAsync();

            if (answers.Count == 0)
                return BadRequest();

            var retVal = answers
                .GroupBy(a => a.Datetime.Date)
                .Select(group =>
                        {
                            return new
                            {
                                Date = group.Key,
                                DailyAverage = group.ToList().Select(a => a.Answer).Sum() / group.Count(),
                                Answers = group.ToList().Select(a => new { a.Answer, a.Datetime.Date })
                            };
                        });

            return Ok(retVal);
        }
    }
}
