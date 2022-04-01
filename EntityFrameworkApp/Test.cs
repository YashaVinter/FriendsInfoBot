using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StateMachineLibrary;
using EntityFrameworkApp.Data;
using EntityFrameworkApp.FriendsBotLibrary;
using EntityFrameworkApp.DataBase;

using Telegram.Bot.Types.ReplyMarkups;

namespace EntityFrameworkApp
{
    public class Test
    {
        public Test() {

        }
        public void test1() {
            //StateMachineData SMData =
            //    new StateMachineData();
            //StateMachineData.States states
            //    = new StateMachineData.States();

            //var home = states.home;
            //var help = states.help;

            //StateMachine stateMachine =
            //   new StateMachine(SMData.states, SMData.transitions, home);

            //stateMachine.AddFunctionHandler(home, FriendsBotData.StateTelegramActions.CaseHome);
            //stateMachine.AddFunctionHandler(home, FriendsBotData.StateTelegramActions.CaseHelp);

            //stateMachine.AddActionRange(SMData.states, SMData.actions);
            //stateMachine.AddCriteraRange(SMData.transitions, SMData.criteria);


            //Console.WriteLine("Start stateMachine");
            //while (true)
            //{
            //    string cmd = Console.ReadLine();
            //    stateMachine.Execute(cmd);
            //}
            //stateMachine.Execute("");


            //var v = stateMachine.transitionDictionary[one].Criteria;
            //stateMachine.transitionDictionary[one].Criteria = v;


            //int a = 1;



        }

