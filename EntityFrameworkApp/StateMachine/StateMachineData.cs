using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkApp.StateMachine
{
    internal class StateMachineData
    {
        public List<string> states;
        public List<string> transitions;
        StateMachineData() 
        {
            states = new List<string>()
            {
                States.home,
                States.find,
                States.edit,
                States.help
            };

            
        }


        public struct States
        {
            public const string home = "home";
            public const string find = "find";
            public const string edit = "edit";
            public const string help = "help";

        }
    }
}
