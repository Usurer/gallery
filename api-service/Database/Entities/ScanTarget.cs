using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class ScanTarget
    {
        // Id is PK by default convention, but I'll keep the attr
        [Key]
        public long Id
        {
            get; set;
        }

        public string Path
        {
            get; set;
        }

        public bool IsScanned
        {
            get; set;
        }

        public bool IsInvalid
        {
            get; set;
        }
    }
}