using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTask.ViewModels
{
    public class LocalDB : DataContext
    {
        public LocalDB(string connectionString):base(connectionString)
        {

        }

        public Table<ItemViewModel> Items;
    }
}
