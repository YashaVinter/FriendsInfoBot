using System;

using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkApp.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public string dbPath = @"C:\Users\User\source\repos\FriendsInfoBot\database\TelegramBotDB.db";
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
            public static Address IvanAdr = new Address { city = "Заречный", street = "Ленина", entrance = 1, flat = 3, floor = 3, home = "1" };
            public static Person Ivan = new Person
            {
                name = "Ivan",
                age = 24,
                debt = 1000,
                address = IvanAdr,
                notes = "Ivan, Атомщик, владелец опеля",
                photo = "https://w7.pngwing.com/pngs/6/214/png-transparent-silhouette-silhouette-animals-monochrome-man-silhouette.png"
            };
            public static Address AnnAdr = new Address { city = "Екатеринбург", street = "Ленина", entrance = 12, flat = 402, floor = 2, home = "11" };
            public static Person Ann = new Person
            {
                name = "Ann",
                age = 22,
                debt = 0,
                address = AnnAdr,
                notes = "Веган",
                photo = "https://previews.123rf.com/images/jemastock/jemastock1804/jemastock180408399/100468063-young-woman-cartoon-with-casual-clothes-icon.jpg"
            };
            public static Address BobAdr = new Address { city = "Екатеринбург", street = "Ленина", entrance = 1, flat = 39, floor = 5, home = "184" };
            public static Person Bob = new Person
            {
                name = "Bob",
                age = 24,
                debt = 0,
                address = BobAdr,
                notes = "Программист, ТНН",
                photo = "https://thumbs.dreamstime.com/b/smiling-man-cartoon-style-character-isolated-white-background-smiling-man-cartoon-style-179530902.jpg"
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
                    DbData.Ivan, DbData.Ann, DbData.Bob
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
                string name = "Ivan";
                var q1 = db.Persons.Where(p => p.name == name).Include(p => p.address);
                var q2 = db.Persons.Where(p => p.name == name);
                var q3 = db.Persons.Where(p => p.name == name).Include(p => p.address).Select(p => p.address.flat);
                var q4 = db.Persons.EntityType.FullName;

                var ans1 = q1.ToQueryString();
                var ans2 = q2.ToQueryString();
                var ans3 = q3.ToQueryString();
                Console.WriteLine(@ans1);
                Console.WriteLine(@ans2);
                Console.WriteLine(@ans3);
            }
        }
    }

}

