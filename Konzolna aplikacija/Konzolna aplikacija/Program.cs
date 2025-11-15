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
                else if (choice == "2") TripsMenu(users, trips);
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

        static void AddTripRaw(Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips, int tripId, int userId, DateTime date, int km, double fuel, double price)
        {
            double total = Math.Round(fuel * price, 2);
            trips.Add(tripId, new Tuple<int, DateTime, int, double, double, double>(userId, date, km, fuel, price, total));
        }

        static void UsersMenu(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
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

        static void DeleteUser(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
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
                    if (u.Value.Item1.Equals(name, StringComparison.OrdinalIgnoreCase) && u.Value.Item2.Equals(surname, StringComparison.OrdinalIgnoreCase))
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
        static void TripsMenu(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== PUTOVANJA ===");
                Console.WriteLine("1 - Unos novog putovanja");
                Console.WriteLine("2 - Brisanje putovanja");
                Console.WriteLine("3 - Uređivanje postojećeg putovanja");
                Console.WriteLine("4 - Pregled svih putovanja");
                Console.WriteLine("5 - Izvještaji i analize");
                Console.WriteLine("0 - Povratak");
                Console.Write("Odabir: ");
                var c = Console.ReadLine();

                if (c == "1") AddTrip(users, trips);
                else if (c == "2") DeleteTrip(users, trips);
                else if (c == "3") EditTrip(users, trips); 
                else if (c == "4") ViewTrips(trips, users);
                else if (c == "5") ;// ReportsMenu(users, trips)
                else if (c == "0") break;
                else
                {
                    Console.WriteLine("Pogrešan unos. Pritisni tipku...");
                    Console.ReadKey();
                }
            }
        }

        static void AddTrip(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
        {
            Console.Clear();
            Console.WriteLine("--- Unos novog putovanja ---");

            Console.Write("Unesi ID korisnika (0 za povratak): ");
            var su = Console.ReadLine();
            if (su == "0") return;
            if (!int.TryParse(su, out int userId) || !users.ContainsKey(userId))
            {
                Console.WriteLine("Ne postoji takav korisnik. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            Console.Write("Datum (YYYY-MM-DD) (0 za povratak): ");
            var sd = Console.ReadLine();
            if (sd == "0") return;
            if (!DateTime.TryParse(sd, out DateTime date))
            {
                Console.WriteLine("Neispravan datum. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            Console.Write("Kilometraža (km) (0 za povratak): ");
            var sk = Console.ReadLine();
            if (sk == "0") return;
            if (!int.TryParse(sk, out int km) || km <= 0)
            {
                Console.WriteLine("Neispravan broj. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            Console.Write("Potrošeno gorivo (L) (0 za povratak): ");
            var sf = Console.ReadLine();
            if (sf == "0") return;
            if (!double.TryParse(sf, out double fuel) || fuel <= 0)
            {
                Console.WriteLine("Neispravan broj. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            Console.Write("Cijena po litri (0 za povratak): ");
            var sp = Console.ReadLine();
            if (sp == "0") return;
            if (!double.TryParse(sp, out double price) || price <= 0)
            {
                Console.WriteLine("Neispravan broj. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            double total = Math.Round(fuel * price, 2);

            int newId = trips.Any() ? trips.Keys.Max() + 1 : 1;
            trips.Add(newId, new Tuple<int, DateTime, int, double, double, double>(userId, date, km, fuel, price, total));
            users[userId].Item4.Add(newId);

            Console.WriteLine($"Putovanje dodano (ID: {newId}). Ukupno: {total}e");
            Console.WriteLine("Pritisni tipku...");
            Console.ReadKey();
        }

        static void DeleteTrip(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
        {
            Console.Clear();
            Console.WriteLine("--- Brisanje putovanja ---");
            Console.WriteLine("a) po id-u");
            Console.WriteLine("b) svih putovanja skupljih od unesenog iznosa");
            Console.WriteLine("c) svih putovanja jeftinijih od unesenog iznosa");
            Console.WriteLine("0) Povratak");
            Console.Write("Odabir: ");
            var opt = Console.ReadLine();
            if (opt == "0") return;

            if (opt == "a")
            {
                Console.Write("Unesi ID putovanja: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || !trips.ContainsKey(id))
                {
                    Console.WriteLine("Ne postoji. Pritisni tipku...");
                    Console.ReadKey();
                    return;
                }

                Console.Write($"Sigurno obrisati putovanje {id}? (da/ne): ");
                var conf = Console.ReadLine();

                if (conf?.ToLower() == "da")
                {
                    int uid = trips[id].Item1;
                    trips.Remove(id);
                    if (users.ContainsKey(uid)) users[uid].Item4.Remove(id);
                    Console.WriteLine("Obrisano.");
                }
                else Console.WriteLine("Otkazano.");

                Console.ReadKey();
            }
            else if (opt == "b" || opt == "c")
            {
                Console.Write("Unesi iznos (e) (0 za povratak): ");
                var s = Console.ReadLine();
                if (s == "0") return;
                if (!double.TryParse(s, out double amount) || amount < 0)
                {
                    Console.WriteLine("Neispravan iznos. Pritisni tipku...");
                    Console.ReadKey();
                    return;
                }

                var toDelete = new List<int>();
                foreach (var t in trips)
                {
                    if (opt == "b" && t.Value.Item6 > amount) toDelete.Add(t.Key);
                    if (opt == "c" && t.Value.Item6 < amount) toDelete.Add(t.Key);
                }

                Console.WriteLine("Broj za brisanje: " + toDelete.Count);
                Console.Write("Sigurno obrisati sve? (da/ne): ");
                var conf = Console.ReadLine();

                if (conf?.ToLower() == "da")
                {
                    foreach (var id in toDelete)
                    {
                        int uid = trips[id].Item1;
                        trips.Remove(id);
                        users[uid].Item4.Remove(id);
                    }
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
        static void ViewTrips(Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips, Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
        {
            Console.Clear();
            Console.WriteLine("--- Pregled putovanja ---");
            Console.WriteLine("a) Sva putovanja redom kako su spremljena");
            Console.WriteLine("b) sortirano po trošku uzlazno");
            Console.WriteLine("c) sortirano po trošku silazno");
            Console.WriteLine("d) sortirano po kilometraži uzlazno");
            Console.WriteLine("e) sortirano po kilometraži silazno");
            Console.WriteLine("f) sortirano po datumu uzlazno");
            Console.WriteLine("g) sortirano po datumu silazno");
            Console.WriteLine("0) Povratak");
            Console.Write("Odabir: ");
            var opt = Console.ReadLine();
            if (opt == "0") return;

            var list = trips.ToList();

            if (opt == "b") list.Sort((x, y) => x.Value.Item6.CompareTo(y.Value.Item6));
            else if (opt == "c") list.Sort((x, y) => y.Value.Item6.CompareTo(x.Value.Item6));
            else if (opt == "d") list.Sort((x, y) => x.Value.Item3.CompareTo(y.Value.Item3));
            else if (opt == "e") list.Sort((x, y) => y.Value.Item3.CompareTo(x.Value.Item3));
            else if (opt == "f") list.Sort((x, y) => x.Value.Item2.CompareTo(y.Value.Item2));
            else if (opt == "g") list.Sort((x, y) => y.Value.Item2.CompareTo(x.Value.Item2));

            Console.WriteLine("ID | Korisnik | Datum | Km | Gorivo(L) | Cijena/L | Ukupno");
            foreach (var t in list)
            {
                string name = users.ContainsKey(t.Value.Item1)
                    ? users[t.Value.Item1].Item1 + " " + users[t.Value.Item1].Item2
                    : "Nepoznato";

                Console.WriteLine("{0} | {1} | {2:d} | {3} | {4}L | {5}e | {6}e",
                    t.Key, name, t.Value.Item2, t.Value.Item3, t.Value.Item4, t.Value.Item5, t.Value.Item6);
            }

            Console.WriteLine("Pritisni tipku...");
            Console.ReadKey();
        }
        static void EditTrip(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<int, DateTime, int, double, double, double>> trips)
        {
            Console.Clear();
            Console.WriteLine("--- Uređivanje putovanja ---");
            Console.WriteLine("0) Povratak");
            Console.Write("Unesi ID putovanja: ");
            var s = Console.ReadLine();
            if (s == "0") return;

            if (!int.TryParse(s, out int id) || !trips.ContainsKey(id))
            {
                Console.WriteLine("Ne postoji. Pritisni tipku...");
                Console.ReadKey();
                return;
            }

            var old = trips[id];

            Console.Write("Novi datum (YYYY-MM-DD) (enter za zadržati): ");
            var din = Console.ReadLine();
            DateTime date = old.Item2;
            if (!string.IsNullOrWhiteSpace(din))
            {
                if (!DateTime.TryParse(din, out date))
                {
                    Console.WriteLine("Neispravan datum.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Write("Nova kilometraža (enter za zadržati): ");
            var kin = Console.ReadLine();
            int km = old.Item3;
            if (!string.IsNullOrWhiteSpace(kin))
            {
                if (!int.TryParse(kin, out km) || km <= 0)
                {
                    Console.WriteLine("Neispravan broj.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Write("Novo gorivo (L) (enter za zadržati): ");
            var fin = Console.ReadLine();
            double fuel = old.Item4;
            if (!string.IsNullOrWhiteSpace(fin))
            {
                if (!double.TryParse(fin, out fuel) || fuel <= 0)
                {
                    Console.WriteLine("Neispravan broj.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Write("Nova cijena po litri (enter za zadržati): ");
            var pin = Console.ReadLine();
            double price = old.Item5;
            if (!string.IsNullOrWhiteSpace(pin))
            {
                if (!double.TryParse(pin, out price) || price <= 0)
                {
                    Console.WriteLine("Neispravan broj.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Write("Promijeni korisnika (unesi novi ID ili enter za zadržati): ");
            var uin = Console.ReadLine();
            int newUserId = old.Item1;
            bool userChanged = false;

            if (!string.IsNullOrWhiteSpace(uin))
            {
                if (!int.TryParse(uin, out newUserId) || !users.ContainsKey(newUserId))
                {
                    Console.WriteLine("Neispravan korisnik.");
                    Console.ReadKey();
                    return;
                }
                if (newUserId != old.Item1) userChanged = true;
            }

            double total = Math.Round(fuel * price, 2);

            trips[id] = new Tuple<int, DateTime, int, double, double, double>(newUserId, date, km, fuel, price, total);

            if (userChanged)
            {
                users[old.Item1].Item4.Remove(id);
                users[newUserId].Item4.Add(id);
            }

            Console.WriteLine("Putovanje ažurirano. Novi ukupni trošak: " + total + "e");
            Console.ReadKey();
        }
    }   
}