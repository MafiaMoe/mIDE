using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace mIDE.InsertClasses
{
    class AutoComplete
    {
        public string name;
        public string succeedingString;
        //public List<SucceedingValue> succeedingValues;
        public Color contextColor;

        /*
        public Command(String Name, List<SucceedingValue> SucceedingValues, Brush ContextBrush)
        {
            this.name = Name;
            this.succeedingValues = SucceedingValues;
            this.contextBrush = ContextBrush;
        }
        */

        public AutoComplete(string Name, string SucceedingString, Color ContextColor)
        {
            this.name = Name;
            this.succeedingString = SucceedingString;
            this.contextColor = ContextColor;
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
