/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class ToggleList
    {
        ToggleSet target;

        List<Object> toggleOptions;
        int currentValue;
        Object currentOption;

        protected void incrementValue()
        {   currentValue ++;
            if(currentValue == toggleOptions.Count) currentValue = 0;
            currentOption = toggleOptions[currentValue];
            target.SetValue(currentOption);
        }


    }
}
*/