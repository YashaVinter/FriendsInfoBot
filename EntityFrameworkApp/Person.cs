using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Person> Persons => Set<Person>();
        public DbSet<Address> Addresses => Set<Address>();
        //public DbSet<Add>
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TelegramBotDB.db");
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
        public int home { get; set; } // may be 124/5г as variant
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
            return $"\t*Персонаж:* {name} *Возраст:* {age}\n*Место жительства:*\n" +
                $"*Город:* {address.city}\n*Улица:* {address.street} *Дом:* {address.home}\n*Квартира:* {address.flat} *Подъезд:* {address.entrance} *Этаж:* {address.floor}" +
                $"\n{char.ConvertFromUtf32(0x1F4A5)}*Заметки:*{char.ConvertFromUtf32(0x1F4A5)}\n {notes}";
        }
        private static List<Person> persons = new List<Person> {
            new Person {
                name = "Артем", age = 24, debt = 0, address = { city = "Екатеринбург", street = " Волгоградская", entrance = 1, flat = 39, floor = 5, home = 184 },
                photo = "http://sun9-3.userapi.com/s/v1/ig2/frNJIc-HZBxVxJlmGCjpyqgwvHLyd8RcHrkvecwcUQsXohlCc21ksRqYLovjAJlkvXxRBGyXPRl3ENemvD65xlEL.jpg?size=400x598&quality=95&crop=68,0,280,419&ava=1",
                notes = "Программист, ТНН, АртБабБек"
            }
        };
        public void test() {
            using (ApplicationContext db = new ApplicationContext())
            {
                // создаем два объекта User
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();


                Address address1 = new Address { city = "Екатеринбург", street = "Волгоградская", entrance = 1, flat = 39, floor = 5, home = 184 };
                Person Artem = new Person { name = "Артем", age = 24, debt = 0, address = address1, notes = "Программист, ТНН, АртБабБек",
                    photo = "http://sun9-3.userapi.com/s/v1/ig2/frNJIc-HZBxVxJlmGCjpyqgwvHLyd8RcHrkvecwcUQsXohlCc21ksRqYLovjAJlkvXxRBGyXPRl3ENemvD65xlEL.jpg?size=400x598&quality=95&crop=68,0,280,419&ava=1" };

                // добавляем их в бд

                db.Persons.Add(Artem);

                db.SaveChanges();
                Console.WriteLine("Объекты успешно сохранены");

                var art = db.Persons.FirstOrDefault();
                art = db.Persons.Where(a => a.name == "Артем").FirstOrDefault();

                var ph = art.address.street;

                // получаем объекты из бд и выводим на консоль
                var users = db.Users.ToList();
                var persons = db.Persons.ToList();
                Console.WriteLine("Список объектов:");
                foreach (Person p in persons)
                {
                    Console.WriteLine($" {p.Id} {p.name} {p.age}  ");
                }
            }

        }
        public Person? Find(string name) {
            Person person;
            Address address;
            using (ApplicationContext db = new ApplicationContext())
            {
                address = db.Addresses.FirstOrDefault();
                person = db.Persons.FirstOrDefault();
                person = db.Persons.Where(p => p.name == name).FirstOrDefault();
            }
            return person;
        }
    }

}


//using (ApplicationContext db = new ApplicationContext())
//{
//    // создаем два объекта User
//    User tom = new User { Name = "Tom", Age = 33 };
//    User alice = new User { Name = "Alice", Age = 26 };
//    User bob = new User {Name="Bob", Age = 21 };

//    // добавляем их в бд
//    db.Users.Add(tom);
//    db.Users.Add(alice);
//    db.SaveChanges();
//    Console.WriteLine("Объекты успешно сохранены");


//    db.Add(bob);
//    db.Remove(tom);
//    db.Remove(alice);
//    db.SaveChanges();

//    // получаем объекты из бд и выводим на консоль
//    var users = db.Users.ToList();
//    Console.WriteLine("Список объектов:");
//    foreach (User u in users)
//    {
//        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
//    }
//}