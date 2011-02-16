using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class ToggleWallsButton:MenuItem
    {
        protected List<Level.WallType> toggleOptions = new List<Level.WallType>();
        protected Level.WallType currentOption;
        int currentValue;
        Level target;
        String basicId;

            

        public ToggleWallsButton(Level Target)
        {
            target = Target;
            basicId = "Wall Settings: ";
            
        }

        public void addOption(String Id, Level.WallType newOption)
        {
            
            toggleOptions.Add(newOption);
        }

        public override void execute()
        {   currentValue ++;
            if(currentValue == toggleOptions.Count) currentValue = 0;
            currentOption = toggleOptions[currentValue];
            id = basicId + currentOption.ToString();
            target.SetValue(currentOption);
        }


    }
}
