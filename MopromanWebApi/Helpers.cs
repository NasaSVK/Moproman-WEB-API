using MopromanWebApi.Models;
using System.Diagnostics;
using System.Collections;

namespace MopromanWebApi
{
    public static class Helpers
    {
        public const int MIN_POCET_HODNOT = 200;
        public static List<Record> RedukujPocetHodnot(List<Record> pList){
        //console.log("POVODNE POLE: " + pPole);
        var dlzka = pList.Count; 
            if (dlzka == 0) return new List<Record>();
        var MAX_INDEX = dlzka - 1;
        decimal PER = Math.Floor((decimal)(dlzka / MIN_POCET_HODNOT));
        if (PER == 0) PER = 1;
        List<Record> result = pList.Where((hodnota,  index) => index % PER == 0).ToList();
        Debug.WriteLine("################################################################");
        Debug.WriteLine("REDUKOVANE POLE: "+ result);
        Debug.WriteLine("################################################################");
        if (MAX_INDEX % PER != 0)
               result.Add(pList[MAX_INDEX]); //vlozi do redukovaneho pola posledny prvok pola povodneho        
        return result;
        }
    }
}
