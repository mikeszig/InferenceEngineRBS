using InferenceEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngineRBS
{
    public class IEngine
    {
        private Criterion criterion { get; set; }

        private char meta { get; set; }
        private List<char> bh { get; set; }
        private Dictionary<int, Fact> bc { get; set; }

        private int step = 1;   // Conteo de ciclos
        private int r = 0;      // Resolución

        public enum Criterion { LowestRate, NumberClausesAntecedents }

        public IEngine(Criterion criterion, char meta, List<char> bh, Dictionary<int, Fact> bc)
        {
            this.criterion = criterion;
            this.meta = meta;
            this.bh = bh;
            this.bc = bc;
        }

        public bool Start()
        {
            int[] cc = IterateBHBC(bh, bc);

            while (!bh.Contains(meta))
            {
                cc = IterateBHBC(bh, bc);                   // Comparamos los antecedentes con los hechos y actualizamos cc
                if (cc.Count() != 0)
                {
                    if (criterion == Criterion.LowestRate)  // Resolver el conjunto conflicto segun criterio
                    {
                        r = SolveConflict(cc);
                    }
                    else
                    {
                        r = SolveConflict(cc, bc);
                    }

                    char newFact = bc[r].consequence;       // Obtener la consecuencia según la regla que se haya activado en el paso anterior
                    bc.Remove(r);                           // Quitamos la regla activada de la base de conocimiento para que no vuelva a salir
                    bh.Add(newFact);                        // Añadir la consecuencia a la base de hechos
                }

                Console.WriteLine($"Ciclo: {step} - " +
                    $"BH: {string.Join(" ", bh).ToUpper()} - " +
                    $"CC: {string.Join(" ", cc).ToUpper()} - " +
                    $"R: {r}");
                step++;
            }

            if (bh.Contains(meta))
            {
                return true;
            }

            return false;
        }

        private int SolveConflict(int[] cc) // La regla de menor indice
        {
            return cc.Min();
        }

        private int SolveConflict(int[] cc, Dictionary<int, Fact> bc) // La regla con mayor numero de clausulas antecedentes
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

        private int[] IterateBHBC(List<char> bh, Dictionary<int, Fact> bc)
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
}
