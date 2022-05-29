using System;

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
    }
}