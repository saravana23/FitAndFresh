using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.ExtensionMethods
{
    public static class ReflectExtension
    {
        public static string GetPropValue<T>(this T item, string nameOfProperty) //we define this as a string type as I will be fetching the values of Id and Name
                                                                                 // depending on our selection, 'nameOfProperty' could be the 'Name' or 'Id'
        {
            return item.GetType().GetProperty(nameOfProperty).GetValue(item, null).ToString(); //so this line will essentially get the value of whatever property we have passed into 'GetPropValue' and return it as a string
        }

    }
}
