using InferenceEngineRBS;
using static InferenceEngineRBS.IEngine;

namespace InferenceEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Parametrización
            char meta = 'h';
            List<char> bh = new List<char> { 'b', 'c' };
            Dictionary<int, Fact> bc = new Dictionary<int, Fact>()
                {
                    { 1, new Fact(){ action = new char[]{'b','d','e'},  consequence = 'f' } },
                    { 2, new Fact(){ action = new char[]{'d','g'},      consequence = 'a' } },
                    { 3, new Fact(){ action = new char[]{'c','f'},      consequence = 'a' } },
                    { 4, new Fact(){ action = new char[]{'b'},          consequence = 'x' } },
                    { 5, new Fact(){ action = new char[]{'d'},          consequence = 'e' } },
                    { 6, new Fact(){ action = new char[]{'a','x'},      consequence = 'h' } },
                    { 7, new Fact(){ action = new char[]{'c'},          consequence = 'd' } },
                    { 8, new Fact(){ action = new char[]{'x','c'},      consequence = 'a' } },
                    { 9, new Fact(){ action = new char[]{'x','b'},      consequence = 'd' } }
                };

            IEngine ie =
                new IEngine(Criterion.NumberClausesAntecedents, meta, bh, bc);

            bool result = ie.Start();
            if (result)
            {
                Console.WriteLine("éxito");
            }
        }
    }
}