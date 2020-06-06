using System;

namespace ChariAPI.Models
{
    public class OrgSearchCriteria
    {
        public Nullable<int> pageSize { get; set; }
        public string searchTerm { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string minRating { get; set; } //0-4
        public string maxRating { get; set; } //0-4
        public string scopeOfWork { get; set; } //ALL (default), REGIONAL, NATIONAL, or INTERNATIONAL
        public string sort { get; set; } //Properties: NAME, RATING, or RELEVANCE and Direction Indicator: ASC or DESC - Example: "RATING:DESC" *Cannot use RELEVANCE property without specifying searchTerm
    }
}
