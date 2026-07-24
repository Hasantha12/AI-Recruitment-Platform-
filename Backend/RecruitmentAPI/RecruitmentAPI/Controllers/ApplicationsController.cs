using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RecruitmentAPI.Data;
using RecruitmentAPI.DTOs;
using RecruitmentAPI.Models;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;


        public ApplicationsController(
            AppDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }




        // POST: api/applications
        // Candidate applies to a job

        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> ApplyToJob(
            ApplyJobDto dto)
        {

            var candidateId =
                int.Parse(
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier)!
                );



            var job =
                await _context.JobPostings
                .FindAsync(dto.JobId);



            if (job == null)
            {
                return NotFound(
                    new
                    {
                        message = "Job not found."
                    });
            }




            bool alreadyApplied =
                await _context.Applications
                .AnyAsync(a =>
                    a.JobId == dto.JobId &&
                    a.CandidateId == candidateId);



            if (alreadyApplied)
            {
                return BadRequest(
                    new
                    {
                        message =
                        "You have already applied to this job."
                    });
            }





            var candidateProfile =
                await _context.CandidateProfiles
                .FirstOrDefaultAsync(
                    c => c.UserId == candidateId
                );




            decimal matchScore =
                CalculateMatchScore(
                    candidateProfile?.Skills,
                    job.RequiredSkills
                );





            var application =
                new Application
                {
                    JobId = dto.JobId,

                    CandidateId = candidateId,

                    Status =
                    ApplicationStatus.Submitted,

                    MatchScore = matchScore
                };





            _context.Applications.Add(application);


            await _context.SaveChangesAsync();





            // ===============================
            // SEND EMAIL TO CANDIDATE
            // ===============================


            var candidate =
                await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Id == candidateId
                );



            if (candidate != null)
            {

                await _emailService.SendEmailAsync(

                    candidate.Email,

                    "Application Submitted - RecruitAI",


                    $@"
                    <h2>Hello {candidate.FullName}</h2>

                    <p>
                    Your application for 
                    <b>{job.Title}</b>
                    has been submitted successfully.
                    </p>


                    <p>
                    AI Match Score:
                    <b>{matchScore}%</b>
                    </p>


                    <p>
                    Our recruitment team will review your application.
                    </p>


                    <br>

                    <b>
                    RecruitAI Team
                    </b>
                    "

                );

            }





            return Ok(
                new
                {
                    message =
                    "Application submitted successfully.",

                    applicationId =
                    application.Id,

                    matchScore
                });

        }









        // GET: api/applications/my

        [HttpGet("my")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> GetMyApplications()
        {

            var candidateId =
                int.Parse(
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier)!
                );



            var applications =
                await _context.Applications

                .Include(a => a.Job)

                .Include(a => a.Candidate)


                .Where(a =>
                    a.CandidateId == candidateId
                )


                .Select(a =>
                    new ApplicationResponseDto
                    {

                        Id = a.Id,

                        JobId = a.JobId,

                        JobTitle =
                        a.Job.Title,


                        CandidateId =
                        a.CandidateId,


                        CandidateName =
                        a.Candidate.FullName,


                        Status =
                        a.Status.ToString(),


                        MatchScore =
                        a.MatchScore,


                        AppliedDate =
                        a.AppliedDate

                    })


                .ToListAsync();



            return Ok(applications);

        }








        // Recruiter views applications

        [HttpGet("job/{jobId}")]
        [Authorize(Roles = "Recruiter,HiringManager,Admin")]
        public async Task<IActionResult> GetApplicationsForJob(
            int jobId)
        {


            var applications =
                await _context.Applications

                .Include(a => a.Job)

                .Include(a => a.Candidate)


                .Where(a =>
                    a.JobId == jobId
                )


                .OrderByDescending(
                    a => a.MatchScore
                )


                .Select(a =>
                    new ApplicationResponseDto
                    {

                        Id = a.Id,

                        JobId = a.JobId,

                        JobTitle =
                        a.Job.Title,


                        CandidateId =
                        a.CandidateId,


                        CandidateName =
                        a.Candidate.FullName,


                        Status =
                        a.Status.ToString(),


                        MatchScore =
                        a.MatchScore,


                        AppliedDate =
                        a.AppliedDate

                    })

                .ToListAsync();



            return Ok(applications);

        }









        // Update application status








        // Update application status

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Recruiter,HiringManager,Admin")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            UpdateApplicationStatusDto dto)
        {

            var application =
                await _context.Applications
                .Include(a => a.Candidate)
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.Id == id);



            if (application == null)
            {
                return NotFound(
                    new
                    {
                        message = "Application not found."
                    });
            }



            if (!Enum.TryParse<ApplicationStatus>(
                dto.Status,
                true,
                out var status))
            {
                return BadRequest(
                    new
                    {
                        message = "Invalid status value."
                    });
            }



            application.Status = status;



            await _context.SaveChangesAsync();





            // ===============================
            // SEND STATUS UPDATE EMAIL
            // ===============================


            if (application.Candidate != null)
            {

                await _emailService.SendEmailAsync(

                    application.Candidate.Email,


                    "Application Status Updated - RecruitAI",


                    $@"
            <h2>Hello {application.Candidate.FullName}</h2>


            <p>
            Your application status has been updated.
            </p>


            <p>
            Job:
            <b>{application.Job.Title}</b>
            </p>


            <p>
            New Status:
            <b>{application.Status}</b>
            </p>


            <br>


            <p>
            Thank you for using RecruitAI.
            </p>


            <b>
            RecruitAI Team
            </b>
            "

                );

            }




            return Ok(
                new
                {
                    message =
                    "Application status updated successfully."
                });

        }


        private decimal CalculateMatchScore(
            string? candidateSkills,
            string? requiredSkills)
        {

            if (
                string.IsNullOrWhiteSpace(candidateSkills)
                ||
                string.IsNullOrWhiteSpace(requiredSkills))
            {
                return 0;
            }



            var candidateSet =
                candidateSkills
                .Split(',',
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries)

                .Select(s =>
                    s.ToLowerInvariant()
                )

                .ToHashSet();




            var requiredSet =
                requiredSkills
                .Split(',',
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries)

                .Select(s =>
                    s.ToLowerInvariant()
                )

                .ToHashSet();





            if (requiredSet.Count == 0)
                return 0;





            int matches =
                requiredSet.Count(
                    skill =>
                    candidateSet.Contains(skill)
                );




            decimal coverageScore =
                (decimal)matches /
                requiredSet.Count *
                100;





            decimal bonusScore =
                candidateSet.Count > 0
                ?
                Math.Min(
                    matches * 2,
                    10
                )
                :
                0;





            return Math.Round(
                Math.Min(
                    coverageScore + bonusScore,
                    100
                ),
                2
            );

        }

    }
}