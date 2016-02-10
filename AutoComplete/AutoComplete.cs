using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mIDE.AutoComplete
{
    class AutoComplete
    {
        public string name;
        public string succeedingString;
        //public List<SucceedingValue> succeedingValues;
        //public Brush contextBrush;

        /*
        public Command(String Name, List<SucceedingValue> SucceedingValues, Brush ContextBrush)
        {
            this.name = Name;
            this.succeedingValues = SucceedingValues;
            this.contextBrush = ContextBrush;
        }
        */

        public AutoComplete(string Name, string SucceedingString)//, Brush ContextBrush)
        {
            this.name = Name;
            this.succeedingString = SucceedingString;
            //this.contextBrush = ContextBrush;
        }
    }

    enum SucceedingType { String, Function, Property, AT, TO, Variable, End };

    struct SucceedingValue
    {
        SucceedingType type;
        String value;

        public SucceedingValue(SucceedingType Type, String Value)
        {
            this.type = Type;
            this.value = Value;
        }
    }
}
