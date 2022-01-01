/* toto.txt http://www.infojegyzet.hu/erettsegi/informatika-ismeretek/kozep-prog-2020okt/
Év;Hét;Forduló;T13p1;Ny13p1;Eredmények
2020;18;1;0;0;2X22211X1X11X1
• Év : A fogadási forduló éve (1998-2020)
• Hét : A fogadási forduló hete (1-53)
• Forduló : Forduló sorszáma (1 vagy 2)
• T13p1 : 13+1 találatos (telitalálatos) szelvények darabszáma
• Ny13p1 : Egy darab 13+1 találatos szelvény után fizetett nyeremény [Ft]
• Eredmények : A forduló 14 mérkőzésének kimenetelei
*/

using System;                       // Console
using System.IO;                    // StreamReader() StreamWriter()
using System.Collections.Generic;   // List<>
using System.Linq;                  // from where select

class Toto
{
    public int      ev          {get; set;}
    public int      het         {get; set;}
    public int      fordulo     {get; set;}
    public Int64    t13p1       {get; set;}
    public Int64      ny13p1      {get; set;}
    public string   eredmenyek  {get; set;}

    public Toto(string sor)
    {
        var s = sor.Split(';');
        ev      = int.Parse( s[0] );
        het     = int.Parse( s[1] );
        fordulo = int.Parse( s[2] );
        t13p1   = Int64.Parse( s[3] );
        ny13p1  = Int64.Parse( s[4] );
        eredmenyek         = s[5];
    }
}

class EredmenyElemzo
{
    private string Eredmenyek;

    private int DontetlenekSzama
    {
        get
        {
            return Megszamol('X');
        }
    }

    private int Megszamol(char kimenet)
    {
        int darab = 0;
        foreach (var i in Eredmenyek)
        {
            if (i == kimenet) darab++;
        }
        return darab;
    }

    public bool NemvoltDontetlenMerkozes
    {
        get
        {
            return DontetlenekSzama == 0;
        }
    }

    public EredmenyElemzo(string eredmenyek) // konstruktor
    {
        Eredmenyek = eredmenyek;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // 2. Olvassa be a toto.txt állományban lévő adatokat
        var lista = new List<Toto>();
        var sr    = new StreamReader("toto.txt");
        var elsosor = sr.ReadLine();

        while(!sr.EndOfStream)
        {
            var sor = sr.ReadLine();
            lista.Add( new Toto(sor) );
        }
        sr.Close();

        // 3. Határozza meg és írja ki a képernyőre, hány forduló adatai találhatók a forrásállományban!
        // 3. feladat: Fordulók száma: {}

        Console.WriteLine($"3. feladat: Fordulók száma: {lista.Count}");

        // 4. Számolja meg és írja ki a képernyőre a telitalálatos szelvények számát!
        var telitalalat = 
        (
            from sor in lista
            select sor.t13p1
        ).Sum();
        
        Console.WriteLine($"4. feladat: Telitalálatos szelvények száma: {telitalalat}");
        /*
        5. Számítsa ki, mekkora volt a „telitalálatos” ( T13p1>0 vagy Ny13p1>0 ) fordulók során a telitalálatos szelvényekre kifizetett nyereményösszegek átlaga! Egy fordulóban a nyereményösszeget a T13p1 * Ny13p1 kifejezéssel számolja! Ügyeljen rá, hogy a telitalálatos fordulók során a telitalálatos szelvényekre kifizetett nyereményösszegek összege nem fér el egy 32  bites egész változóban. Az átlagot egész számra kerekítve jelenítse meg!
        */

        var darab_szorozva_nyeremeny = 
        (
            from sor in lista
            where sor.ny13p1 > 0
            select sor.ny13p1 * sor.t13p1
        );
      
        Console.WriteLine($"5. feladat: Átlag: {darab_szorozva_nyeremeny.Sum()/lista.Count:0.} Ft");
    
        // 6. Írja ki annak a két fordulónak az adatait a minta szerint, ahol a legnagyobb és a legkisebb volt az egy telitalálatos szelvény után fizetett nyeremény! Feltételezheti, hogy nem alakult ki holtverseny a két szélsőértéknél és nem fordult olyan elő, hogy a telitalálatos szelvény után ne fizettek volna nyereményt!
        
        var nyeremeny = 
        (
            from sor in lista
            where sor.ny13p1 > 0
            orderby sor.ny13p1
            select sor
        );
        var maxi = nyeremeny.Last();
        var mini = nyeremeny.First();

        Console.WriteLine($"6. feladat:");
        Console.WriteLine($"        Legnagyobb:");
        Console.WriteLine($"        Év: {maxi.ev}");
        Console.WriteLine($"        Hét: {maxi.het}.");
        Console.WriteLine($"        Forduló: {maxi.fordulo}.");
        Console.WriteLine($"        Telitalálat: {maxi.t13p1} db");
        Console.WriteLine($"        Nyeremény: {maxi.ny13p1} Ft");
        Console.WriteLine($"        Eredmények: {maxi.eredmenyek}");
        Console.WriteLine();
        Console.WriteLine($"        Legkisebb:");
        Console.WriteLine($"        Év: {mini.ev}");
        Console.WriteLine($"        Hét: {mini.het}.");
        Console.WriteLine($"        Forduló: {mini.fordulo}.");
        Console.WriteLine($"        Telitalálat: {mini.t13p1} db");
        Console.WriteLine($"        Nyeremény: {mini.ny13p1} Ft");
        Console.WriteLine($"        Eredmények: {mini.eredmenyek}");

        /* 7. Forráskódjába tegye elérhetővé a java.txt vagy a csharp.txt állományból az EredmenyElemzo osztályt definiáló kódrészletet!
        Az osztály felhasználható arra, hogy megállapítsa egy forduló eredményeiről (pl.: „2X22211X1X11X1” ), hogy egyetlen mérkőzés sem végződött döntetlen eredménnyel ( NemvoltDontetlenMerkozes ).    
        */
        var dontetlen_nelkul = 
        (
            from sor in lista
            select  new EredmenyElemzo(sor.eredmenyek)
                into res
                where !res.NemvoltDontetlenMerkozes 
                select true
        );

        if (dontetlen_nelkul.Any()) Console.WriteLine( "8. feladat: Volt döntetlen nélküli forduló!" );
        else                        Console.WriteLine( "8. feladat: Nem volt döntetlen nélküli forduló!" );


    } // ----- end of Main method ----
} // -------- end of Program class ------        