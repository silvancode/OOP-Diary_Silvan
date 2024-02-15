using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace DairyConsole {

    [Serializable]
    public class Tagebucheintrag {
        public DateTime Datum { get; set; }
        public string Text { get; set; }
        public Tagebucheintrag() {
        }
        public Tagebucheintrag(DateTime datum, string text) {
            Datum = datum;
            Text = text;
        }
    }
    internal class Program {
        private static void LadenEintrag() {
            // Liste für die Eintäge
            List<Tagebucheintrag> einträge;

            // Deserialisieren der Einträge in der Textdatei
            using (StreamReader reader = new StreamReader("tagebuch.txt")) {
                var serializer = new XmlSerializer(typeof(List<Tagebucheintrag>));
                einträge = (List<Tagebucheintrag>)serializer.Deserialize(reader);
            }

            while (true) {
                Console.WriteLine("Datum (JJJJ-MM-TT):");
                DateTime datum = DateTime.Parse(Console.ReadLine());

                bool vorhanden = false;

                // Liste nach Datum durchsuchen
                foreach (var eintrag in einträge) {
                    if (eintrag.Datum == datum) {

                        // Ausgabe des Eintrags aus der Liste
                        Console.WriteLine("Datum: " + eintrag.Datum);
                        Console.WriteLine("Text: " + eintrag.Text);
                        Console.ReadKey();
                        vorhanden = true;
                        break;
                    }
                }

                if (vorhanden) {
                    Console.WriteLine("Weiteren Eintrag lesen? (j/n)");
                } else {
                    Console.WriteLine("Eintrag nicht gefunden?");
                    break;
                }

                string antwort = Console.ReadLine();

                if (antwort != "j") {
                    break;
                }
            }
        }
        private static void NeuerEintrag() {
            while (true) {
                // Liste der Eintäge
                List<Tagebucheintrag> einträge;

                // Deserialisieren der Einträge in der Textdatei
                using (StreamReader reader = new StreamReader("tagebuch.txt")) {
                    var serializer = new XmlSerializer(typeof(List<Tagebucheintrag>));
                    einträge = (List<Tagebucheintrag>)serializer.Deserialize(reader);
                }

                Console.WriteLine("Datum (JJJJ-MM-TT):");
                DateTime datum = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Neuer Eintrag:");
                string text = Console.ReadLine();

                // Erstellen eines neuen Eintrags
                Tagebucheintrag eintrag = new Tagebucheintrag(datum, text);

                // Eintrag zur Liste hinzufügen
                einträge.Add(eintrag);

                // Serialisieren des Einträge in eine Textdatei
                using (StreamWriter writer = new StreamWriter("tagebuch.txt")) {
                    var serializer = new XmlSerializer(typeof(List<Tagebucheintrag>));
                    serializer.Serialize(writer, einträge);
                }

                Console.WriteLine("Eintrag gespeichert!");

                Console.WriteLine("Weiteren Eintrag hinzufügen? (j/n)");
                string antwort = Console.ReadLine();

                if (antwort != "j") {
                    break;
                }
            }
        }
        static void Main(string[] args) {
            
            // Variablen
            string benutzername;
            string passwort;
            bool eingeloggt = false;

            // Login
            while (!eingeloggt) {
                Console.Write("Benutzername: ");
                benutzername = Console.ReadLine();

                Console.Write("Passwort: ");
                passwort = Console.ReadLine();

                if (benutzername == "s" && passwort == "f") {
                    eingeloggt = true;
                } else {
                    Console.WriteLine("Benutzername oder Passwort ist falsch!\n");
                }

            }

            if (!File.Exists("tagebuch.txt")) {
                Console.WriteLine("XML-Datei nicht vorhanden, soll eine neue erstellt werden? (j/n)");
                string antwort = Console.ReadLine();

                if (antwort == "j") {

                    // Erstellen einer neuen XML-Datei
                    List<Tagebucheintrag> einträge = new List<Tagebucheintrag>();

                    using (StreamWriter writer = new StreamWriter("tagebuch.txt")) {
                        var serializer = new XmlSerializer(typeof(List<Tagebucheintrag>));
                        serializer.Serialize(writer, einträge);
                    }

                } else {
                    // Ende
                    Console.WriteLine("Ende");
                    Console.ReadKey();
                }

            }

            // Auswahl Lesen oder Schreiben
            while (eingeloggt) {
                Console.WriteLine("Möchtest du lesen oder schreiben?");
                Console.WriteLine("l - Lesen");
                Console.WriteLine("s - Schreiben");

                string auswahl = Console.ReadLine();

                if (auswahl == "l") {
                    LadenEintrag();
                    eingeloggt = false;
                } else if (auswahl == "s") {
                    NeuerEintrag();
                    eingeloggt = false;
                } else {
                    Console.WriteLine("Ungültige Auswahl.");
                }
            }

            // Ende
            Console.WriteLine("Ende");
            Console.ReadKey();
        }
    }
}
