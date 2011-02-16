using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class ToggleLimitsButton : MenuItem
    {
        protected List<int> toggleOptions = new List<int>();
        protected int currentOption;
        int currentValue;
        Level target;
        String basicId;
        Surface.SurfaceType type;



        public ToggleLimitsButton(Level Target, Surface.SurfaceType Type)
        {
            target = Target;
            type = Type;
            switch (type)
            {
                case Surface.SurfaceType.Absorbant:
                    basicId = "Absorbant Walls: ";
                    break;
                case Surface.SurfaceType.Refractive:
                    basicId = "Refractive Walls: ";
                    break;

                case Surface.SurfaceType.Reflective:
                    basicId = "Reflective Walls: ";
                    break;
                default:
                    basicId = "Whaats?";
                    break;

                    
            }

            addOption(0); addOption(1); addOption(2); addOption(3); addOption(5); addOption(8);
            currentValue = 1;
            execute();

            

        }

        public void addOption(int newOption)
        {
            toggleOptions.Add(newOption);
        }

        public override void execute()
        {
            currentValue++;
            if (currentValue == toggleOptions.Count) currentValue = 0;
            currentOption = toggleOptions[currentValue];
            id = basicId + currentOption.ToString();
            target.SetValue(type, currentOption);
        }


    }
}