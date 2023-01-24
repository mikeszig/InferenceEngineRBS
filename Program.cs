namespace InferenceEngine
{
    class IEngine
    {
        static Criterion criterion { get; set; }

        static char meta;
        static List<char> bh;
        static Dictionary<int, Fact> bc;

        static int step = 1;

        enum Criterion { LowestRate, NumberClausesAntecedents }

        public static void Main(string[] args)
        {
            // Parametrización
            criterion = Criterion.NumberClausesAntecedents;

            meta = 'h';
            bh = new List<char> { 'b', 'c', 'c', 'd', 'e' }; 
            bc = new Dictionary<int, Fact>()
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

            Start();
        }

        private static bool Start()
        {
            int[] cc = IterateBHBC(bh, bc);

            while (!bh.Contains(meta))
            {
                cc = IterateBHBC(bh, bc);                   // Comparamos los antecedentes con los hechos y actualizamos cc
                if (cc.Count() != 0)
                {
                    int r = 0;
                    if (criterion == Criterion.LowestRate)  // Resolver el conjunto conflicto segun criterio
                    {
                        r = SolveConflict(cc); 
                    } else
                    {
                        r = SolveConflict(cc, bc);
                    }

                    char newFact = bc[r].consequence;       // Obtener la consecuencia según la regla que se haya activado en el paso anterior
                    bc.Remove(r);                           // Quitamos la regla activada de la base de conocimiento para que no vuelva a salir
                    bh.Add(newFact);                        // Añadir la consecuencia a la base de hechos
                }

                Console.WriteLine($"Ciclo: {step}");
                step++;
            }

            if (bh.Contains(meta))
            {
                Console.WriteLine("éxito");
                return true;
            }
                
            return false;
        }

        private static int SolveConflict(int[] cc) // La regla de menor indice
        {
            return cc.Min();
        }

        private static int SolveConflict(int[] cc, Dictionary<int, Fact> bc) // La regla con mayor numero de clausulas antecedentes
        {
            int index = 0;
            int size = 0;

            foreach (int i in cc)
            {
                if (bc[i].action.Length > size) // La regla con mas antecedentes es almacenada
                {
                    index = i;
                    size = bc[i].action.Length;
                }

                if (bc[i].action.Length == size) // Si tienen el mismo número de antecedentes...
                {
                    index = i < index ? i : index; // Comparamos cual tiene el menor índice
                    size = bc[index].action.Length;
                }
            }
            return index;
        }

        private static int[] IterateBHBC(List<char> bh, Dictionary<int, Fact> bc)
        {
            List<int> list = new List<int>();

            foreach (KeyValuePair<int, Fact> entry in bc)
            {
                int coin = 0;
                char[] actionCopy = entry.Value.action.ToArray();
                int actionSize = actionCopy.Length;

                foreach (char c in bh)
                {
                    int numIndex = Array.IndexOf(actionCopy, c);

                    if (actionCopy.Contains(c))
                    {
                        coin++;
                        actionCopy = actionCopy.Where((val, idx) => idx != numIndex).ToArray();
                    }

                    if (coin == actionSize)
                        break;
                }

                if (coin == actionSize && !list.Contains(entry.Key))
                    list.Add(entry.Key);

                coin = 0;
            }
            return list.ToArray();
        }
    }

    public class Fact 
    {
        public char[] action { get; set; } // Antecedente
        public char consequence { get; set; } // Consecuente
    }
}