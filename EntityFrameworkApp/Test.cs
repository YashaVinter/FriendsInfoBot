using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkAppTest
{
    public class Test
    {
        public Test() {

            string tr(string f, string s) { return f +':'+ s; }

            string one = "one";
            string two = "two";
            string three = "three";
            string four = "four";
            string onetwo = tr(one,two);
            string onethree = tr(one, three);
            string twofour = tr(two, four);
            string threefour = tr(three, four);

            
            List<string> states = new List<string>() 
            { 
                one,
                two,
                three,
                four
            };
            List<string> transitions = new List<string>()
            {
                tr(one,two),
                tr(two,three),
                tr(three,four),
                tr(four,one)
            };


            StateMachineTest stateMachine = new StateMachineTest(states, transitions, one);
            Action<string> a1 = (s) => { Console.WriteLine($"Its actions from state 1. Your command was {s}"); };
            Action<string> a2 = (s) => { Console.WriteLine($"Its actions from state 2. Your command was {s}"); };
            Action<string> a3 = (s) => { Console.WriteLine($"Its actions from state 3. Your command was {s}"); };
            Action<string> a4 = (s) => { Console.WriteLine($"Its actions from state 4. Your command was {s}"); };

            var actionsList = new List<Action<string>>() { a1, a2, a3, a4 };

            Func<string, bool> func12 = (s) => { return s == "toTwo"; };
            Func<string, bool> func13 = (s) => { return s == "toThree"; };
            Func<string, bool> func24 = (s) => { return s == "toFour"; };
            Func<string, bool> func34 = (s) => { return s == "toOne"; };

            var criteraList = new List<Func<string, bool>> { func12, func13,func24,func34 };

            stateMachine.AddActionRange(states, actionsList);
            stateMachine.AddCriteraRange(transitions, criteraList);


            Console.WriteLine("Start stateMachine");
            while (true)
            {
                string cmd = Console.ReadLine();
                stateMachine.Execute(cmd);
            }
            stateMachine.Execute("");


            var v = stateMachine.transitionDictionary[one].Criteria;
            stateMachine.transitionDictionary[one].Criteria = v;


            int a = 1;



        }

    }
}
