using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMaple.Internationalization.Models
{

    public class Iso639_MacroLanguage
    {
        /// <summary>
        /// Iso639-3 code for macro
        /// </summary>
        public required string M_Id { get; set; }

        /// <summary>
        /// Iso639-3 code for individual language
        /// </summary>
        public string? I_Id { get; set; }
        public string? I_Status { get; set; }
    }


    public class Iso639
    {
        public required string Id { get; set; }
        public string? Part2B { get; set; }
        public string? Part2T { get; set; }
        public string? Part1 { get; set; }
        public string? Scope { get; set; }
        public string? Language_Type { get; set; }
        public string? Ref_Name { get; set; }
        public string? Comment { get; set; }
    }

}
