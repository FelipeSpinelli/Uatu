using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Uatu.API.Models.Request
{
    public class SearchLogRequestViewModel : IValidatableObject
    {
        public string Application { get; set; }
        public string RequestKey { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime? Since { get; set; }
        public DateTime? Until { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            Since = Since ?? DateTime.Now.AddDays(-1);
            Until = Until ?? DateTime.Now;

            if (Since > Until)
            {
                results.Add(new ValidationResult("Search until date cannot be less than until date!"));
            }

            return results;
        }
    }
}
