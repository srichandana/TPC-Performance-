using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Common.Enumerations
{
    public enum SubjectEnums
    {
        [Display(Description = "Art/Rec")]
        ArtRecreation = 1,
        Concept = 2,
        History = 3,
        [Display(Description = "Lang/Lit")]
        LanguageLiterature = 4,
        Science = 5,
        [Display(Description = "Social Studies")]
        SocialStudies = 6
    }
}
