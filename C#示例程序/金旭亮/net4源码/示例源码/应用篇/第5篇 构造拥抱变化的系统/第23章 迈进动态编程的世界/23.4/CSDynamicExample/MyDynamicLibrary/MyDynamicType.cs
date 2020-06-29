using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Xml.Linq;

namespace MyDynamicLibrary
{
    public class MyDynamicType : DynamicObject
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            
           return _data.TryGetValue(binder.Name, out result);

        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _data[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;
            if (!_data.ContainsKey(binder.Name))  return false;
            dynamic target = _data[binder.Name];
            switch (args.Length)
            {
                case 0: target(this); break;
                case 1: target(this, args[0]); break;
                default: throw new NotImplementedException();
            }
            return true;
        }

        public XElement ToXml()
        {
            return new XElement("MyDynamicType",
                    from x in _data
                    select new XElement(x.Key,x.Value));

        }

        public static void VisitDynamicObject(dynamic obj)
        {
            Console.WriteLine("{0}有{1}年的历史", obj.Name, obj.Age);
        }
    }
}
