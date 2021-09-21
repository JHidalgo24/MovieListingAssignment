using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System.Linq;
using NLog;

namespace ConsoleApp4
{
    class Program
    {
        private static void Main(string[] args)
        {
            int number = 1;
            List<Movie> temp;
            string filmPicked;
            int lastId = 0;
            while (number != 4)
            {
                PrintMenu();
                number = ValueGetter();
                switch (number)
                {
                    case 1:
                        temp = ReturnFilmList();
                        lastId = temp[temp.Count - 1].Id;
                        AddMovie(lastId);
                        break;
                    case 2:
                        System.Console.WriteLine(
                            $"There are {ReturnFilmList().Count} movies in file from where to where do you want to see");
                        Console.WriteLine($"What is the first number from 1 - {ReturnFilmList().Count}");
                        int firstNumber = ValueGetter();
                        Console.WriteLine($"What is the second number from {firstNumber} - {ReturnFilmList().Count}");
                        int secondNumber = ValueGetter();
                        while (secondNumber < firstNumber)
                        {
                            System.Console.WriteLine("Second value can't be smaller!");
                            secondNumber = ValueGetter();
                        }

                        ;
                        ListFilms(firstNumber, secondNumber);
                        break;
                    case 3:
                        Console.WriteLine("What movie do you want to look up?(Press Enter for all)");
                        filmPicked = Console.ReadLine();
                        SearchMovie(filmPicked);
                        break;
                    case 4:
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("That is not an option sorry!");
                        break;
                }
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("What do you want to do?\n1.)Add movie\n2.)List Movies\n3.)Search Movie\n4.)Exit)");
        }

        private static void ListFilms(int firstNum, int secondNum)
        {
            List<Movie> temp = ReturnFilmList();

            for (int i = firstNum - 1; i <= secondNum - 1; i++)
            {
                Console.WriteLine(temp[i]);
            }
        }

        private static void AddMovie(int id)
        {
            try
            {
                string titlePicked = "", genresPicked = "";
                int genresTotal;
                Console.WriteLine("What is the title of the film");
                titlePicked = Console.ReadLine();
                while (DuplicateChecker(titlePicked))
                {
                    Console.WriteLine("Sorry the film is already in the list enter a new one");
                    titlePicked = Console.ReadLine();
                }

                Console.WriteLine("How many genres do you want to add?");
                genresTotal = ValueGetter();
                for (int i = 0; i < genresTotal; i++)
                {
                    Console.WriteLine($"What is the {i + 1} genre?");
                    genresPicked += Console.ReadLine();
                }

                var records = new List<Movie> {new Movie {Id = id + 1, title = titlePicked, genres = genresPicked},};
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) {HasHeaderRecord = false,};
                using (var stream = File.Open("C:\\Users\\justi\\RiderProjects\\ConsoleApp4\\ConsoleApp4\\movies.csv",
                    FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(records);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to write to file!");
            }
        }

        private static void SearchMovie(string filmPicked)
        {
            List<Movie> temp = ReturnFilmList();
            foreach (Movie movies in temp)
            {
                String currentMovie = movies.title.ToLower();
                if (currentMovie.Contains(filmPicked.ToLower()))
                {
                    Console.WriteLine(movies);
                }
            }
        }

        private static Boolean DuplicateChecker(string filmPicked)
        {
            bool contained = false;
            List<Movie> temp = ReturnFilmList();
            foreach (Movie movies in temp)
            {
                String currentMovie = movies.title.ToLower();
                if (currentMovie.Contains(filmPicked.ToLower()))
                {
                    contained = true;
                }
            }

            return contained;
        }

        private static List<Movie> ReturnFilmList()
        {
            List<Movie> movies;
            try
            {
                using (var streamReader =
                    new StreamReader("C:\\Users\\justi\\RiderProjects\\ConsoleApp4\\ConsoleApp4\\movies.csv"))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Movie>().ToList();
                    movies = records;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read file sorry!");
                throw;
            }

            return movies;
        }

        private static int ValueGetter()
        {
            string option = Console.ReadLine();
            int number;
            bool success = Int32.TryParse(option, out number);

            while (success == false)
            {
                Console.WriteLine("That isn't a number sorry!");
                option = Console.ReadLine();
                success = Int32.TryParse(option, out number);
            }

            return number;
        }
    }
}