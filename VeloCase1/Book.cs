using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloCase1
{
    class Book
    {
        public string title{ get; set; }
        public string author{ get; set; }
        public string ISBN{ get; set; }
        public int copyCount{ get; set; }
        public int borrowCopyCount{ get; set; }
        public Book(){}
        public Book(string title, string author, string ISBN, int copyCount)
        {
            this.title = title;
            this.author = author;
            this.ISBN = ISBN;
            this.copyCount = copyCount;
            this.borrowCopyCount = 0; 
        }
    }
}
