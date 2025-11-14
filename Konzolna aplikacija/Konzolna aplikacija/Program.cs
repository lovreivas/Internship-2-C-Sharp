namespace Konzolna_aplikacija
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var users = InitUsers();
            var trips = InitTrips();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("APLIKACIJA ZA EVIDENCIJU PUTOVANJA");
                Console.WriteLine("1 - Korisnici");
                Console.WriteLine("2 - Putovanja");
                Console.WriteLine("0 - Izlaz");
                Console.Write("Odabir: ");
                var choice = Console.ReadLine();

                if (choice == "1") UsersMenu(users, trips);
                else if (choice == "2") Console.WriteLine("Nije još dovršeno"); //TripsMenu(users, trips)
                else if (choice == "0")
                {
                    Console.WriteLine("Pozdrav!");
                    break;
                }
                else
                {
                    Console.WriteLine("Pogrešan unos. Pritisni tipku...");
                    Console.ReadKey();
                }
            }
        }
        static bool IsNameValid(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            foreach (char c in s)
            {
                if (!char.IsLetter(c)) return false;
            }
            return true;
        }

        static Dictionary<int, Tuple<string, string, DateTime, List<int>>> InitUsers()
        {
            return new Dictionary<int, Tuple<string, string, DateTime, List<int>>>()
            {
                {1, new Tuple<string,string,DateTime,List<int>>("Ante","Antic", new DateTime(1998,1,11), new List<int>{1,2})},
                {2, new Tuple<string,string,DateTime,List<int>>("Iva","Ivic", new DateTime(1990,5,3), new List<int>{3})},
                {3, new Tuple<string,string,DateTime,List<int>>("Marko","Maric", new DateTime(2004,8,20), new List<int>{4,5})}
            };
        }

        static Dictionary<int, Tuple<int, DateTime, int, double, double, double>> InitTrips()
        {
            var trips = new Dictionary<int, Tuple<int, DateTime, int, double, double, double>>();
            AddTripRaw(trips, 1, 1, new DateTime(2025, 1, 10), 200, 10, 1.5);
            AddTripRaw(trips, 2, 1, new DateTime(2025, 2, 5), 150, 8, 1.4);
            AddTripRaw(trips, 3, 2, new DateTime(2025, 3, 20), 300, 15, 1.6);
            AddTripRaw(trips, 4, 3, new DateTime(2025, 4, 8), 180, 9, 1.5);
            AddTripRaw(trips, 5, 3, new DateTime(2025, 5, 30), 250, 11, 1.55);
            return trips;
        }

        static void AddTripRaw(Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips,
                               int tripId, int userId, DateTime date, int km, double fuel, double price)
        {
            double total = Math.Round(fuel * price, 2);
            trips.Add(tripId, new Tuple<int, DateTime, int, double, double, double>(userId, date, km, fuel, price, total));
        }

        static void UsersMenu(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users,
                              Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== KORISNICI ===");
                Console.WriteLine("1 - Unos novog korisnika");
                Console.WriteLine("2 - Brisanje korisnika");
                Console.WriteLine("3 - Uređivanje korisnika");
                Console.WriteLine("4 - Pregled svih korisnika");
                Console.WriteLine("0 - Povratak");
                Console.Write("Odabir: ");
                var c = Console.ReadLine();

                if (c == "1") AddUser(users);
                else if (c == "2") DeleteUser(users, trips);
                else if (c == "3") EditUser(users);
                else if (c == "4") ViewUsers(users);
                else if (c == "0") break;
                else
                {
                    Console.WriteLine("Pogrešan unos. Pritisni tipku...");
                    Console.ReadKey();
                }
            }
        }

        static void AddUser(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
        {
            Console.Clear();
            Console.WriteLine("--- Unos novog korisnika ---");

            Console.Write("Ime: ");
            var name = Console.ReadLine();
            if (!IsNameValid(name))
            {
                Console.WriteLine("Ime može sadržavati samo slova. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            Console.Write("Prezime: ");
            var surname = Console.ReadLine();
            if (!IsNameValid(surname))
            {
                Console.WriteLine("Prezime može sadržavati samo slova. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            Console.Write("Datum rođenja (YYYY-MM-DD): ");
            var d = Console.ReadLine();
            if (!DateTime.TryParse(d, out DateTime dob) || dob.Year > 2025)
            {
                Console.WriteLine("Neispravan datum. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            int id = users.Any() ? users.Keys.Max() + 1 : 1;
            users.Add(id, new Tuple<string, string, DateTime, List<int>>(name, surname, dob, new List<int>()));

            Console.WriteLine("Korisnik dodan. Pritisni tipku...");
            Console.ReadKey();
        }

        static void DeleteUser(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users,
                               Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
        {
            Console.Clear();
            Console.WriteLine("--- Brisanje korisnika ---");
            Console.WriteLine("a) po id-u");
            Console.WriteLine("b) po imenu i prezimenu");
            Console.WriteLine("0) Povratak");
            Console.Write("Odabir: ");
            var opt = Console.ReadLine();

            if (opt == "0") return;

            if (opt == "a")
            {
                Console.Write("Unesi ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || !users.ContainsKey(id))
                {
                    Console.WriteLine("Korisnik ne postoji. Pritisni tipku...");
                    Console.ReadKey();
                    return;
                }

                Console.Write($"Sigurno obrisati {users[id].Item1} {users[id].Item2}? (da/ne): ");
                var conf = Console.ReadLine();
                if (conf?.ToLower() == "da")
                {
                    foreach (var tid in users[id].Item4.ToList()) trips.Remove(tid);
                    users.Remove(id);
                    Console.WriteLine("Obrisano.");
                }
                else Console.WriteLine("Otkazano.");

                Console.ReadKey();
            }
            else if (opt == "b")
            {
                Console.Write("Ime: ");
                var name = Console.ReadLine();
                Console.Write("Prezime: ");
                var surname = Console.ReadLine();

                int foundId = -1;
                foreach (var u in users)
                {
                    if (u.Value.Item1.Equals(name, StringComparison.OrdinalIgnoreCase)
                        && u.Value.Item2.Equals(surname, StringComparison.OrdinalIgnoreCase))
                    {
                        foundId = u.Key;
                        break;
                    }
                }

                if (foundId == -1)
                {
                    Console.WriteLine("Nije pronađen. Pritisni tipku...");
                    Console.ReadKey();
                    return;
                }

                Console.Write($"Sigurno obrisati {users[foundId].Item1} {users[foundId].Item2}? (da/ne): ");
                var conf = Console.ReadLine();

                if (conf?.ToLower() == "da")
                {
                    foreach (var tid in users[foundId].Item4.ToList()) trips.Remove(tid);
                    users.Remove(foundId);
                    Console.WriteLine("Obrisano.");
                }
                else Console.WriteLine("Otkazano.");

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Pogrešan unos.");
                Console.ReadKey();
            }
        }

        static void EditUser(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
        {
            Console.Clear();
            Console.WriteLine("--- Uređivanje korisnika ---");
            Console.WriteLine("0) Povratak");
            Console.Write("Unesi ID korisnika: ");
            var s = Console.ReadLine();
            if (s == "0") return;

            if (!int.TryParse(s, out int id) || !users.ContainsKey(id))
            {
                Console.WriteLine("Ne postoji. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            var old = users[id];

            Console.Write("Novo ime (enter za zadržati): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name) && !IsNameValid(name))
            {
                Console.WriteLine("Ime može sadržavati samo slova.");
                Console.ReadKey();
                return;
            }
            if (string.IsNullOrWhiteSpace(name)) name = old.Item1;

            Console.Write("Novo prezime (enter za zadržati): ");
            var surname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(surname) && !IsNameValid(surname))
            {
                Console.WriteLine("Prezime može sadržavati samo slova.");
                Console.ReadKey();
                return;
            }
            if (string.IsNullOrWhiteSpace(surname)) surname = old.Item2;

            Console.Write("Novi datum rođenja (YYYY-MM-DD) (enter za zadržati): ");
            var din = Console.ReadLine();

            DateTime dob = old.Item3;
            if (!string.IsNullOrWhiteSpace(din))
            {
                if (!DateTime.TryParse(din, out dob) || dob.Year > 2025)
                {
                    Console.WriteLine("Neispravan datum.");
                    Console.ReadKey();
                    return;
                }
            }

            users[id] = new Tuple<string, string, DateTime, List<int>>(name, surname, dob, old.Item4);

            Console.WriteLine("Korisnik ažuriran.");
            Console.ReadKey();
        }

        static void ViewUsers(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
        {
            Console.Clear();
            Console.WriteLine("--- Pregled korisnika ---");
            Console.WriteLine("a) ispis abecedno po prezimenu");
            Console.WriteLine("b) svi koji imaju više od 20 godina");
            Console.WriteLine("c) svi koji imaju barem 2 putovanja");
            Console.WriteLine("0) Povratak");
            Console.Write("Odabir: ");
            var opt = Console.ReadLine();

            if (opt == "0") return;

            if (opt == "a")
            {
                var list = users.ToList();
                list.Sort((x, y) => string.Compare(x.Value.Item2, y.Value.Item2, StringComparison.OrdinalIgnoreCase));

                Console.WriteLine("ID | Prezime | Ime | Datum rođenja | Br. putovanja");
                foreach (var u in list)
                {
                    Console.WriteLine("{0} | {1} | {2} | {3:d} | {4}", u.Key, u.Value.Item2, u.Value.Item1, u.Value.Item3, u.Value.Item4.Count);
                }
            }
            else if (opt == "b")
            {
                var today = DateTime.Now;

                Console.WriteLine("ID | Ime Prezime | Datum rođenja | Godine");
                foreach (var u in users)
                {
                    int age = today.Year - u.Value.Item3.Year;
                    if (today.Month < u.Value.Item3.Month || (today.Month == u.Value.Item3.Month && today.Day < u.Value.Item3.Day))
                        age--;

                    if (age > 20)
                        Console.WriteLine("{0} | {1} {2} | {3:d} | {4}", u.Key, u.Value.Item1, u.Value.Item2, u.Value.Item3, age);
                }
            }
            else if (opt == "c")
            {
                Console.WriteLine("ID | Ime Prezime | Broj putovanja");
                foreach (var u in users)
                {
                    if (u.Value.Item4.Count >= 2)
                        Console.WriteLine("{0} | {1} {2} | {3}", u.Key, u.Value.Item1, u.Value.Item2, u.Value.Item4.Count);
                }
            }
            else
            {
                Console.WriteLine("Pogrešan unos.");
            }

            Console.WriteLine("Pritisni tipku...");
            Console.ReadKey();
        }
    }   
}