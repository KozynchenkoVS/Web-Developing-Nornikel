using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DB_1_2
{
   internal class BuildingContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<Building> Buildings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=DESKTOP-TSHMHUG;Database=FirstTaskADO;Trusted_Connection=True;");
            base.OnConfiguring(options);
        }
        public void CreateIfDontExist()
        {
            this.Database.EnsureCreated();
        }
        public void InitializeMaterial()
        {
            Material first = new Material
            {
                Name = "Панель"
            };
            Material second = new Material
            {
                Name = "Кирпич"
            };
            Material third = new Material
            {
                Name = "Монолит"
            };
            this.Materials.AddRange(first, second, third);
            this.SaveChanges();
        }
        public void InitializeBuildings()
        {
            var materials = this.Materials.ToList();
            Building first = new Building
            {
                Street = "A",
                Area = 50.2,
                MaterialId = materials[0],
                Price = 13000.2
            };
            Building second = new Building
            {
                Street = "A",
                Area = 50.2,
                MaterialId = materials[0],
                Price = 6000.2

            };
            Building third = new Building
            {
                Street = "H",
                Area = 100.2,
                MaterialId = materials[0],
                Price = 8000.2
            };
            Building fourth = new Building
            {
                Street = "B",
                Area = 32.2,
                MaterialId = materials[1],
                Price = 8000.2
            };
            Building fifth = new Building
            {
                Street = "C",
                Area = 103.2,
                MaterialId = materials[1],
                Price = 9000.2
            };
            Building sixth = new Building
            {
                Street = "D",
                Area = 122.2,
                MaterialId = materials[1],
                Price = 10000.2
            };
            Building seventh = new Building
            {
                Street = "E",
                Area = 22.2,
                MaterialId = materials[2],
                Price = 11000.2
            };
            Building eigth = new Building
            {
                Street = "F",
                Area = 15.2,
                MaterialId = materials[2],
                Price = 12000.2
            };
            Building nineth = new Building
            {
                Street = "G",
                Area = 60.2,
                MaterialId = materials[2],
                Price = 13000.2
            };
            this.Buildings.AddRange(first, second, third, fourth, fifth, sixth, seventh, eigth, nineth);
            this.SaveChanges();
        }
    }
    public class Material
    {
        public int Id { get; set; }
        [MaxLength(10)]
        public string Name { get; set; }
    }
    public class Building
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double Area { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }

        public Material MaterialId { get; set; }
    }

}
