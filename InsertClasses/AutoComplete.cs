using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace mIDE.InsertClasses
{
    public class InstructionFramework
    {
        public string Name { get; }
        public string Framework { get; }
        public string Result { get; }
        //public List<SucceedingValue> succeedingValues;
        public Color ContextColor { get; }

        public InstructionFramework(string Name, string Framework, string Result, Color ContextColor)
        {
            this.Name = Name;
            this.Framework = Framework;
            this.Result = Result;
            this.ContextColor = ContextColor;
        }
    }

    /*enum SucceedingType { String, Function, Property, AT, TO, Variable, End };

    struct SucceedingValue
    {
        SucceedingType type;
        String value;

        public SucceedingValue(SucceedingType Type, String Value)
        {
            this.type = Type;
            this.value = Value;
        }
    }*/
}
