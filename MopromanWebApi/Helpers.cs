using MopromanWebApi.Models;
using System.Diagnostics;
using System.Collections;

namespace MopromanWebApi
{
    public static class Helpers
    {
        public const int MIN_POCET_HODNOT = 200;
        public static List<Record> RedukujPocetHodnot(List<Record> pList) {
            //console.log("POVODNE POLE: " + pPole);
            var dlzka = pList.Count;
            if (dlzka == 0) return new List<Record>();
            var MAX_INDEX = dlzka - 1;
            int PER = (int)Math.Floor((decimal)(dlzka / MIN_POCET_HODNOT));
            if (PER == 0) PER = 1;
            dodajKumulovanuSpotrebu(ref pList, PER);
            List<Record> result = pList.Where((hodnota, index) => index % PER == 0).ToList();
            Debug.WriteLine("################################################################");
            Debug.WriteLine("REDUKOVANE POLE: " + result);
            Debug.WriteLine("################################################################");
            if (MAX_INDEX % PER != 0)
                result.Add(pList[MAX_INDEX]); //vlozi do redukovaneho pola posledny prvok pola povodneho        
            return result;
        }

        private static void dodajKumulovanuSpotrebu(ref List<Record> pList, int pIndex)
        {
            if (pList == null || pList.Count == 0) return;
            
            //nultemu zaznamu ponechavam jeho aktualnu spotrebu kedze 0 je delitelna bezozvysku pre kazde cislo
            float sucet = pList[0].OkamzitaSpotreba;
            pList[0].OkamzitaSpotreba = sucet;
            
            var dlzka = pList.Count;
            sucet = 0;
            for (int i = 1; i < dlzka; i++)
            {
                sucet += pList[i].OkamzitaSpotreba;
                if (i % pIndex == 0)
                {
                    pList[i].OkamzitaSpotreba = sucet;
                    sucet = 0;
                }
            }
            pList[dlzka - 1].OkamzitaSpotreba = sucet;

        }
    }
    
}
