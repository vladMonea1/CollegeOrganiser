using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class ModelFeedCurs
    {
        public List <AnuntModelCurs>  AnunturileCursului { get; set; }
        public List<FileModel> TemeleCursului { get; set; }
        public List<CourseAttendances> Prezente { get; set; }

        public List<CoursesHeld> CursurileCareSeTin { get; set; }

        public int CourseId { get; set; }
    }
}
