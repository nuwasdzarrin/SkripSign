using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkripSignature.UploadRepo
{
    public class FileUploadDetails
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
        public string FileCreatedTime { get; set; }
    }
}