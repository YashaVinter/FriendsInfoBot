using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityFrameworkApp.StateMachine;

namespace EntityFrameworkApp.FriendsBot
{
    public class FrontendData
    {
        public class ButtonData 
        {
            private StateMachineData.States states = new StateMachineData.States();
            public readonly string home;
            public readonly string find;
            public readonly string edit;
            public readonly string help;
            public readonly string findPerson;
            public ButtonData() {
                string homeEmj = char.ConvertFromUtf32(0x1F3E0);
                string findEmj = char.ConvertFromUtf32(0x1F50D);
                string editEmj = char.ConvertFromUtf32(0x2699);
                string helpEmj = char.ConvertFromUtf32(0x1F4DA);
                home = states.home+ homeEmj;
                find = states.find + findEmj;
                edit = states.edit + editEmj;
                help = states.help + helpEmj;
            }
        }
        public class CaseText 
        {
            private StateMachineData.States states = new StateMachineData.States();
            public readonly string home;
            public readonly string find;
            public readonly string edit;
            public readonly string help;
            public readonly string findPerson;
            public CaseText() {
                home = $"Choose mode: {states.home} {states.find} {states.edit} {states.help}";
                find = "Write person name. If you want see all persons write \"ALL\"";
                findPerson = "Person not found, try again or return home";
                edit = "Write person name. If you want add or edit person";
                help = "Its a friendBot. Here you can add informations about your friends";
            }
        }

    }
}
