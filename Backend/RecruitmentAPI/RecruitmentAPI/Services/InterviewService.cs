using RecruitmentAPI.DTOs;
using RecruitmentAPI.Models;
using RecruitmentAPI.Repositories.Interfaces;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Services
{
    public class InterviewService : IInterviewService
    {

        private readonly IInterviewRepository _interviewRepository;
        private readonly CalendarService _calendarService;



        public InterviewService(
            IInterviewRepository interviewRepository,
            CalendarService calendarService)
        {
            _interviewRepository = interviewRepository;
            _calendarService = calendarService;
        }





        public async Task<(bool Success, string Message, int? InterviewId)> ScheduleInterviewAsync(
            ScheduleInterviewDto dto,
            int interviewerId)
        {

            var application =
                await _interviewRepository.GetApplicationByIdAsync(dto.ApplicationId);



            if (application == null)
            {
                return (
                    false,
                    "Application not found.",
                    null
                );
            }





            var interview = new Interview
            {

                ApplicationId = dto.ApplicationId,

                ScheduledTime = dto.ScheduledTime,

                InterviewerId = interviewerId,

                Mode = dto.Mode,

                Notes = dto.Notes,

                Status = "Scheduled"

            };





            await _interviewRepository.AddInterviewAsync(interview);



            application.Status =
                ApplicationStatus.Interviewed;




            await _interviewRepository.SaveChangesAsync();






            // ==============================
            // CREATE CALENDAR EVENT
            // ==============================

            _calendarService.CreateInterviewEvent(

                "Interview - " + application.Job.Title,

                dto.ScheduledTime,

                dto.ScheduledTime.AddHours(1)

            );







            return (
                true,
                "Interview scheduled successfully.",
                interview.Id
            );

        }







        public async Task<List<InterviewResponseDto>> GetInterviewsForApplicationAsync(
            int applicationId)
        {

            var interviews =
                await _interviewRepository
                .GetInterviewsForApplicationAsync(applicationId);


            return interviews
                .Select(MapInterview)
                .ToList();

        }







        public async Task<List<InterviewResponseDto>> GetMyInterviewsAsync(
            int interviewerId)
        {

            var interviews =
                await _interviewRepository
                .GetMyInterviewsAsync(interviewerId);



            return interviews
                .Select(MapInterview)
                .ToList();

        }








        public async Task<List<InterviewResponseDto>> GetCandidateInterviewsAsync(
            int candidateId)
        {

            var interviews =
                await _interviewRepository
                .GetCandidateInterviewsAsync(candidateId);



            return interviews
                .Select(MapInterview)
                .ToList();

        }








        public async Task<(bool Success, string Message)> UpdateInterviewAsync(
            int interviewId,
            ScheduleInterviewDto dto)
        {

            var interview =
                await _interviewRepository.GetInterviewByIdAsync(interviewId);



            if (interview == null)
            {
                return (
                    false,
                    "Interview not found."
                );
            }



            interview.ScheduledTime =
                dto.ScheduledTime;


            interview.Mode =
                dto.Mode;


            interview.Notes =
                dto.Notes;




            await _interviewRepository.SaveChangesAsync();




            return (
                true,
                "Interview updated successfully."
            );

        }








        public async Task<(bool Success, string Message)> CancelInterviewAsync(
            int interviewId)
        {

            var interview =
                await _interviewRepository.GetInterviewByIdAsync(interviewId);



            if (interview == null)
            {
                return (
                    false,
                    "Interview not found."
                );
            }



            await _interviewRepository
                .DeleteInterviewAsync(interview);



            await _interviewRepository
                .SaveChangesAsync();



            return (
                true,
                "Interview cancelled successfully."
            );

        }








        private InterviewResponseDto MapInterview(Interview i)
        {

            return new InterviewResponseDto
            {

                Id = i.Id,

                ApplicationId = i.ApplicationId,

                CandidateName =
                    i.Application.Candidate.FullName,


                JobTitle =
                    i.Application.Job.Title,


                ScheduledTime =
                    i.ScheduledTime,


                InterviewerName =
                    i.Interviewer.FullName,


                Mode =
                    i.Mode,


                Notes =
                    i.Notes,


                Status =
                    i.Status

            };

        }

    }
}