using System;
using System.Collections.Generic;

namespace Models.DBModels
{
    public partial class Answers
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int QuestionId { get; set; }
        public double Answer { get; set; }
        public DateTime Datetime { get; set; }

        public virtual Departments Department { get; set; }
        public virtual Questions Question { get; set; }
    }
}
