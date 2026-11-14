using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Storage.Serialization;

namespace Core.Storage
{

    /*
     * Klasa koja je zadužena za čuvanje podataka u fajl sistemu.
     * Ova klasa vodi računa da odgovarajući fajl postoji,
     * piše i čita podatke iz njega. Storage korsiti Serializer
     * klasu za konverziju objekta u string i obrnuto.
     */

    // ovaj kod je preuzet iz primera sa vezbi i prilagodjen je potrebama projekta
    // izvor: Termn 3 - Kolekcije i serijalizacija - CrudExample.zip
    public class Storage<T> where T : ISerializable, new()
    {
        // puna putanja do fajla
        private readonly string _filePath;
        //private readonly string _fileName = @"../../../Data/{0}";

        // serijalajzer koji pretvara objekte u CSV i nazad
        private readonly Serializer<T> _serializer = new();

        public Storage(string fileName)
        {
            _filePath = Path.Combine("..", "..", "..", "Data", fileName);
        }
        public List<T> Load()
        {
            // ako direktorijum ne postoji - napravi ga
            string directory = Path.GetDirectoryName(_filePath)!;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // ukoliko fajl ne postoji napravi novi fajl i
            // zatvori stream za pisanje u fajl
            if (!File.Exists(_filePath))
            {
                FileStream fs = File.Create(_filePath);
                fs.Close();
            }

            // ucitaj sve linije i deserijalizuj
            IEnumerable<string> lines = File.ReadLines(_filePath);
            List<T> objects = _serializer.FromCSV(lines);

            return objects;
        }

        public void Save(List<T> objects)
        {
            // osiguraj da direktorijum postoji
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            // serijalizuj objekte
            string serializedObjects = _serializer.ToCSV(objects);

            using (StreamWriter streamWriter = new StreamWriter(_filePath))
            {
                streamWriter.Write(serializedObjects);
            }
        }
    }
}
