using System;

namespace PythTriplev21
{
    class Program
    {
        static void Main(string[] args)
        {      
            // Alles für die Berechnung.
            int u = 2;
            int v = 1;
            int maxTriple = 1;
            int tripleCount = 0;
            int divider = 2;
            bool divable = true;
            bool running = true;

            int x = 0;
            int y = 0;
            int z = 0;

            // Alles für die Konsolenausgabe.
            int toPrint = 0;
            int sleepTime = 0;
            string mode;

            // Alles für den Log Mode und die Log Datei.
            int toLog = 0;
            string logMode = "a";
            int logFileCount = 0;

            // Alles für den weiterModus.
            const int statsCount = 2;
            const int varCount = 7;
            int i = 0;

            // Dateipfade
            string dataPath = "Data/";
            string logPath = System.IO.Path.Combine(dataPath, "logs/"); //Wird weiter unten vervollständigt.
            string statsPath = System.IO.Path.Combine(dataPath, "stats.txt");
            string varPath = System.IO.Path.Combine(dataPath, "var.txt");

            // Erstellung der Ordner (wenn nicht vorhanden).
            System.IO.Directory.CreateDirectory(dataPath);    
            System.IO.Directory.CreateDirectory(logPath);

            // Gewünschter Mode wird abgefragt.
            Console.Write("Wähle den Modus(langsam/normal/schnell/weiter): ");
            mode = Console.ReadLine().ToLower();

            if (mode.StartsWith("w")){
                // Stats Datei wird ausgelesen und die einzelnen Variablen geladen.
                using (System.IO.StreamReader stats = new System.IO.StreamReader(statsPath)){
                    while (i < statsCount){
                        switch(i){                            
                            case 0: u = Convert.ToInt32(stats.ReadLine()); break;
                            case 1: tripleCount = Convert.ToInt32(stats.ReadLine()); break;
                        }
                        i ++;
                    }    
                }
                i = 0;

                using (System.IO.StreamReader var = new System.IO.StreamReader(varPath)){
                    while(i < varCount){
                        switch(i){
                            case 0: logFileCount = Convert.ToInt32(var.ReadLine()); break;
                            case 1: mode = var.ReadLine(); break;
                            case 2: maxTriple = Convert.ToInt32(var.ReadLine()); break;
                            case 3: sleepTime = Convert.ToInt32(var.ReadLine()); break;
                            case 4: logMode = var.ReadLine(); break;
                            case 5: logPath = var.ReadLine(); break;
                            case 6: toLog = Convert.ToInt32(var.ReadLine()); break;
                        }
                        i ++;
                    }
                }
            }
            else{
                // Die einzelnen Variablen werden vom Benutzer bestimmt.    
                Console.Write("Gebe an wie viele Tripeln generiert werden sollen (-1 für endlos): ");
                maxTriple = Convert.ToInt32(Console.ReadLine());
                if (maxTriple == -1){
                    maxTriple = 2000000000;
                }

                Console.Write("Wähle den Log Modus (schnell/komplett/aus): ");
                logMode = Console.ReadLine().ToLower();

                if (mode.StartsWith("l")){
                    Console.Write("Wähle die Pause zwischen den Ausgaben(in ms): ");
                    sleepTime = Convert.ToInt32(Console.ReadLine());
                }

                if (mode.StartsWith("s")){
                    Console.Write("Gebe ein jede wievieltste Tripel angezeigt werden soll: ");
                    toPrint = Convert.ToInt32(Console.ReadLine());
                }

                if (logMode.StartsWith("s")){
                    Console.Write("wähle jede wievielste Tripel geloggt werden soll: ");
                    toLog = Convert.ToInt32(Console.ReadLine());
                }

                // Die Logdatei wird geladen oder erstellt.

                if (System.IO.File.Exists(varPath)){
                    using (System.IO.StreamReader log = new System.IO.StreamReader(varPath)){
                        logFileCount = Convert.ToInt32(log.ReadLine());
                    }
                }

                if (logMode.StartsWith("k") || logMode.StartsWith("s")){
                    logFileCount ++;
                    logPath = logPath + "log" + logFileCount +".txt";

                }
                else{
                    // Wenn der Logmodus aus ist, benötigt der Streamwriter trotzdem eine Datei die geladen wird.
                    // Diese wird nicht verändert. Hier wird var verwendet, dass der Streamwrite keine leere Datei erstellt.
                    logPath = varPath;
                }

            }         

            using (System.IO.StreamWriter var = new System.IO.StreamWriter(varPath)){
                var.WriteLine(logFileCount);
                var.WriteLine(mode);
                var.WriteLine(maxTriple);
                var.WriteLine(sleepTime);
                var.WriteLine(logMode);
                var.WriteLine(logPath);
                var.WriteLine(toLog);
            }

            // Der StreamWriter für die Logdatei wird erstellt.
            using (System.IO.StreamWriter log = new System.IO.StreamWriter(logPath, true))

            while(running){
                while(u > v){
                    if( u%2 == 0 || v%2 == 0){

                        // Erstellung der Tripeln.

                        while(divider <= v){
                            if( u%divider == 0 & v%divider == 0){
                                divable = false;
                            }
                            divider ++;
                         }

                        if(u <= 2 || v <= 1 || divable){

                            x = u*u - v*v;
                            y = 2*u*v;
                            z = u*u + v*v;
                            
                            // Konsolenausgabe bei langsamem oder normalem Modus.
                            if (!mode.StartsWith("s")){
                                Console.WriteLine(x + " " + y + " " + z);
                            }

                            // Log wird beschrieben bei Logmodus komplett.
                            if (logMode.StartsWith("k")){
                                log.WriteLine(x + " " + y + " " +z);
                            }

                            // Pause von Mode langsam wird gemacht.
                            if (mode.StartsWith("l")){
                                System.Threading.Thread.Sleep(sleepTime);
                            }

                            tripleCount ++;

                            // Ausgabe des Modes schnell.
                            if (mode.StartsWith("s") && tripleCount%toPrint == 0){
                                Console.WriteLine("u,v: " + u + " " + v + " a,b,c: " + x + " " + y + " " + z);  
                            }

                            // Log wird im Logmodus schnell beschrieben.
                            if (logMode.StartsWith("s") && tripleCount%toLog == 0){
                                log.WriteLine("u,v: " + u + " " + v + " a,b,c: " + x + " " + y + " " + z);
                            }

                            if (tripleCount == maxTriple){
                            running = false;
                            break;
                            }                              
                        }
                    }   

                    divable = true;
                    divider = 2;
                    v ++;
                }
                
                v = 1;
                u ++;

                //Die Logdatei wird mit dem zuvor "aufgestauten" beschrieben.
                log.Flush();

                //Stats werden geschrieben.

                using (System.IO.StreamWriter stats = new System.IO.StreamWriter(statsPath)){

                    //Wenn weitere stats hinzugefügt werden Statscount updaten!
                    stats.WriteLine(u);
                    stats.WriteLine(tripleCount);                
                }
            }

            // Programmende
            Console.WriteLine("Zum Beenden eine beliebige Taste drücken!");
            Console.ReadKey();
        }
    }
}