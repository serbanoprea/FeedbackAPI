using System;
using System.Collections.Generic;

namespace Models.DBModels
{
    public partial class Questions
    {
        public Questions()
        {
            Answers = new HashSet<Answers>();
        }

        public int Id { get; set; }
        public string Question { get; set; }
        public string Label { get; set; }
        public double MaxPossible { get; set; }
        public double MinPossible { get; set; }
        public double PossibleMean { get; set; }

        public virtual ICollection<Answers> Answers { get; set; }
    }
}
