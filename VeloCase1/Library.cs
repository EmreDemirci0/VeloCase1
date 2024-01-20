using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloCase1
{
    class Library
    {
        private List<Book> books;
        string Paths = Path.Combine(Directory.GetCurrentDirectory(), "library.txt");

        /// <summary>
        /// Bu kısım constructor içerisinden file verilerini çekerek books listesine atar.
        /// </summary>
        public Library()
        {
            books = new List<Book>();

            try
            {
                using (StreamReader sr = new StreamReader(Paths))
                {
                    string satir;
                    while ((satir = sr.ReadLine()) != null)
                    {
                        string[] parcalar = satir.Split(',');

                        // Satırdaki verileri kullanarak Kitap nesnesi oluştur
                        Book book = new Book
                        {
                            title = parcalar[0],
                            author = parcalar[1],
                            ISBN = (parcalar[2]),
                            copyCount = Convert.ToInt16(parcalar[3]),
                            borrowCopyCount = Convert.ToInt16(parcalar[4])
                        };

                        // Oluşturulan Kitap nesnesini listeye ekle
                        books.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dosya okuma hatası: " + ex.Message);
            }

        }

        /// <summary>
        /// Bu kısım Kitap ekleme kısmıdır. Ilgili kısımda book isimli bir nesne atanarak ilgili değerleri dosyaya yükler.
        /// </summary>
        public void AddBook(Book book)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Paths, true))
                {
                    sw.WriteLine($"{book.title},{book.author},{book.ISBN},{book.copyCount},{book.borrowCopyCount}");
                }

                books.Add(book);

                Console.WriteLine("\nYeni kitap eklendi: " + book.title);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dosya yazma hatası: " + ex.Message);
            }
        }

        /// <summary>
        /// Bu kısım Tüm Kitapları görüntüleme kısmıdır.
        /// </summary>
        public void ViewAllBooks()
        {
            if (books.Count > 0)
            {
                Console.WriteLine("\n***** Tüm Kitaplar *****\n");
                foreach (var book in books)
                {
                    PrintToScreenWriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("Kütüphanede gösterilecek Kitap yok...");
            }

        }

        /// <summary>
        /// Bu kısım Kitap bulma kısmıdır. kriter ve değere göre kitabı bulur. kriter baslik veya yazar olabilir. value ise kitabın başlığıdır.
        /// </summary>
        public void FindBook(string criter, string value)
        {
            
            Console.WriteLine($"Aranan {criter} '{value}' için sonuçlar:");
            int count = 0;
            foreach (var book in books)
            {
                if (criter.ToLower() == "baslik" && book.title.ToLower()==value.ToLower())
                {
                    Console.WriteLine($"Başlık: {book.title}, Yazar: {book.author}, ISBN: {book.ISBN}, Kopya Sayısı: {book.copyCount}");
                    count++;
                }
                else if (criter.ToLower() == "yazar" && book.author.ToLower()== value.ToLower())
                {
                    Console.WriteLine($"Başlık: {book.title}, Yazar: {book.author}, ISBN: {book.ISBN}, Kopya Sayısı: {book.copyCount}");
                    count++;

                }

            }
            if (count==0)
            {
                Console.WriteLine("Aranan Kritere göre kitap bulunamadı...");
            }
        }
         
        /// <summary>
        /// Bu kısım Kitap alma kısmıdır. başlığa göre hangi kitap seçili ise o kitabı geçici süreliğine almış olur.
        /// </summary>
        public void BorrowBook(string bookTitle)
        {

            var book = books.Find(x => x.title.ToLower() == bookTitle.ToLower());
           
            if (book != null && book.copyCount > book.borrowCopyCount)
            {
                book.borrowCopyCount++;
                UpdateBorrowOrReturn(book.title, "Plus");
                Console.WriteLine($"{book.title} kitabı ödünç alındı.");
            }
            else if (book is null)
            {
                Console.WriteLine("Kitap ödünç alınamadı. Seçtiğiniz Kitap Kütüphanede bulunmamaktadır...");
            }
            else
            {
                Console.WriteLine("Kitap ödünç alınamadı. Stokta yeterli kopya bulunmamaktadır.");
            }


        }

        /// <summary>
        /// Bu kısım Kitap verme kısmıdır. başlığa göre hangi kitap seçili ise ve varsa o kitabı geçici süreliğine iade etmiş olur.
        /// </summary>
        public void ReturnBook(string bookTitle)
        {
            var book = books.Find(x => x.title.ToLower() == bookTitle.ToLower());
            if (book is not null && book.borrowCopyCount > 0)
            {
                book.borrowCopyCount--;
                Console.WriteLine(book.title + " kitabı iade edildi.");
                UpdateBorrowOrReturn(book.title, "Minus");
            }
            else if (book is null)
            {
                Console.WriteLine("Kitap iade edilemedi. Seçtiğiniz Kitap Kütüphanede bulunmamaktadır...");
            }
            else
            {
                Console.WriteLine("Kitap iade edilemedi. Ödünç alınmış kopya bulunmamaktadır.");
            }
        }

        /// <summary>
        /// Bu kısım Kitap alma ve verme kısmının file kısmına işlenme yeridir. titleToCompare başlığı temsil etmekte ve operation stringi plus ise eklenir, Minus ise çıkarılır.
        /// </summary>
        void UpdateBorrowOrReturn(string titleToCompare, string operation)
        {
            try
            {
                // Dosyadan tüm satırları oku
                List<string> lines = File.ReadAllLines(Paths).ToList();

                for (int i = 0; i < lines.Count; i++)
                {
                    string[] parcalar = lines[i].Split(',');

                    if (parcalar[0].ToLower() == titleToCompare.ToLower())
                    {
                        if (operation == "Plus")
                        {
                            int borrowCopyCount = Convert.ToInt32(parcalar[4]);
                            borrowCopyCount++;
                            parcalar[parcalar.Length - 1] = borrowCopyCount.ToString();
                        }
                        else if (operation == "Minus")
                        {
                            int borrowCopyCount = Convert.ToInt32(parcalar[4]);
                            borrowCopyCount--;
                            parcalar[parcalar.Length - 1] = borrowCopyCount.ToString();
                        }

                        lines[i] = string.Join(",", parcalar);
                    }
                }

                File.WriteAllLines(Paths, lines);

                Console.WriteLine($"{titleToCompare} kitabının kopya sayısı güncellendi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dosya güncelleme hatası: " + ex.Message);
            }
        }

        /// <summary>
        /// Bu kısım Kitabı ekrana yazma kısmıdır.
        /// </summary>
        void PrintToScreenWriteLine(Book book)
        {
            Console.WriteLine($"Başlık: {book.title}, Yazar: {book.author}, ISBN: {book.ISBN}, Kopya Sayısı: {book.copyCount}, Ödünç Alınan Kopyalar: {book.borrowCopyCount}");
        }
    }
}
