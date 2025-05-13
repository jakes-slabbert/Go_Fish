namespace GoFish.Models
{
    public class GanttTaskUpdateDto // This comes in json from gantt.
    {
        public string id { get; set; }

        public string text { get; set; }

        public string start_date { get; set; }

        public string end_date { get; set; }

        public double progress { get; set; }

        public string parent { get; set; }
    }
}
