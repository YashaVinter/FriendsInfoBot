using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;


using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkApp.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public string dbPath = @"C:\Users\User\source\repos\EntityFrameworkApp\EntityFrameworkApp\TelegramBotDB.db";
        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={dbPath}");// TelegramBotDB / TelegramBotDB
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasIndex(a => a.name).IsUnique();
        }
    }


    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
    public class Address
    {
        public int Id { get; set; }
        public string? city { get; set; }
        public string? street { get; set; }
        public string home { get; set; } // may be 124/5г as variant
        public int flat { get; set; }
        public int floor { get; set; }
        public int entrance { get; set; }
        //public int PersonId { get; set; }
        public Person? Person { get; set; }
    }
    public class Person
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public int age { get; set; }
        public double debt { get; set; }
        public string? photo { get; set; }
        public string? notes { get; set; }
        public int AddressId { get; set; }
        public Address? address { get; set; }
        public string Print()
        {
            var emoji = char.ConvertFromUtf32(0x1F4A5);
            return $"\t*Имя:* {name} *Возраст:* {age}\n*Место жительства:*\n" +
                $"*Город:* {address.city}\n*Улица:* {address.street} *Дом:* {address.home}\n*Квартира:* {address.flat} *Подъезд:* {address.entrance} *Этаж:* {address.floor}" +
                $"\n{emoji}*Заметки:*{emoji}\n {notes}";
        }
        private static class DbData
        {
            public static Address YanAdr = new Address { city = "Заречный", street = "Комунальная", entrance = 1, flat = 3, floor = 3, home = "1" };
            public static Person Yan = new Person
            {
                name = "Ян",
                age = 24,
                debt = 10580,
                address = YanAdr,
                notes = "Яня, Атомщик, опелевод",
                photo = "https://sun9-53.userapi.com/sun9-66/impf/c846417/v846417975/148899/Q607m_E6GGM.jpg?size=807x563&quality=96&sign=166ea468ac706c2dfdc3a6f169c50478&type=album"
            };
            public static Address PolinaAdr = new Address { city = "Екатеринбург", street = "Новгородцевой", entrance = 12, flat = 402, floor = 2, home = "11" };
            public static Person Polina = new Person
            {
                name = "Полина",
                age = 22,
                debt = 0,
                address = PolinaAdr,
                notes = "Веган, ветош",
                photo = "https://sun9-51.userapi.com/impg/IP4LoYkuiLZg1IAgYZlINa95GH71_VaczqvT-Q/8FIpF2uicXA.jpg?size=1393x1920&quality=96&sign=2fce3dc29258a856f40441dd0d929020&type=album"
            };
            public static Address ArtemAdr = new Address { city = "Екатеринбург", street = "Волгоградская", entrance = 1, flat = 39, floor = 5, home = "184" };
            public static Person Artem = new Person
            {
                name = "Артем",
                age = 24,
                debt = 0,
                address = ArtemAdr,
                notes = "Программист, ТНН, АртБабБек",
                photo = "http://sun9-3.userapi.com/s/v1/ig2/frNJIc-HZBxVxJlmGCjpyqgwvHLyd8RcHrkvecwcUQsXohlCc21ksRqYLovjAJlkvXxRBGyXPRl3ENemvD65xlEL.jpg?size=400x598&quality=95&crop=68,0,280,419&ava=1"
            };
        }

        public bool AddAllPersons() {
            using (ApplicationContext db = new ApplicationContext())
            {
                //recreated Database
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                // adding all persons in DbData
                var persons = new List<Person> 
                { 
                    DbData.Yan, DbData.Polina, DbData.Artem
                };
                db.Persons.AddRange(persons);
                db.SaveChanges();
            }
            return true;
        }
        public bool AddPerson(Person person) {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Persons.Contains(person))
                {
                    return false;
                }
                db.Persons.Add(person);
                db.SaveChanges();
            }
                return true;
        }
        public bool UpdatePerson(Person person)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Persons.Contains(person))
                {
                    return false;
                }
                Person existperson = db.Persons.Where(a => a.Id == person.Id).FirstOrDefault();
                existperson = person;
                db.SaveChanges();
            }
            return true;
        }

        public static Person? Find(string name) {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Persons.Where(p => p.name == name).Include(p => p.address).FirstOrDefault();
            }
        }
        public List<Person> AllPesons() {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Persons.Select(p => p).ToList();
            }
        }
        public void test(string st="") {
            using (ApplicationContext db = new ApplicationContext())
            {
                string name = "Ян";
                var q1 = db.Persons.Where(p => p.name == name).Include(p => p.address);
                var q2 = db.Persons.Where(p => p.name == name);
                var q3 = db.Persons.Where(p => p.name == name).Include(p => p.address).Select(p => p.address.flat);
                var q4 = db.Persons.EntityType.FullName;
                //q4.age = 10;

                var ans1 = q1.ToQueryString();
                var ans2 = q2.ToQueryString();
                var ans3 = q3.ToQueryString();
                Console.WriteLine(@ans1);
                Console.WriteLine(@ans2);
                Console.WriteLine(@ans3);
                //var sql = ((System.Data.Objects.ObjectQuery)q1).ToTraceString();
                //var ans1 = db.GetCommand(q1).CommandText;
            }
        }
    }

}

