using Google.Apis.Calendar.v3.Data;

namespace RecruitmentAPI.Services
{
    public class CalendarService
    {

        public string CreateInterviewEvent(
            string title,
            DateTime startTime,
            DateTime endTime)
        {

            Event calendarEvent = new Event
            {
                Summary = title,

                Description = "Interview scheduled through Recruitment API",

                Start = new EventDateTime
                {
                    DateTime = startTime,
                    TimeZone = "Asia/Colombo"
                },

                End = new EventDateTime
                {
                    DateTime = endTime,
                    TimeZone = "Asia/Colombo"
                }

            };


            // Google Calendar API connection
            // will be added after OAuth setup


            return "Calendar event created successfully";

        }

    }
}