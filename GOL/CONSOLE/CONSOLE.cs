using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CONSOLE
{
    class CONSOLE
    {
        //Main Console method...
        static void Main(string[] args)
        {
            LIBRARY.LIFE myGOL = new LIBRARY.LIFE();

            //myGOL.Welcome();

            myGOL.StartMenuGo();
        }
    }
}