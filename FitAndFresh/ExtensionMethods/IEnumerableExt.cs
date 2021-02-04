using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.ExtensionMethods
{
    public static class IEnumerableExt
    {
        public static IEnumerable<SelectListItem> ItemListSelection<T>(this IEnumerable<T> items, int valueSelected)
            //first arguement of an extension method should be of the extended class
            //it should also be preceded by the 'this' keyword - which is usually used to refer to the current instance of a class, or used as a modifed in the first parameter of an extension method
            // the second parameter that I define (valueSelected) is because we're using a dropdown list - and this will be the default value
        {
            return from item in items
            
                   select new SelectListItem //this then converts items to sel
                   {
                       Text = item.GetPropValue("Name"),
                       Value = item.GetPropValue("Id"),
                       Selected = item.GetPropValue("Id").Equals(valueSelected.ToString()) //this will compare the value passed in to this method, with the value passed into 'GetPropValue'
                       // so we fetch an object, and if that object has an Id and a Name property, we'll convert it and then return a SelectListItem, which can be displayed as a dropdown item
                   };
        }
    }
}
