using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace DB_1
{

    class Program
    {
      
        static void Main(string[] args)
        {
            BuildingsContext db = new BuildingsContext();
            db.CreateIfDontExist();
            //для Инициализации таблиц, создадутся объекты, которые можно использовать в дальнейшем
            //db.InitializeMaterial();
            //db.InitializeBuildings();
            // 3 - Монолит
            // 2 - Кирпич
            // 1 - Панель
            Console.WriteLine("Вывести список предложений, отсортированных по типу дома, далее по цене");
            var FirstExercise = db.Buildings.OrderByDescending(c => c.MatrialID.Id).ThenByDescending(c => c.Price).Include(c => c.MatrialID).ToList();
            for (int i = 0; i < FirstExercise.Count(); i++)
            {
                Console.WriteLine($"Price =  {FirstExercise[i].Price} Area = {FirstExercise[i].Area} Street = {FirstExercise[i].Street} Material = {FirstExercise[i].MatrialID?.Name}");
                
            }

            Console.WriteLine("\n");
            Console.WriteLine("Вывести первые три самых дешёвых предложения по цене за квадратный метр");
            var SecondExercise = db.Buildings.OrderBy(c => c.Price / c.Area).Include(c => c.MatrialID).ToList();
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Price =  {SecondExercise[i].Price} Area = {SecondExercise[i].Area} Street = {SecondExercise[i].Street} Material = {SecondExercise[i].MatrialID?.Name}");

            }

            Console.WriteLine("\n");
            Console.WriteLine("Вывести информацию для каждого типа дома { Тип дома, средний метражпредложений, средняя цена предложения }");
            var ThirdExercise = db.Buildings.GroupBy(c => c.MatrialID.Name).Select(g => new {Name = g.Key, Price = g.Average(x => x.Price), Area = g.Average(v => v.Area)}).ToList();
            for (int i = 0; i < ThirdExercise.Count(); i++)
            {
                Console.WriteLine($"Material = {ThirdExercise[i].Name} Avg.Price =  {ThirdExercise[i].Price} Avg.Area = {ThirdExercise[i].Area}");

            }

            Console.WriteLine("\n");
            Console.WriteLine("Вывести { тип дома, средняя цена за метр } отсортированные по средней цене от дорогих к дешёвым");
            var GroupBy = db.Buildings.GroupBy(c => c.MatrialID.Name).Select(g => new { Name = g.Key, Price = g.Average(x => x.Price / x.Area)});
            var FourthExercise = GroupBy.OrderByDescending(c => c.Price).ToList();
            for (int i = 0; i < FourthExercise.Count(); i++)
            {
                Console.WriteLine($"Material = {FourthExercise[i].Name} Avg.Price =  {FourthExercise[i].Price}");

            }

            Console.WriteLine("\n");
            Console.WriteLine("Найти самое дорогое по отношению цены за метр предложение, уценить на 10 %.");
                var FifthExercise = db.Buildings.OrderByDescending(c => c.Price / c.Area).Include(c => c.MatrialID).First();
                FifthExercise.Price = FifthExercise.Price * 0.9;
                db.Update(FifthExercise);
                db.SaveChanges();
            
            var Check = db.Buildings.OrderBy(c => c.Price / c.Area).Include(c => c.MatrialID).ToList();
            for (int i = 0; i < Check.Count(); i++)
            {
                Console.WriteLine($"Material = {Check[i].MatrialID?.Name} Price =  {Check[i].Price}");

            }
            Console.WriteLine("\n");
            Console.WriteLine("Добавить новое предложение в самую дешёвую по цене за метр категорию");
                var SixthExercise = db.Buildings.OrderBy(c => c.Price / c.Area).Include(c => c.MatrialID).First();
            Building LessPrice = new Building
            {
                Area = SixthExercise.Area,
                Price = SixthExercise.Price * 0.5,
                Street = SixthExercise.Street,
                MatrialID = SixthExercise.MatrialID
                };
                db.Add(LessPrice);
                db.SaveChanges();
          
            var Check_2 = db.Buildings.OrderBy(c => c.Price / c.Area).Include(c => c.MatrialID).ToList();
            for (int i = 0; i < Check_2.Count(); i++)
            {
                Console.WriteLine($"Material = {Check_2[i].MatrialID?.Name} Price =  {Check_2[i].Price}");

            }


            Console.WriteLine("\n");
            Console.WriteLine("Удалить из базы предложения дешевле среднего по базе.");
            var SeventhExercise = db.Buildings.OrderBy(c => c.Price / c.Area).Include(c => c.MatrialID).Average(c=> c.Price / c.Area);
            var ListLessAvg = db.Buildings.Where(c => (c.Price / c.Area) < SeventhExercise);
            db.RemoveRange(ListLessAvg);
            db.SaveChanges();

            var Check_3 = db.Buildings.OrderBy(c => c.Price / c.Area).Include(c => c.MatrialID).ToList();
            for (int i = 0; i < Check_3.Count(); i++)
            {
                Console.WriteLine($"Material = {Check_3[i].MatrialID?.Name} Price =  {Check_3[i].Price}");

            }
        }
    }
}
