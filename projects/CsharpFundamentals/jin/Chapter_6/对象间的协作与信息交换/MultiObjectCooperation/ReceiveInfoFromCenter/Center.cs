using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiveInfoFromCenter
{
    class Center<T>
    {
        public List<T> forms = new List<T>();
        public T CurrentForm { get; set; }
        public void AddForm(T form)
        {
            CurrentForm = form;
            forms.Add(form);
        }
    }
}
