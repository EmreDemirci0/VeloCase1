using System;

namespace VeloCase1
{
    class Program
    {
        static void Main()
        {
            Library library = new Library();
            bool isContinue = true;

            while (isContinue)
            {
                Console.WriteLine("\n1. Kitap Ekle");
                Console.WriteLine("2. Tüm Kitapları Görüntüle");
                Console.WriteLine("3. Kitap Ara");
                Console.WriteLine("4. Kitap Ödünç Al");
                Console.WriteLine("5. Kitap İade Et");
                Console.WriteLine("6. Süresi Geçmiş Kitapları Görüntüle");
                Console.WriteLine("0. Çıkış");

                Console.Write("\nSeçiminizi yapınız: ");
                string select = Console.ReadLine();

                switch (select)
                {
                    case "1":
                        Console.WriteLine("\n***** Yeni Kitap Ekleyin *****");
                        Book book = new Book("Başlık", "Yazar", "ISBN", 0);
                        Console.Write("Başlık: ");
                        book.title = Console.ReadLine();
                        Console.Write("Yazar: ");
                        book.author = Console.ReadLine();
                        Console.Write("ISBN: ");
                        book.ISBN = Console.ReadLine();
                        Console.Write("Kopya Sayısı: ");
                        int kopyaSayisi;
                        if (int.TryParse(Console.ReadLine(), out kopyaSayisi))
                        {
                            book.copyCount = kopyaSayisi;
                            library.AddBook(book);
                        }
                        else
                        {
                            Console.WriteLine("Geçersiz kopya sayısı girişi.");
                        }
                        break;

                    case "2":
                        library.ViewAllBooks();
                        break;

                    case "3":

                        Console.Write("Arama Kriteri (Baslik/Yazar): ");
                        string criter = Console.ReadLine();
                        Console.Write("Arama Değeri: ");
                        string value = Console.ReadLine();
                        library.FindBook(criter, value);
                        break;

                    case "4":
                        Console.Write("Ödünç almak istediğiniz kitabın başlığını giriniz: ");
                        string borrowBookTitle = Console.ReadLine();
                        library.BorrowBook(borrowBookTitle);
                        break;

                    case "5":
                        Console.Write("İade etmek istediğiniz kitabın başlığını giriniz: ");
                        string returnBookTitle = Console.ReadLine();
                        library.ReturnBook(returnBookTitle);
                        break;

                    case "6":
                        Console.WriteLine("***Süresi geçmiş kitaplarla ilgili bilgileri görüntüleyin. Kısmını anlamadığım için yapmadım *** \n ");
                        break;

                    case "0":
                        isContinue = false;
                        Console.WriteLine("Çıkış yapılıyor...");
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                        break;
                }
            }
        }
    }

}
