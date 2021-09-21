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
                        int firstNumber,secondNumber;
                        temp = ReturnFilmList();
                        System.Console.WriteLine($"There are {temp.Count()} movies in file from where to where do you want to see");
                        Console.WriteLine($"What is the first number from 1 - {temp.Count}");
                        firstNumber = ValueGetter(); 
                        Console.WriteLine($"What is the second number from {firstNumber} - {temp.Count}");
                        secondNumber = ValueGetter();
                        while(secondNumber < firstNumber){
                            System.Console.WriteLine("Second value can't be smaller!");
                            secondNumber = ValueGetter();};
                        ListFilms(temp,firstNumber,secondNumber);
                        break;
                    case 3:
                        Console.WriteLine("What movie do you want to look up?(Press Enter for all)");
                        filmPicked = Console.ReadLine();
                        SearchMovie(ReturnFilmList(),filmPicked);
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

        private static void ListFilms(List<Movie> films,int firstNum, int secondNum)
        {
            List<Movie> temp = ReturnFilmList();

            for (int i = firstNum-1; i<=secondNum-1; i++)
            {
                Console.WriteLine(temp[i]);
            }
        }

        private static void AddMovie(int id){
            
        }//figure out how to add movies

        private static void SearchMovie(List<Movie> film, string filmPicked)
        {
            List<Movie> temp = film;
            foreach (Movie movies in temp)
            {
                String currentMovie = movies.title;
                if (currentMovie.Contains(filmPicked))
                {
                    Console.WriteLine(movies);
                }
            }
        }

        private static List<Movie> ReturnFilmList()
        {
            List<Movie> movies = new List<Movie>();
            try
            {
                using (var streamReader = new StreamReader("C:\\Users\\justi\\RiderProjects\\ConsoleApp4\\ConsoleApp4\\movies.csv"))
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