using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoll
{
    class CopyFile
    {
        
        public List<PartOfPackage> slicedPart(long sizeOfFile,int countOfParts)
        {
            long sizeOfPart = (long)sizeOfFile / countOfParts;
            long lastSize = sizeOfPart + (sizeOfFile - (countOfParts * sizeOfPart));
            List<PartOfPackage> list = new List<PartOfPackage>();
            for (int i=0;i<countOfParts;i++)
            {
                list.Add(new PartOfPackage(sizeOfPart, i * sizeOfPart));
            }
            list[countOfParts-1].size = lastSize;
            return list;
        }
    }
}
