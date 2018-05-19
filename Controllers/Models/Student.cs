using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace studentApi.Models
{
    public class Student
    {
        public string Name { get; set; }
        public string SSN { get; set; }
        public int  courseIdLink { get; set; }

    }
}