using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.Common
{
    public class DataResult<T>
    {
        public T Data { get; set; }
        public int TotalCount { get; set; }
        public int TotalFilteredCount { get; set; }

        public DataResult(T data, int count)
        {
            this.Data = data;
            this.TotalCount = count;            
        }

        public DataResult(T data, int count, int filteredCount)
        {
            this.Data = data;
            this.TotalCount = count;
            this.TotalFilteredCount = filteredCount;
        }
    }    
}
