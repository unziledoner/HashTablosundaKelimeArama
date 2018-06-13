using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
namespace muh
{
    class Program
    {
        public static int kelimeSira = 1;
        static void Main(string[] args)
        {
            string[] yerlestirmeSonuc = yerlestirme(); //Hash tablomuza eklenen kelime dizisini döndürür.

            for(int i = 0; i < yerlestirmeSonuc.Length; i++) //Hash tablomuzdan dönen diziyi ekrana bastırıyoruz.
            {
                if (yerlestirmeSonuc[i] != null) //Boş dizi değerleriniz eliyoruz.
                {
                    Console.WriteLine(i+". indisteki değer= "+yerlestirmeSonuc[i]);
                }
            }
            Console.WriteLine("");
            int aramaKontrol = 0; //Tekrar arama yapılacak mı sorusu için kontrol oluşturur
            while (aramaKontrol == 0) //kontrol 0 ise tekrar arama yapılır, 1 ise arama sonlanır
            {
                Console.Write("Kelimeyi Giriniz: ");
                string aranan = Console.ReadLine();
                string aramaSonuc = arama(aranan); // Arama fonksiyonundan dönen sonuç
                Console.WriteLine(aramaSonuc); 
                Console.WriteLine("");
                Console.Write("Tekrar arama yapmak istiyorsanız 'e' , arama işlemini sonlandırmak için 'h' yazınız: "); //Tekrar arama için kullanıcıdan değer alır
                string aramaTekrar = Console.ReadLine();
                if (aramaTekrar == "e") // alınan degere göre while tekrar eder veya işlemi sonlandırır
                {
                    aramaKontrol = 0;
                }
                else if (aramaTekrar == "h")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Arama işleminiz sonlandı.");
                    aramaKontrol = 1;
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Böyle bir seçenek bulunmamaktadır");
                }
            }
            Console.ReadLine();
        }
        public static string[] yerlestirme()
        {
            StreamReader sr = new StreamReader("metin.txt");
            string kelime;
            kelime = sr.ReadLine();
            int toplam = 0;
            string[] dizi = new string[211];
            for (int a = 0; a < dizi.Length; a++) //dizimin tüm elemanlarına null degeri attım
            {
                dizi[a] = null;
            }
            while (kelime != null) //tum kelimeleri okuyana kadar dongu dönsün
            {
                byte[] Numara;
                char[] karakter;
                karakter = kelime.ToCharArray();
                Numara = System.Text.Encoding.UTF8.GetBytes(karakter); //kelimedeki karakterlerin tek tek karsılıgını bulup numara dizisine attım
                toplam = 0; //toplama üstüste yazmaması icin
                int j = 1;
                for (int i = 0; i < karakter.Length; i++)
                {
                    int agirlik = j * Numara[i];
                    toplam += agirlik; //bulunan agırlıgı toplam dizisina attım
                    j++; //kelime karakter sayısı kadar artsın
                }
                int indis = toplam % 211; //kelimenin konulacagı indisi buldum

                if (dizi[indis] == null) //indis boş ise kelimeyi o indise yerlestirdim
                {
                    dizi[indis] = kelime + "-" + toplam;
                }
                else //indis bos degilse 
                {
                    int artir = 1;
                    while (artir < 14) //diziyi tasmayacak kadar artsın
                    {
                        indis = (indis + artir * artir) % 211; // (indis*i^2)%211 ile bos indis aradık
                        if (dizi[indis] == null) //bulunan indis bos ise yerlestir
                        {
                            dizi[indis] = kelime + "-" + toplam;
                            break;
                        }
                        artir++;
                    }
                }
                kelime = sr.ReadLine();
            }
            return dizi;
            Console.ReadLine();
            sr.Close();
        }
        public static string arama(string aranan) //Arama fonksiyonu
        {
            int sayac = 0; 
            int diziKontrol = 0;
            Boolean kelimeVarmi = false;
            string[] parca = new string[211];
            string[] yerlestir = yerlestirme(); //kelimeleri diziye yerlestirme fonksiyonu
            for (int b = 0; b < 211; b++)
            {
                if (yerlestir[b] != null) //bos olmayan indisleri almak için
                {
                    parca[b] = yerlestir[b]; //yerlestirden aldıgımız bos olmayan indisleri parca dizisine ekliyoruz
                    string[] gelen = parca[b].Split('-'); //parca dizisinde her indis kelime-ağırlık içerir, indis içindeki elemanı parçalayıp gelen dizimize attık
                    //gelen dizisi parça dizisinin 2 katı indise sahiptir.
                    for (int j = 0; j < gelen.Length; j += 2) // for döngümüzü 2şer 2şer artırıyoruz. Çünkü hem ağırlığı hem de kelimeyi alabilmek için
                    {
                        string parcaKelime = gelen[j]; //ilk indis kelime
                        string parcaAgirlik = gelen[j + 1]; //ikinci indis ağırlık
                        if (aranan == parcaKelime && sayac == 0) // aranan kullanıcıdan alınan değer, hash de varsa kelime bulundu mesajı verilir
                        {
                            sayac = sayac + 1; //eğer aranan kelimeden birden fazla varsa tek sefer yazmak için
                            diziKontrol = diziKontrol + 1; //en son tüm dizi tarandığında kelime yoksa for dışındaki DİZİ KONTROL kısmına girebilmek için
                            kelimeVarmi = true; //aranan kelime yoksa diğer if e girer
                            //arama işlemi tüm indisleri tek tek döndüğü için diziKontrol değişkenimizi arttırdık ,daha sonra for bitince kelime yoksa DİZİ KONTROL koşulumuzda for ile dönerek diğer işlemlerimizi gerçekleştiriyoruz.
                            return "Aradığınız '" + parcaKelime + "' kelimesi bulundu.";
                        }
                        else if (aranan == parcaAgirlik && sayac == 0)
                        {
                            sayac = sayac + 1;
                            diziKontrol = diziKontrol + 1;
                            kelimeVarmi = true;
                            return "Aradığınız '" + parcaAgirlik + "' ağırlık bulundu.";
                        }
                    }
                }
            }
            sayac = 0;//sayacı yeni işlemler için ilk değere döndürdük
            if (diziKontrol == 0)//DİZİ KONTROL
            {
                for (int b = 0; b < 211; b++)
                {
                    if (yerlestir[b] != null)
                    {
                        parca[b] = yerlestir[b];
                        string[] gelen = parca[b].Split('-');
                        for (int j = 0; j < gelen.Length; j += 2)
                        {
                            string parcaKelime = gelen[j];
                            string parcaAgirlik = gelen[j + 1];
                            string[] fonkDonen = karakterDegistirerek(aranan); //karakterleri yer değişerek oluşturduğumuz kelime dizisini döndürdük
                            for (int i = 0; i < fonkDonen.Length; i++)//karakterleri yer değişen kelime dizisi kadar döndürdük
                            {
                                if (parcaKelime == fonkDonen[i] && sayac == 0) //kelime , hash dizimizde var mı diye kontrol ettik
                                {
                                    sayac = sayac + 1;
                                    kelimeVarmi = true;
                                    return "Aranan '" + aranan + "' kelime yer değiştirerek '" + fonkDonen[i] + "' haliyle bulundu";
                                }
                            }
                        }
                    }
                }
            }
            sayac = 0; 
            if (!kelimeVarmi) //Aramalarda bulunamadıysa son arama işlemi olan KarakterSilerek fonksiyonunu çalıştırmak için
            {
                for (int b = 0; b < 211; b++)
                {
                    if (yerlestir[b] != null)
                    {
                        parca[b] = yerlestir[b];
                        string[] gelen = parca[b].Split('-');
                        for (int j = 0; j < gelen.Length; j += 2)
                        {
                            string parcaKelime = gelen[j];
                            string parcaAgirlik = gelen[j + 1];
                            string[] karakterSilerekDonen = karakterSilerek(aranan);
                            for (int i = 0; i < karakterSilerekDonen.Length; i++)
                            {
                                if (parcaKelime == karakterSilerekDonen[i] && sayac == 0)
                                {
                                    sayac = sayac + 1;
                                    kelimeVarmi = true;
                                    return "Aranan '" + aranan + "' kelime içinden harf silinerek '" + karakterSilerekDonen[i] + "' haliyle bulundu";
                                }
                            }
                        }
                    }
                }
            }
            if (!kelimeVarmi)//aranan kelime yoksa 
            {
                return "Aradığınız Kelime Bulunamadı !!";
            }
            return "";
        }
        public static string[] karakterDegistirerek(string mainAranan) //arama işlemini karakterleri yer değiştirerek yapan fonk
        {
            char[] karakterler = mainAranan.ToCharArray();// aranan kelimeyi karakterlerine parçalayıp karakterler dizisine attım
            int karakterSayisi = karakterler.Length; //kelime uzunluğunu buldum
            string[] yerDegisenDizi = new string[karakterSayisi]; //dinamik diziyi oluşturdum , değişen yeni kelimeleri eklemek için
            char karTut;
            string butunKelime = "";
            for (int i = 0; i < karakterler.Length - 1; i++) //son elemanın devamı olmadığı için uzunlugunun bir eksiğine gittim
            {
                karTut = karakterler[i];//ilk harfi sakladım
                karakterler[i] = karakterler[i + 1]; //ilk harfe bir sonraki harfi atadım
                karakterler[i + 1] = karTut; //bir sonraki harfe , saklanan ilk harfi atadım
                for (int j = 0; j < karakterler.Length; j++)//yeni diziye atamak için yeni gelen kelime değerlerini harf harf ekledim
                {
                    butunKelime = butunKelime + karakterler[j]; //harfleri tek tek yeni kelimeye ekledim
                }
                yerDegisenDizi[i] = butunKelime;//kelimeyi , yeni dizime ekledim
                butunKelime = ""; // üstüne yazıp değerler değişmesin diye karakterlerin yer değişmiş halini ve butunKelime değişkenimi eski değerlerine yükledim
                karakterler = mainAranan.ToCharArray();
            }
            return yerDegisenDizi;
        }
        public static string[] karakterSilerek(string mainAranan)
        {
            char[] karakterler = mainAranan.ToCharArray();
            int karakterSayisi = karakterler.Length;
            string[] karakterSilinenDizi = new string[karakterSayisi];
            string butunKelime = "";
            for (int i = 0; i < karakterler.Length; i++)
            {
                for (int j = 0; j < karakterler.Length; j++)
                {
                    if (karakterler[i] == karakterler[j])
                    {
                        for (int m = 0; m < karakterler.Length; m++)
                        {
                            if (m != i)
                            {
                                butunKelime = butunKelime + karakterler[m];
                            }
                        }
                        karakterSilinenDizi[j] = butunKelime;
                        butunKelime = "";
                        karakterler = mainAranan.ToCharArray();
                    }
                }
            }
            return karakterSilinenDizi;
        }
    }
}