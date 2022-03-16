using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityFrameworkApp.DataBase;

namespace EntityFrameworkApp.StateMachine
{
    internal class StateMachineData
    {
        public List<string> states;
        public List<string> transitions;
        public List<Action<string>> actions;
        public List<Func<string, bool>> criteria;
        public StateMachineData() 
        {
            states = new List<string>()
            {
                States.home,
                States.find,
                States.edit,
                States.help,
                States.findPerson
            };

            var act = new StateConsoleActions();
            actions = new List<Action<string>>()
            {
                act.homeAction,
                act.findAction,
                act.editAction,
                act.helpAction,
                act.findPersonAction
            };
            Func<string,string,string> tr = (a, b) => { return a +":"+ b; };
            transitions = new List<string>()
            { 
                tr(States.home,States.find),
                tr(States.home,States.edit),
                tr(States.home,States.help),
                tr(States.find,States.home),
                tr(States.find,States.findPerson),
                tr(States.findPerson,States.home),
                tr(States.edit,States.home),
                tr(States.help,States.home)
            };
            criteria = new List<Func<string, bool>>()
            { 
                new Criteria().toFind,
                new Criteria().toEdit,
                new Criteria().toHelp,
                new Criteria().toHome,
                new Criteria().toFindPerson,
                new Criteria().toHome,
                new Criteria().toHome,
                new Criteria().toHome,
            };
        }
        public struct States
        {
            public const string home = "home";
            public const string find = "find";
            public const string edit = "edit";
            public const string help = "help";
            public const string findPerson = "findPerson";

        }
        public struct Transitions 
        { 
            
        }
        public class StateConsoleActions
        {
            public void homeAction(string cmd) {
                Console.WriteLine($"Choose mode: {States.home} {States.find} {States.edit} {States.help}");   
            }
            public void findAction(string cmd)
            {
                Console.WriteLine("Write person name. If you want see all persons write \"ALL\"");
            }
            public void editAction(string cmd)
            {
                Console.WriteLine("Write person name. If you want add or edit person");
            }
            public void helpAction(string cmd)
            {
                Console.WriteLine("Its a friendBot/ Here you can add informations about your friends");
            }
            public void findPersonAction(string cmd)
            {
                Person person = new Person().Find(cmd);
                if (person is null)
                {
                    Console.WriteLine("Person not found, try again or return home");
                    return;
                }
                Console.WriteLine($"Person found! Its a : {person.name}, notes: {person.notes}");
                Console.WriteLine($"Write peron name");
            }
        }

        public struct Criteria
        {
            public bool toHome(string st) {
                return st == States.home;
            }
            public bool toFind(string st)
            {
                return st == States.find;
            }
            public bool toEdit(string st)
            {
                return st == States.edit;
            }
            public bool toHelp(string st)
            {
                return st == States.help;
            }
            public bool toFindPerson(string st)
            {
                return st != States.home;
            }

        }
    }
}