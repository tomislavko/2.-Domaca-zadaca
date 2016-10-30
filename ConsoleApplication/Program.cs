using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class Student
    {
        public string Name { get; set; }
        public string Jmbag { get; set; }
        public Gender Gender { get; set; }

        public Student(string name, string jmbag)
        {
            Name = name;
            Jmbag = jmbag;
        }

        public override bool Equals(object obj)
        {
            Student stud = (Student) obj;
            if (this.Name.Equals(stud.Name) && this.Jmbag.Equals(stud.Jmbag) && this.Gender.Equals(stud.Gender))
                return true;
            return false;
        }


        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Jmbag != null ? Jmbag.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Gender;
                return hashCode;
            }
        }

        public static bool operator !=(Student prvi, Student drugi)
        {
            if (prvi.Name.Equals(drugi.Name) && prvi.Jmbag.Equals(drugi.Jmbag) && prvi.Gender.Equals(drugi.Gender))
                return false;
            return true;
        }

        public static bool operator ==(Student prvi, Student drugi)
        {
            if (prvi.Name.Equals(drugi.Name) && prvi.Jmbag.Equals(drugi.Jmbag) && prvi.Gender.Equals(drugi.Gender))
                return true;
            return false;
        }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class University
    {
        public string Name { get; set; }
        public Student[] Students { get; set; }
    }

    class Program
    {
        static void Example1()
        {
            var list = new List<Student>()
            {
                new Student("Ivan", jmbag: "001234567")
            };
            var ivan = new Student("Ivan", jmbag: "001234567");
            // false :(
            bool anyIvanExists = list.Any(s => s == ivan);
            // Vise ne :)
        }

        static void Example2()
        {
            var list = new List<Student>()
            {
                new Student("Ivan", jmbag: "001234567"),
                new Student("Ivan", jmbag: "001234567")
            };
            // 2 :(
            var distinctStudents = list.Distinct().Count();
            Console.WriteLine(distinctStudents);
        }

        static void Main(string[] args)
        {
            University[] universities = GetAllCroatianUniversities();
            Example1();
            Example2();
            int[] integers = new[] {1, 2, 2, 2, 3, 3, 4, 5};

            string[] scounts = integers.GroupBy(n => n)
                .ToDictionary(g => g.Key.ToString(), g => g.Count().ToString())
                .Select(x => "Broj " + x.Key + " ponavlja se " + x.Value + " puta")
                .ToArray();
            foreach (var x in scounts)
            {
                Console.WriteLine(x);
            }
            Student[] allCroatianStudents = universities.SelectMany(uni => uni.Students).Distinct().ToArray();
            Student[] croatianStudentsOnMultipleUniversities =
                universities.SelectMany(uni => uni.Students)
                    .GroupBy(stud => stud)
                    .Where(st => st.Count() > 1)
                    .Select(x => x.Key)
                    .ToArray();
            Student[] studentsOnMaleOnlyUniversities =
                universities.SkipWhile(uni => !uni.Students.TakeWhile(stud => stud.Gender.Equals(1)).Equals(null))
                    .SelectMany(univ => univ.Students)
                    .ToArray();

            Console.Read();
        }

        public static University[] GetAllCroatianUniversities()
        {
            University[] uni = new University[] {};
            return uni;
        }
    }
}