using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerificaInfo41021
{
    class VeicoloSenzaMotore : Veicolo
    {
        public int NRuote { get; set; }

        public VeicoloSenzaMotore(string nome, string descrizione, double prezzo, int nruote) : base(nome, descrizione, prezzo)
        {
            NRuote = nruote;
        }

        internal override double CalcolaIncentivo(bool edit)
        {
            double og = Prezzo;

            if (edit)
            {                
                Prezzo -= Prezzo * 0.2;
            }      
            
            return og * 0.2;
        }
    }
}
