using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        public class OneProducerTwoConsumer
        {
            private readonly BlockingCollection<int> randomNumbersForDivisibleByFive = new BlockingCollection<int>(10);
            private readonly BlockingCollection<int> randomNumbersForDivisibleByThree = new BlockingCollection<int>(10);

            private void Producer()
            {
                int numberOfNumbers = 50;
                Random random = new Random();

                for (int i = 0; i < numberOfNumbers; i++)
                {
                    int randomNumber = random.Next(1000);

                    randomNumbersForDivisibleByFive.Add(randomNumber);
                    randomNumbersForDivisibleByThree.Add(randomNumber);
                }
            }
            private void ConsumerDivisibleByFive()
            {
                foreach (int randomNumber in randomNumbersForDivisibleByFive.GetConsumingEnumerable())
                {
                    if(randomNumber % 5 == 0)
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
            public void RunTasks()
            {
                var producingTask = Task.Run(() => Producer());

                var divisibleByFiveTask = Task.Run(() => ConsumerDivisibleByFive());

                var divisibleByThreeTask = Task.Run(() => ConsumerDivisibleByThree());

                Task.WaitAll(producingTask, divisibleByFiveTask, divisibleByThreeTask);
            }
        }
        public static void Main(string[] args)
        {
            OneProducerTwoConsumer oneProducerTwoConsumer = new OneProducerTwoConsumer();
            Console.WriteLine();
            oneProducerTwoConsumer.RunTasks();
        }
    }
}
