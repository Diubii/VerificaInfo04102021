using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerificaInfo41021
{
    abstract class Veicolo
    {
        public string Nome { get; set; }
        public string VIN { get; set; }
        public string Descrizione { get; set; }
        public double Prezzo { get; set; }
        public Veicolo(string nome, string descrizione, double prezzo)
        {
            Nome = nome;
            VIN = FormVerifica.RandomString(8);
            Descrizione = descrizione;
            Prezzo = prezzo;
        }

        internal abstract double CalcolaIncentivo(bool edit);
    }
}
