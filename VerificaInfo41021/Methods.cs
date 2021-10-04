using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VerificaInfo41021
{
    class Methods
    {
        internal static void SaveData()
        {
            JsonSerializer serializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            List<VeicoloAMotore> vmot = new List<VeicoloAMotore>();
            List<VeicoloSenzaMotore> vnomot = new List<VeicoloSenzaMotore>();

            foreach (var v in FormVerifica.Veicoli)
            {
                if (v.GetType() == typeof(VeicoloAMotore))
                {
                    vmot.Add((VeicoloAMotore)v);
                }
                else
                {
                    vnomot.Add((VeicoloSenzaMotore)v);
                }
            }

            using (StreamWriter sw = new StreamWriter(@"VeicoliAMotore.Gioia"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, vmot);
                // {"ExpiryDate":new Date(1230375600000),"Price":0}
            }

            using (StreamWriter sw = new StreamWriter(@"VeicoliSenzaMotore.Gioia"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, vnomot);
                // {"ExpiryDate":new Date(1230375600000),"Price":0}
            }
        }

        internal static void LoadData()
        {
            List<VeicoloAMotore> vmot = new List<VeicoloAMotore>();
            List<VeicoloSenzaMotore> vnomot = new List<VeicoloSenzaMotore>();

            if (File.Exists(@"VeicoliAMotore.Gioia"))
            {
                vmot = JsonConvert.DeserializeObject<List<VeicoloAMotore>>(File.ReadAllText(@"VeicoliAMotore.Gioia"));
            }

            if (File.Exists(@"VeicoliSenzaMotore.Gioia"))
            {
                vnomot = JsonConvert.DeserializeObject<List<VeicoloSenzaMotore>>(File.ReadAllText(@"VeicoliSenzaMotore.Gioia"));
            }

            FormVerifica.Veicoli.AddRange(vmot);
            FormVerifica.Veicoli.AddRange(vnomot);
        }

        internal static double CalcAverageIncentive()
        {
            double incentivoMedio = 0;

            if(FormVerifica.Veicoli.Count > 0)
            {
                foreach (var v in FormVerifica.Veicoli)
                {
                    incentivoMedio += v.CalcolaIncentivo(false);
                }

                return incentivoMedio /= FormVerifica.Veicoli.Count;
            }
            else
            {
                return 0;
            }               
        }
    }
}
