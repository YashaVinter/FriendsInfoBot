using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using EntityFrameworkApp.DataBase;
using System.Text.RegularExpressions;

namespace EntityFrameworkApp.Data
{
    public class StateMachineData
    {
        private static StateMachineData instance = null!;
        public States states { get; init; }
        public Transitions transitions { get; init; }
        private StateMachineData()
        {
            states = States.getInstance();
            transitions = Transitions.getInstance();
        }
        public static StateMachineData Instance()
        {
            if (instance is null)
                instance = new StateMachineData();
            return instance;
        }
        /// <summary>
        ///  Singleton
        /// </summary>
        public class States
        {
            private static States instance;
            public string home { get; init; } = "home";
            public string find { get; init; } = "find";
            public string edit { get; init; } = "edit";
            public string help { get; init; } = "help";
            public string findPerson { get; init; } = "findPerson";
            public ISet<string> stateSets { get; init; }
            private States()
            {
                stateSets = new HashSet<string>()
                {
                    home,find,edit,help,findPerson
                };
            }
            public static States getInstance() {
                if (instance is null)
                    instance = new States();
                return instance;
            }
        }
        /// <summary>
        ///  Singleton
        /// </summary>
        public class Transitions 
        {
            private static Transitions instance = null;
            public ISet<string> transitionSets { get; init; }
            private Transitions()
            {
                Func<string, string, string> tr = (a, b) => { return a + ":" + b; };
                States st = States.getInstance();

                transitionSets = new HashSet<string>()
                {
                    tr(st.home,st.find),
                    tr(st.home,st.edit),
                    tr(st.home,st.help),
                    tr(st.find,st.home),
                    tr(st.find,st.findPerson),
                    tr(st.findPerson,st.home),
                    tr(st.edit,st.home),
                    tr(st.help,st.home)
                };
            }
            public static Transitions getInstance() { 
                if(instance is null)
                    instance = new Transitions();
                return instance;
            }
        }

        //public class StateConsoleActions
        //{
        //    private States states = new States();
        //    public void homeAction(string cmd) {
        //        Console.WriteLine($"Choose mode: {states.home} {states.find} {states.edit} {states.help}");   
        //    }
        //    public void findAction(string cmd)
        //    {
        //        Console.WriteLine("Write person name. If you want see all persons write \"ALL\"");
        //    }
        //    public void editAction(string cmd)
        //    {
        //        Console.WriteLine("Write person name. If you want add or edit person");
        //    }
        //    public void helpAction(string cmd)
        //    {
        //        Console.WriteLine("Its a friendBot/ Here you can add informations about your friends");
        //    }
        //    public void findPersonAction(string cmd)
        //    {
        //        Person person = new Person().Find(cmd);
        //        if (person is null)
        //        {
        //            Console.WriteLine("Person not found, try again or return home");
        //            return;
        //        }
        //        Console.WriteLine($"Person found! Its a : {person.name}, notes: {person.notes}");
        //        Console.WriteLine($"Write peron name");
        //    }
        //}

        //public class Criteria
        //{
        //    private States states = new States();
        //    private bool isMatch(string pattern, string input) { 
        //        return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(input); 
        //    }
        //    public bool toHome(string input) {
        //        return isMatch(states.home, input);
        //    }
        //}
    }
    //public static class SingletonFactory
    //{
    //    private static readonly IDictionary<Type, object> instances;

    //    static SingletonFactory()
    //    {
    //        instances = new Dictionary<Type, object>();
    //    }

    //    public static T Create<T>(params object[] args)
    //    {
    //        Type instanceType = typeof(T);

    //        T instance;

    //        if (instances.ContainsKey(instanceType))
    //        {
    //            instance = (T)instances[instanceType];
    //        }
    //        else
    //        {
    //            instance = (T)Activator.CreateInstance(instanceType, args);

    //            instances.Add(instanceType, instance);
    //        }

    //        return instance;
    //    }
    //}
}