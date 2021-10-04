using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerificaInfo41021
{
    class VeicoloAMotore : Veicolo
    {
        public double Potenza { get; set; }

        public VeicoloAMotore(string nome, string descrizione, double prezzo, double potenza) : base(nome, descrizione, prezzo)
        {
            Potenza = potenza;
        }

        internal override double CalcolaIncentivo(bool edit)
        {

             if (Potenza < 30)
             {
                double og = Prezzo;

                if (edit)
                {
                    Prezzo -= Prezzo * 0.05;
                }
                  
                return og * 0.05;
             }
             else
             {
                  return Prezzo;
             }
        }
    }
}
