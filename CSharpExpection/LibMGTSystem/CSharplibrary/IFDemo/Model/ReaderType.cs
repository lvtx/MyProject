using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ReaderType
    {
        public int ReaderTypeId { get; set; }
        public string ReaderTypeName { get; set; }

        public override string ToString()
        {
            return this.ReaderTypeName;
        }
    }
}
