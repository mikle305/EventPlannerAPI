using Microsoft.Extensions.Logging.Abstractions;

namespace EventPlannerAPI
{
    public class Event
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime Deadline { get; set; }

        public ReadinessState Status { get; set; }
        
        public RepeatingState RepeatingFreq { get; set; }

        public enum ReadinessState
        {
            Ended,
            Failed,
            InProcess
        }
        
        public enum RepeatingState
        {
            
            
        }
        
    }
}