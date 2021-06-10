﻿using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeOrganiser.Models
{
    public class AnuntModel
    {
      
        public int Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
       

       
    }
}