        public void test2() {
            Hero hero = new();
            Enemy enemy = new();
            hero.act += boo;
            enemy.act += boo;
            hero.eventHandler += Print;
            enemy.eventHandler += Print;



            HeroBase heroBase = new HeroBase();
            heroBase.Print1();
            heroBase.Print2();
            hero.Print1();
            hero.Print2();

            heroBase = new Hero() as HeroBase;
            heroBase.Print1();
            heroBase.Print2();
            hero =  new HeroBase() as Hero;
            hero.Print1();
            hero.Print2();

            hero.Hit();
            enemy.Hit();

            hero.Voice();
            enemy.Voice();


        }
        public void test3() {
            //State state = new State("one");

            //FunctionHandler v = FriendsBotData.StateTelegramActions.CaseHome;
            //state.functionHandler += v;

            //FriendsBot bot = new FriendsBot("");
            //bot.botCommand.command = "write";

            //state.DoCommand(bot, bot.botCommand);

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

        public void test5() {
            List<int> list = new List<int>() { 5, 4, 1, 3 };
            Action<IEnumerable<int>> show = delegate(IEnumerable<int> collection) {
                foreach (var item in collection)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
            };

            SortedSet<string> ss = new SortedSet<string>()
            {
                "one","four","three"
            };
            ss.Add("two");
            ss.Add("one");
            //show(ss);
            foreach (var item in ss)
            {
                Console.WriteLine(item + " ");
            }
            Dictionary<string, int> d = new Dictionary<string, int>() {
                { "one",1},
                {"three",3 }
            };
            d.Add("two", 2);
            //d.Add("one", 1);
            double d1 = 0.10;
            decimal dec = 0.10m;
            for (int i = 0; i < 10; i++)
            {
                d1 += 0.1;
            }
            for (int i = 0; i < 10; i++)
            {
                dec += 0.1m;
            }
            //d1 += d1;
            //dec += dec;
            Console.WriteLine(d1*10);
            Console.WriteLine(dec * 10);
            
        }
        public void test6() {
            var list = new List<string>()
            {
                "Tom","Bob","Tim","Yasha","Artem"
            };
            var num = new List<int>()
            {
                10,5,51,70,70,15156,128,753,9
            };

            var q = list.Where( p => p.StartsWith('T')).OrderBy(p => p).ToList();
            var q1 = num.Where(n => n > 100).OrderByDescending(n => n);
            var q11 = q1.Average();
            var q12 = q1.Sum();
            var q13 = q1.All(n => n > 10000);
            var q14 = q1.Any(n => n > 10000);

            Predicate<string> d1 = null;
            Func<string, bool, string> d2 = null;
            var v1 = d1 is Func<string, bool, string>;
            var v2 = d2 is Predicate<string>;

        }
        public void test7() {
            
        }

        public void test8() {
            //StateMachineData.Transitions transitions = StateMachineData.Transitions.getInstance();
            //var a = transitions.transitions;
            
            //var b = StateMachineData.Transitions.tr1;
        }
        public void test9() {
            string emj = "😁";
            var h = J3QQ4.Emoji.House;
            var f = J3QQ4.Emoji.Mag_Right;
            var e = J3QQ4.Emoji.Pencil;
            var help = J3QQ4.Emoji.Books;
        }

        public void test10() {
            //var states = StateMachineData.States.getInstance();
            //string homeEmj = char.ConvertFromUtf32(0x1F3E0);
            //string findEmj = char.ConvertFromUtf32(0x1F50D);
            //string editEmj = char.ConvertFromUtf32(0x2699);
            //string helpEmj = char.ConvertFromUtf32(0x1F4DA);

            //var homeButton= new EntityFrameworkApp.Data.ButtonData2(states.home, homeEmj);
            //var findButton = new EntityFrameworkApp.Data.ButtonData2(states.find, findEmj);
            //var editButton = new EntityFrameworkApp.Data.ButtonData2(states.edit, editEmj);
            //var helpButton = new EntityFrameworkApp.Data.ButtonData2(states.help, helpEmj);
            //var buttonsSet = new HashSet<EntityFrameworkApp.Data.ButtonData2>() 
            //{
            //    homeButton,
            //    findButton,
            //    editButton,
            //    helpButton
            //};
            //var buttonsBuilder2 = new EntityFrameworkApp.Data.KeyboardBuilder2(buttonsSet);
            //var homeButtonsList = new List<string>() 
            //{
            //    states.edit,
            //    states.help,
            //    states.home,
            //    states.find
            //};
            //var keyboardByState = new Dictionary<string, IReplyMarkup>();
            //keyboardByState.Add(states.home, buttonsBuilder2.BuildKeyboard(homeButtonsList));

        }
        public void test11() {
            JObject o = JToken.FromObject(new Dictionary<string, string>() { {"one","two" } , { "three", "four" } }) as JObject;

            Person person = new Person(25, "Stiven");
            string json = JsonConvert.SerializeObject(person);
            Console.WriteLine(json);
            Person? restoredPerson = JsonConvert.DeserializeObject<Person?>(json);
            string path = @"C:\Users\User\source\repos\EntityFrameworkApp\EntityFrameworkApp\JsonObjects\";

            Person p1 = new Person(40, "Ben");
            p1.act = delegate (string st) { Console.WriteLine(st); };

            JsonSerializer jsonSerializer1 = new JsonSerializer();
            //jsonSerializer1.

            var v4 = JsonConvert.SerializeObject(p1);

            //JObject jobj = JObject.Parse(File.ReadAllText(path));

            //var v2 = new JToken(json);
            JObject obj1 = JToken.FromObject(person) as JObject;
            var obj2 = new JProperty("person", obj1);
            JArray jArray = new JArray();
            jArray.Add(obj1);
            jArray.Add(obj1);

            File.WriteAllText(path + "user.json", jArray.ToString());
            foreach (var item in jArray)
            {
                Person p = item.ToObject<Person>();
            }
            //
            var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

            var button1 = new FrontendDataNew.ButtonData(StateMachineData.States.getInstance().home, J3QQ4.Emoji.House);
            var button2 = new FrontendDataNew.ButtonData(StateMachineData.States.getInstance().find, J3QQ4.Emoji.Mag_Right);

            var button1JSON = JsonConvert.SerializeObject(button1,Newtonsoft.Json.Formatting.Indented,settings);
            var button2JSON = JsonConvert.SerializeObject(button2, Newtonsoft.Json.Formatting.Indented, settings);

            JsonSerializer jsonSerializer = new JsonSerializer() { TypeNameHandling = TypeNameHandling.All };
            var b1 = JToken.FromObject(button1, jsonSerializer);
            var b2 = JToken.FromObject(button2, jsonSerializer);
            JArray jArray1 = new JArray();
            jArray1.Add(b1);
            jArray1.Add(b2);

            File.WriteAllText(path+"buttons.json", jArray1.ToString());


            //JObject jobj = JObject.Parse(File.ReadAllText(path + "buttons.json"));
            JArray jArray2 = JArray.Parse(File.ReadAllText(path + "buttons.json"));
            foreach (var token in jArray2)
            {
                var b = token.ToObject<FrontendDataNew.ButtonData>(jsonSerializer);
            }

            var b3 = JsonConvert.DeserializeObject<FrontendDataNew.ButtonData>(button1JSON,settings);

        }
        public void test12() {
            string path = @"C:\Users\User\source\repos\EntityFrameworkApp\EntityFrameworkApp\XML\";

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path + "people.xml");

            XmlElement? xRoot = xDoc.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {

            }
        }

        private void Hero_eventHandler(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public class HeroBase
        { 
            public string name { get; set; }
            public virtual void Print1() {
                Console.WriteLine(this.ToString + " "+ name + " print1");
            }
            public void Print2()
            {
                Console.WriteLine(this.ToString + " " + name + " print2");
            }
        }
        public class Hero :HeroBase
        {
            public override void Print1()
            {
                Console.WriteLine(this.ToString + " " + name + " print1");
            }
            public void Print2()
            {
                Console.WriteLine(this.ToString + " " + name + " print2");
            }
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
        public class Enemy : IDisposable
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

            public void Dispose()
            {
                GC.SuppressFinalize(this);   
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
        public class Person
        {
            public int age { get; set; }
            public string name{ get; set; }
            public Action<string> act { get; set; }
            public Person(int a, string n)
            {
                age = a;
                name = n;
            }
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
