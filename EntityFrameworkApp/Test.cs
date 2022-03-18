﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace EntityFrameworkApp
{
    public class Test
    {
        public Test() {

        }
        public void test1() {
            EntityFrameworkApp.StateMachine.StateMachineData SMData =
                new EntityFrameworkApp.StateMachine.StateMachineData();
            EntityFrameworkApp.StateMachine.StateMachineData.States states
                = new StateMachine.StateMachineData.States();

            var home = states.home;
            var help = states.help;

            EntityFrameworkApp.StateMachine.StateMachine stateMachine =
                new EntityFrameworkApp.StateMachine.StateMachine(SMData.states, SMData.transitions, home);

            stateMachine.AddFunctionHandler(home, FriendsBot.FriendsBotData.StateTelegramActions.CaseHome);
            stateMachine.AddFunctionHandler(home, FriendsBot.FriendsBotData.StateTelegramActions.CaseHelp);

            //stateMachine.AddActionRange(SMData.states, SMData.actions);
            stateMachine.AddCriteraRange(SMData.transitions, SMData.criteria);


            Console.WriteLine("Start stateMachine");
            while (true)
            {
                string cmd = Console.ReadLine();
                stateMachine.Execute(cmd);
            }
            stateMachine.Execute("");


            //var v = stateMachine.transitionDictionary[one].Criteria;
            //stateMachine.transitionDictionary[one].Criteria = v;


            int a = 1;



        }

        public void test2() {
            Hero hero = new();
            Enemy enemy = new();
            hero.act += boo;
            enemy.act += boo;
            hero.eventHandler += Print;
            enemy.eventHandler += Print;

            hero.Hit();
            enemy.Hit();

            hero.Voice();
            enemy.Voice();


        }
        public void test3() {
            StateMachine.State state = new StateMachine.State("one");

            StateMachine.FunctionHandler v = FriendsBot.FriendsBotData.StateTelegramActions.CaseHome;
            state.functionHandler += v;

            FriendsBot.FriendsBot bot = new FriendsBot.FriendsBot("");
            bot.botCommand.command = "write";
            
            state.DoCommand(bot, bot.botCommand);

        }
        public void test4() {
            string st1 = "home";
            Regex regex = new Regex(st1,RegexOptions.IgnoreCase);

            string st2 = "HomeeEE1";
            regex.IsMatch(st2);

            
            string st = Console.ReadLine();
            while (true)
            {
                st = Console.ReadLine();
                Console.WriteLine("Compare is: " + regex.IsMatch(st));
            }


        }



        private void Hero_eventHandler(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public class Hero
        {
            public event EventHandler act;
            public MyArgs args = new MyArgs() { health = 200 };
            public void Hit() {
                Console.WriteLine($"{this} is hit by sword");
                act.Invoke(this,new());
            }
            public void Voice() {
                eventHandler(this, args);
            }
            public event EventHandler<EventArgs> eventHandler;
        }
        public class Enemy
        {
            public event EventHandler act;
            public MyArgs args = new MyArgs(){health = 100};
            public void Hit()
            {
                Console.WriteLine($"{this} is hit by sword");
                act.Invoke(this, new());
            }
            public void Voice()
            {
                eventHandler(this, args);
            }
            public event EventHandler<EventArgs> eventHandler;
        }

        public void boo(object sender, EventArgs eventArgs)
        {
            new EventArgs();
            
            if (sender is Hero)
            {
                Console.WriteLine("its hero");
            }
            if (sender is Enemy)
            {
                Console.WriteLine("its enemy");
            }

        }

        public event EventHandler<EventArgs> EventHandlerMyArgs;

        public void Print(object sender, EventArgs eventArgs) {
            if (sender is null)
                return;
            if (sender is Hero hero)
            {
                MyArgs myArgs = eventArgs as MyArgs;
                Console.WriteLine($" its a {hero} with health {myArgs.health}");
            }
            if (sender is Enemy enemy)
            {
                MyArgs myArgs = eventArgs as MyArgs;
                Console.WriteLine($" its a {enemy} with health {myArgs.health}");
            }
        }

        public class MyArgs : EventArgs
        {
            public int health = 0;
        }
    }
}
/*
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
 
 */