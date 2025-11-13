namespace Konzolna_aplikacija
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var trips = new Dictionary<int, Tuple<int, DateTime, int, double, double, double>>()
            {
                {1, new Tuple<int,DateTime,int,double,double,double>(1, new DateTime(2025,1,10), 200, 10, 1.5, 15)}
            };
            var trip = trips[1];

            Console.WriteLine("Broj putovanja: {0}, ID korisnika: {1}, Datum: {2:d}, Prijeđeni kilometri: {3}, Potrošene litre goriva: {4}L, Cijena goriva: {5}e, Ukupna cijena: {6}e", 1, trip.Item1, trip.Item2, trip.Item3, trip.Item4, trip.Item5, trip.Item6);

            var users = new Dictionary<int, Tuple<string, string, DateTime>>()
            {
                {1, new Tuple<string,string,DateTime>("Ante","Antić",new DateTime(2000,1,11)) }
            };
            var user = users[1];
            Console.WriteLine("{0}-{1}-{2}-{3:d}",1,user.Item1,user.Item2,user.Item3);
        }
    }
}