using System;
using System.Collections.Generic;

namespace Models.DBModels
{
    public partial class Departments
    {
        public Departments()
        {
            Answers = new HashSet<Answers>();
        }

        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }

        public virtual ICollection<Answers> Answers { get; set; }
    }
}
