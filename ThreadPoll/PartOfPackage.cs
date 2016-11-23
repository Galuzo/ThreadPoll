using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoll
{
    class PartOfPackage
    {
        public PartOfPackage(long size, long offset)
        {
            this.size = size;
            this.offset = offset;
        }
        public long size { get; set; }
        public long offset { get; set; }
    }
}
