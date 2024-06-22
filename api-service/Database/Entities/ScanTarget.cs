using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class ScanTarget
    {
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