using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component
{
    public enum ScaleDirection
    {
        None = 0,
        X_Axis = 1,
        Y_Axis = 2,
        Z_Axis = 3
    }
    public enum ScaleType
    {
        Continuous,
        Categorical
    }
    public enum ScalePrimary
    {
        Primary,
        Secondary
    }

    public enum MtbColor
    {
        DarkSkyBlue = 127, Wine = 28, Yellow = 7, Spinach = 58, Copper = 116,
        LightSkyBlue = 78, Salmon = 29, Lemon = 45, SpringGreen = 123, Wheat = 35,
        Eggplant = 73, DarkRed = 8, Olived = 49, Forest = 57, TerraCotta = 26
    }
    public enum Align
    {
        Left, Center, Right
    }

}
