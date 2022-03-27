using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityFrameworkApp.Data
{
    public class FrontendData
    {
        public class ButtonData 
        {
            public Dictionary<string,string> stateToButtonText { get; init; }

            //private StateMachineData.States states = new StateMachineData.States();
            //public readonly string home;
            //public readonly string find;
            //public readonly string edit;
            //public readonly string help;
            //public readonly string findPerson;
            //public ButtonData() {
            //    string homeEmj = char.ConvertFromUtf32(0x1F3E0);
            //    string findEmj = char.ConvertFromUtf32(0x1F50D);
            //    string editEmj = char.ConvertFromUtf32(0x2699);
            //    string helpEmj = char.ConvertFromUtf32(0x1F4DA);
            //    home = states.home+ homeEmj;
            //    find = states.find + findEmj;
            //    edit = states.edit + editEmj;
            //    help = states.help + helpEmj;
            //}
            public ButtonData(StateMachineData.States states)
            {
                string homeEmj = char.ConvertFromUtf32(0x1F3E0);
                string findEmj = char.ConvertFromUtf32(0x1F50D);
                string editEmj = char.ConvertFromUtf32(0x2699);
                string helpEmj = char.ConvertFromUtf32(0x1F4DA);
                var createPair = delegate (string name, string emoji)
                {
                    return new KeyValuePair<string, string>(name, name + emoji);
                };
                IEnumerable<KeyValuePair<string, string>> collection = new[]
                {
                    createPair(states.home,homeEmj),
                    createPair(states.find,findEmj),
                    createPair(states.edit,editEmj),
                    createPair(states.help,helpEmj),
                };
                stateToButtonText = new Dictionary<string, string>(collection);
            }

        }
        public class CaseText 
        {
            public Dictionary<string, string> stateToCaseText { get; set; }
            //private StateMachineData.States states = new StateMachineData.States();
            //public readonly string home;
            //public readonly string find;
            //public readonly string edit;
            //public readonly string help;
            //public readonly string findPerson;
            //public CaseText() {
            //    home = $"Choose mode: {states.home} {states.find} {states.edit} {states.help}";
            //    find = "Write person name, If you want see all persons write \"ALL\"";
            //    findPerson = "Person not found, try again or return home";
            //    edit = "EDIT Write person name, If you want add or edit person";
            //    help = "HELP Its a friendBot, Here you can add informations about your friends";
            //}
            public CaseText(StateMachineData.States states)
            {
                string home = $"Choose mode: {states.home} {states.find} {states.edit} {states.help}";
                string find = "Write person name, If you want see all persons write \"ALL\"";
                string findPerson = "Person not found, try again or return home";
                string edit = "EDIT Write person name, If you want add or edit person";
                string help = "HELP Its a friendBot, Here you can add informations about your friends";

                stateToCaseText = new Dictionary<string, string>() 
                {
                    { states.home,home},
                    { states.find,find},
                    { states.edit,edit},
                    { states.help,help},
                    { states.findPerson,findPerson}
                };
            }
        }

    }
}
