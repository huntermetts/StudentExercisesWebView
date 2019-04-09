﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesWebView.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A first name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "A last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Instructor Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [Required(ErrorMessage = "A slack handle is required")]
        [Display(Name = "Slack")]
        public string SlackHandle { get; set; }
        [Required(ErrorMessage = "A cohort is required")]
        [Display(Name = "Cohort")]
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
    }
}
