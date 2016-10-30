using System;
using System.Threading.Tasks;

namespace zad78
{
    class Zadatak7
    {
        public static int FactorialDigitSum(int n)
        {
            int m = n;
            int i = 0;
            for (int j = 1; j < n; j++)
            {
                m *= j;
            }
            while (m != 0)
            {
                i += m%10;
                m /= 10;
            }
            return i;
        }

        public static async Task<int> FactorialDigitSumAsync(int n)
        {
            Task<int> t1 = Task.Run(() => n = FactorialDigitSum(n));
            await t1;
            return n;
        }

        public async void Main()
        {
            await FactorialDigitSumAsync(562);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Main method is the only method that
            // can ’t be marked with async .
            // What we are doing here is just a way for us to simulate
            // async - friendly environment you usually have with
            // other .NET application types ( like web apps , win apps etc .)
            // Ignore main method , you can just focus on
            // LetsSayUserClickedAButtonOnGuiMethod() as a
            // first method in call hierarchy .
            Task.Run(() => LetsSayUserClickedAButtonOnGuiMethod());

            Console.Read();
        }

        private static async void LetsSayUserClickedAButtonOnGuiMethod()
        {
            var result = await GetTheMagicNumber();
            Console.WriteLine(result);
        }

        private static async Task<int> GetTheMagicNumber()
        {
            Task<int> t1 = KnowIGuyWhoKnowsAGuy();
            await t1;
            return t1.Result;
        }

        private static async Task<int> KnowIGuyWhoKnowsAGuy()
        {
            Task<int> t1 = KnowWhoKnowsThis(10);
            Task<int> t2 = KnowWhoKnowsThis(5);
            await t1;
            await t2;
            return t1.Result + t2.Result;
        }

        private static async Task<int> KnowWhoKnowsThis(int n)
        {
            Task<int> t1 = FactorialDigitSumAsync(n);
            await t1;
            return t1.Result;
        }

        public static async Task<int> FactorialDigitSumAsync(int n)
        {
            Task<int> t1 = Task.Run(() => n = FactorialDigitSum(n));
            await t1;
            return n;
        }

        public static int FactorialDigitSum(int n)
        {
            int m = n;
            int i = 0;
            for (int j = 1; j < n; j++)
            {
                m *= j;
            }
            while (m != 0)
            {
                i += m%10;
                m /= 10;
            }
            return i;
        }
    }
}