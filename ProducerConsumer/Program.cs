using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        public class OneProducerTwoConsumer
        {
            private readonly BlockingCollection<int> randomNumbersForDivisibleByFive = new BlockingCollection<int>(1);
            private readonly BlockingCollection<int> randomNumbersForDivisibleByThree = new BlockingCollection<int>(1);

            private void Producer()
            {
                int numberOfNumbers = 5;
                Random random = new Random();

                for (int i = 0; i < numberOfNumbers; i++)
                {
                    int randomNumber = random.Next(1000);
                    Thread.Sleep(1000);

                    randomNumbersForDivisibleByFive.Add(randomNumber);
                    randomNumbersForDivisibleByThree.Add(randomNumber);
                }
            }
            private void ConsumerDivisibleByFive()
            {
                foreach (int randomNumber in randomNumbersForDivisibleByFive.GetConsumingEnumerable())
                {
                    if (randomNumber % 5 == 0)
                    {
                        Console.WriteLine($"{randomNumber} is divisible by 5");
                    }
                    else
                    {
                        Console.WriteLine($"{randomNumber} is not divisible by 5");
                    }
                }
            }
            private void ConsumerDivisibleByThree()
            {
                foreach (int randomNumber in randomNumbersForDivisibleByThree.GetConsumingEnumerable())
                {
                    if (randomNumber % 3 == 0)
                    {
                        Console.WriteLine($"{randomNumber} is divisible by 3");
                    }
                    else
                    {
                        Console.WriteLine($"{randomNumber} is not divisible by 3");
                    }
                }
            }
            public void RunTasks1()
            {
                var producingTask = Task.Run(() => Producer());

                var divisibleByFiveTask = Task.Run(() => ConsumerDivisibleByFive());

                var divisibleByThreeTask = Task.Run(() => ConsumerDivisibleByThree());

                Task.WaitAll(producingTask, divisibleByFiveTask, divisibleByThreeTask);
            }
        }

        public class PipeLinePattern
        {
            private readonly BlockingCollection<string> CookPizza = new BlockingCollection<string>(4);
            private readonly BlockingCollection<string> PizzaOnCounter = new BlockingCollection<string>(3);

            private void TakePizzaOrder()
            {
                List<string> ovenFood = new List<string>()
                {
                    "Margherita Pizza", "Beef Pepperoni Pizza", "Chicken Pepperoni Pizza", "Pineappple Pizza",
                    "Mushroom Pizza", "Greek Pizza", "Cheese Pizza", "Ham Pizza", "Four Cheese Pizza", "Lemon Pizza", "Bacon Pizza",
                };

                Random randomOrders = new Random();
                Random pizza = new Random();

                int NumberOfOrders = randomOrders.Next(3, 21);
                for (int p = 0; p < NumberOfOrders; p++)
                {
                    int pizzaNumber = pizza.Next(ovenFood.Count);
                    string pizzaName = ovenFood[pizzaNumber];

                    CookPizza.Add(pizzaName);
                }
            }

            private void PutPizzaInOven()
            {
                foreach (string pizza in CookPizza.GetConsumingEnumerable())
                {
                    Thread.Sleep(300);
                    Console.WriteLine($"A {pizza} Is Cooked!");
                    PizzaOnCounter.Add(pizza);
                }
            }

            private void PutPizzaOnTable()
            {
                foreach (string pizza in PizzaOnCounter.GetConsumingEnumerable())
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Enjoy your {pizza}!");
                }
            }

            public void RunTasks2()
            {
                var takePizzaOrder = Task.Run(() => TakePizzaOrder());
                var putPizzaInOven = Task.Run(() => PutPizzaInOven());
                var putPizzaOnTable = Task.Run(() => PutPizzaOnTable());

                Task.WaitAll(takePizzaOrder, putPizzaInOven, putPizzaOnTable);
            }
        }

        public static void Main(string[] args)
        {
            /*OneProducerTwoConsumer oneProducerTwoConsumer = new OneProducerTwoConsumer();
            Console.WriteLine();
            oneProducerTwoConsumer.RunTasks1();
            */
            PipeLinePattern pipeLinePattern = new PipeLinePattern();
            Console.WriteLine("What would you like to order?");
            pipeLinePattern.RunTasks2();
        }
    }
}
