using System;
using System.Data.SqlClient;

namespace DB_1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new BuildingContext();
            db.CreateIfDontExist();
            // Функции для инициализации таблиц
            //db.InitializeMaterial();
            //db.InitializeBuildings();
            var connection = new SqlConnection("Server=DESKTOP-TSHMHUG;Database=FirstTaskADO;Trusted_Connection=True;");
            connection.Open();
            // 1 - Панель 
            // 2 - Кирпич
            // 3 - Монолит
            Console.WriteLine("Вывести список предложений, отсортированных по типу дома, далее по цене");
            var firstExercise = connection.CreateCommand();
            firstExercise.CommandText = "SELECT * FROM Buildings ORDER BY MaterialIdId, Price DESC";
            var first = firstExercise.ExecuteReader();
            while (first.Read())
            {
                Console.WriteLine($"Price =  {first.GetValue(1)} Area = {first.GetValue(2)} Street = {first.GetValue(3)} Material = {first.GetValue(4)}");
            }
            first.Close();
            Console.WriteLine("\n");
            Console.WriteLine("Вывести первые три самых дешёвых предложения по цене за квадратный метр");
            var secondExercise = connection.CreateCommand();
            secondExercise.CommandText = "SELECT TOP(3) Street, (Price/Area) , MaterialIdId FROM Buildings ORDER BY (Price/Area)";
            var second = secondExercise.ExecuteReader();
            while (second.Read())
            {
                Console.WriteLine($"Price =  {second.GetValue(1)} Material = {second.GetValue(2)} Street = {second.GetValue(0)}");
            }
            second.Close();
            Console.WriteLine("\n");
            Console.WriteLine("Вывести информацию для каждого типа дома { Тип дома, средний метражпредложений, средняя цена предложения }");
            var thirdExercise = connection.CreateCommand();
            thirdExercise.CommandText = "Select MaterialIdId, AVG(Area), AVG(Price) From Buildings GROUP BY MaterialIdId";
            var third = thirdExercise.ExecuteReader();
            while (third.Read())
            {
                Console.WriteLine($"Price =  {third.GetValue(2)} Area = {third.GetValue(1)} Material = {third.GetValue(0)}");
            }
            third.Close();

            Console.WriteLine("\n");
            Console.WriteLine("Вывести { тип дома, средняя цена за метр } отсортированные по средней цене от дорогих к дешёвым");
            var fourthExercise = connection.CreateCommand();
            fourthExercise.CommandText = "Select MaterialIdId, AVG(Price/Area) From Buildings GROUP BY MaterialIdId ORDER BY AVG(Price/Area) DESC";
            var fourth = fourthExercise.ExecuteReader();
            while (fourth.Read())
            {
                Console.WriteLine($"Price =  {fourth.GetValue(1)} Material = {fourth.GetValue(0)}");
            }
            fourth.Close();
            Console.WriteLine("\n");
            Console.WriteLine("Найти самое дорогое по отношению цены за метр предложение, уценить на 10 %.");
            var pre_update = connection.CreateCommand();
            Console.WriteLine("До UPDATE");
            pre_update.CommandText = "SELECT Id, (Price/Area) FROM Buildings ORDER BY (Price/Area) DESC";
            var pre_update_reader = pre_update.ExecuteReader();
            while (pre_update_reader.Read())
            {
                Console.WriteLine($"Price =  {pre_update_reader.GetValue(1)} Id = {pre_update_reader.GetValue(0)}");
            }
            pre_update_reader.Close();
            Console.WriteLine("После UPDATE");
            var fifthExercise = connection.CreateCommand();
            var UpdateCommand = connection.CreateCommand();
            UpdateCommand.CommandText = $"UPDATE Buildings SET Price = Price * 0.9 WHERE Id = (SELECT TOP(1) Id FROM Buildings ORDER BY (Price/Area) DESC)";
            UpdateCommand.ExecuteNonQuery();
            fifthExercise.CommandText = "SELECT Id, (Price/Area) FROM Buildings ORDER BY (Price/Area) DESC";
            var check_reader = fifthExercise.ExecuteReader();
            while (check_reader.Read())
            {
                Console.WriteLine($"Price =  {check_reader.GetValue(1)} Id = {check_reader.GetValue(0)}");
            }
            check_reader.Close();

            Console.WriteLine("\n");
            Console.WriteLine("Добавить новое предложение в самую дешёвую по цене за метр категорию");
            var sixthExercise = connection.CreateCommand();
            sixthExercise.CommandText = "INSERT INTO Buildings (Price, Area, Street, MaterialIdId) " +
                "                       VALUES ( (SELECT Price FROM Buildings WHERE Id = (SELECT TOP(1) Id FROM Buildings ORDER BY (Price/Area))) * 0.5" +
                "                               ,(SELECT Area FROM Buildings WHERE Id = (SELECT TOP(1) Id FROM Buildings ORDER BY (Price/Area)))" +
                "                               ,(SELECT Street FROM Buildings WHERE Id = (SELECT TOP(1) Id FROM Buildings ORDER BY (Price/Area)))" +
                "                               ,(SELECT MaterialIdId FROM Buildings WHERE Id = (SELECT TOP(1) Id FROM Buildings ORDER BY (Price/Area)))" +
                "                              ) ";
            sixthExercise.ExecuteNonQuery();
            var check_2 = connection.CreateCommand();
            check_2.CommandText = "SELECT Id, (Price/Area), MaterialIdId FROM BUILDINGS ORDER BY (Price/Area)";
            var check_2_reader = check_2.ExecuteReader();
            while (check_2_reader.Read()) 
            {
                Console.WriteLine($"Id = {check_2_reader.GetValue(0)} Price/Area = {check_2_reader.GetValue(1)} MaterialIdId = {check_2_reader.GetValue(2)} ");
            }
            check_2_reader.Close();
            Console.WriteLine("\n");
            Console.WriteLine("Удалить из базы предложения дешевле среднего по базе.");
            Console.WriteLine("До выполнения DELETE");
            var check_2_reader_2 = check_2.ExecuteReader();
            while (check_2_reader_2.Read())
            {
                Console.WriteLine($"Id = {check_2_reader_2.GetValue(0)} Price/Area = {check_2_reader_2.GetValue(1)} MaterialIdId = {check_2_reader_2.GetValue(2)} ");
            }
            check_2_reader_2.Close();
            Console.WriteLine("\n");
            Console.WriteLine("После выполнения DELETE");
            var seventhExercise = connection.CreateCommand();
            seventhExercise.CommandText = "DELETE FROM Buildings Where (Price/Area) < (SELECT AVG(Price/Area) FROM Buildings)";
            seventhExercise.ExecuteNonQuery();
            var check_2_reader_3 = check_2.ExecuteReader();
            while (check_2_reader_3.Read())
            {
                Console.WriteLine($"Id = {check_2_reader_3.GetValue(0)} Price/Area = {check_2_reader_3.GetValue(1)} MaterialIdId = {check_2_reader_3.GetValue(2)} ");
            }
            check_2_reader_3.Close();
            connection.Close();

        }
    }
}
