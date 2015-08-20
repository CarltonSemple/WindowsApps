using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NewTask.Resources;
using System.Linq;

namespace NewTask.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //public LocalDB localDb { get; set; }
        private LocalDB localDb;

        public MainViewModel()
        {
            localDb = new LocalDB("Data Source=isostore:/Data.sdf");
            this.Items = new ObservableCollection<ItemViewModel>();
        }
    

        /// A collection for ItemViewModel objects.
        public ObservableCollection<ItemViewModel> Items { get; private set; }


        public bool IsDataLoaded { get; private set; }

        public void LoadData()
        {
            using (localDb)
            {
                // in case the database does not exist
                if (!localDb.DatabaseExists())
                {
                    localDb.CreateDatabase();
                    localDb.SubmitChanges();

                    
                    localDb.Items.InsertOnSubmit(new ItemViewModel { LineOne = " Example - Swipe the square to the right to delete me.......iahidosnoiah bleinaioh uigoyvuvhjguvoyv hctycit uviutcyxdfhhcxretjdrhgfgftgvfgvcfbv b gfxyttfjdknkiosandhkiedk", LineTwo = "From", LineThree = "james croft" });
                    localDb.SubmitChanges();
                }
                else
                {
                    foreach (var item in localDb.Items)
                    {
                        Items.Add(item);
                    }

                    //if (localDb.Items.Any())
                    //{
                    //    Items = new ObservableCollection<ItemViewModel>(localDb.Items);
                    //}

                    //var itemsInDB = from ItemViewModel item in localDb.Items
                    //                select item;
                    //Items = new ObservableCollection<ItemViewModel>(itemsInDB);
                }
            }

            //localDb.Dispose();

            this.IsDataLoaded = true;
        }

        public void AddItem(ItemViewModel newItem)
        {
            // add to collection
            Items.Add(newItem);

            // Add to database
            localDb.Items.InsertOnSubmit(newItem);

            // Save changes to database
            localDb.SubmitChanges();
        }
        public void DeleteItem(ItemViewModel itemForDelete)
        {
            // Remove from the collection
            Items.Remove(itemForDelete);

            using (localDb)
            {
                // Remove from the database
                if (localDb.DatabaseExists())
                {
                    localDb.Items.DeleteOnSubmit(itemForDelete);

                    // Submit changes to database
                    localDb.SubmitChanges();
                }
            }

         }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}