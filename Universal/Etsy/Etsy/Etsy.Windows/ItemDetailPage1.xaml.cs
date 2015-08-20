using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Etsy.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Etsy.Model.Shop;
using Etsy.Model;
using Etsy.Model.Image;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Core;
using Windows.Devices.Sensors;
using Windows.UI.ViewManagement;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Etsy.Model.ShippingNamespace;
using Etsy.Model.Shop.User_Feedback;
using Etsy.DataTransfer;
using Etsy.Model.Variations;
using Etsy.DataBinding;
using Windows.UI;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Etsy
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ItemDetailPage1 : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // page variables
        private Shop shop = new Shop();
        private Listing listing = new Listing();
        private ObservableCollection<Shipping> shipping_info;
        private ObservableCollection<TransactionFeedback> user_reviews;
        private ObservableCollection<Listing> sectionItems = new ObservableCollection<Listing>();
        private string errorMessage;

        // page xaml pointers - meant for accessing nested XAML objects
        ComboBox quantityBox, var1Box, var2Box;
        TextBlock quantityBlock, detailsBlock, bankTransfer, moneyOrder, otherPayment, creditCard;
        Grid variation1_panel, variation2_panel;
        Windows.UI.Xaml.Controls.Image check, payPal;//, bigImage;
        ListView relatedItemsList;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ItemDetailPage1()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            initial_Orientation();
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        

        /// <summary>
        /// Load the item info and set up the page sources according to the item
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //this.DefaultViewModel["itemTitle"] = listing.title;   
            try
            {
                // assign the page listing to the one that was passed as an argument
                listing = (Listing)e.NavigationParameter;
            }
            catch
            {
            }

            // Set up variations ***************************************************
            // See var1Box and var2Box loaded events
            await setupVariations();

            // assign the page shop
            shop.general_info = listing.Shop;

            this.DefaultViewModel["listing"] = listing;
            this.DefaultViewModel["listingImages"] = listing.Images;            // Images  
            this.DefaultViewModel["shopInfo"] = shop.general_info;
            this.DefaultViewModel["shippingList"] = listing.shippingPractical;  //ShippingInfo;      // shipping
            

            // Select the first image to be displayed as the main image
            if(imgListView.Items.Count > 0)
                imgListView.SelectedIndex = 0;
                        
            await loadUserFeedBack();                               // get user feedback
            this.DefaultViewModel["userReviews"] = user_reviews;        // user feedback

            
        }

        private async void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (user_reviews != null)
                if (user_reviews.Count > 0)
                    ;
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        /// <summary>
        /// Handle the initial orientation state
        /// </summary>
        void initial_Orientation()
        {
            // Get the view state
            string CurrentViewState = ApplicationView.GetForCurrentView().Orientation.ToString();

            if (CurrentViewState == "Portrait")
            {
                // images
                bigImage.SetValue(Grid.ColumnSpanProperty, 2);
                bigImage.SetValue(Grid.RowSpanProperty, 1);
                imgListView.SetValue(Grid.RowSpanProperty, 1);
                // border
                border.Visibility = Visibility.Collapsed;
                border_portrait.Visibility = Visibility.Visible;
                // hub
                mainHub.SetValue(Grid.RowProperty, 2);
                mainHub.SetValue(Grid.RowSpanProperty, 1);
                mainHub.SetValue(Grid.ColumnProperty, 0);
                mainHub.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else if (CurrentViewState == "Landscape")
            {
                // images
                bigImage.SetValue(Grid.ColumnSpanProperty, 1);
                bigImage.SetValue(Grid.RowSpanProperty, 2);
                imgListView.SetValue(Grid.RowSpanProperty, 2);
                // border
                border.Visibility = Visibility.Visible;
                border_portrait.Visibility = Visibility.Collapsed;
                // hub
                mainHub.SetValue(Grid.RowProperty, 1);
                mainHub.SetValue(Grid.RowSpanProperty, 2);
                mainHub.SetValue(Grid.ColumnProperty, 1);
                mainHub.SetValue(Grid.ColumnSpanProperty, 1);
            }

        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // Get the new view state
            string CurrentViewState = ApplicationView.GetForCurrentView().Orientation.ToString();

            if (CurrentViewState == "Portrait")
            {
                // images
                bigImage.SetValue(Grid.ColumnSpanProperty, 2);
                bigImage.SetValue(Grid.RowSpanProperty, 1);
                imgListView.SetValue(Grid.RowSpanProperty, 1);
                // border
                border.Visibility = Visibility.Collapsed;
                border_portrait.Visibility = Visibility.Visible;
                // hub
                mainHub.SetValue(Grid.RowProperty, 2);
                mainHub.SetValue(Grid.RowSpanProperty, 1);
                mainHub.SetValue(Grid.ColumnProperty, 0);
                mainHub.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else if (CurrentViewState == "Landscape")
            {
                // images
                bigImage.SetValue(Grid.ColumnSpanProperty, 1);
                bigImage.SetValue(Grid.RowSpanProperty, 2);
                imgListView.SetValue(Grid.RowSpanProperty, 2);
                // border
                border.Visibility = Visibility.Visible;
                border_portrait.Visibility = Visibility.Collapsed;
                // hub
                mainHub.SetValue(Grid.RowProperty, 1);
                mainHub.SetValue(Grid.RowSpanProperty, 2);
                mainHub.SetValue(Grid.ColumnProperty, 1);
                mainHub.SetValue(Grid.ColumnSpanProperty, 1);
            }
            
        }

        /// <summary>
        /// Handle the selection of a different image in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lView = (ListView)sender;
            // get the selected image
            var selectedImage = (Etsy.Model.Image.Image)lView.SelectedItem;

            bigImage.Source = new BitmapImage(
                                new Uri(selectedImage.url_570xN, UriKind.Absolute)); 
        }
        
        /// <summary>
        /// Get the payment options for the listing
        /// Add representative images to the payment options list
        /// </summary>
        private async Task loadPaymentOptions()
        {
            if (listing.Shop.payment_template == null)
                await listing.Shop.getPaymentTemplate();    // Get payment method options

            PaymentTemplate template = listing.Shop.payment_template;

            if (template.allow_bt)      // bank transfer
            {
                bankTransfer.Height = 20;
                bankTransfer.Width = 73;
            }
            if (template.allow_check)   // check
            {
                check.Height = 20;
                check.Width = 73;
            }
            if (template.allow_mo)      // money order
            {
                moneyOrder.Height = 20;
                moneyOrder.Width = 70;
            }   
            if (template.allow_other)   // other payment method
            {
                otherPayment.Height = 20;
                otherPayment.Width = 32;
            }
            if (template.allow_paypal)  // paypal
            {
                payPal.Height = 20;
                payPal.Width = 73;
            }
            if (template.allow_cc)      // credit card
            {
                creditCard.Height = 20;
                creditCard.Width = 60;
            }

            return;
        }

        /// <summary>
        /// Load the user feedback and link it to the page variable for easy data binding
        /// Also load the images for the first 3 feedback items
        /// </summary>
        private async Task loadUserFeedBack()
        {
            //if (listing.Shop.userFeedback == null)
              //  await listing.Shop.getUserFeedback();
            

            if (shop.general_info.userFeedback == null)
                shop.general_info.userFeedback = await DataGET.getUserFeedback(shop.general_info.user_id);        //shop.general_info.userFeedback;  // set the page variable

            user_reviews = new ObservableCollection<TransactionFeedback>();

            int ai = 0;
            foreach(var feedback in shop.general_info.userFeedback)
            {
                if (ai < 3)
                    user_reviews.Add(feedback);
                else
                    break;
                ai++;
            }
            

            // Add necessary info from each feedback's associated listing to the feedback object itself for easier data binding

            if(user_reviews != null)
            {
                for (int i = 0; i < 3 && i < user_reviews.Count; i++) // only do the first 3 reviews for this page
                {
                    var feedback = user_reviews[i];
                    if(feedback.Listing != null)
                        if (feedback.Listing.Images == null)
                        {
                            await feedback.Listing.getImages();
                            if (feedback.Listing.Images != null)
                                if (feedback.Listing.Images.Count > 0)
                                    feedback.item_photo_url = feedback.Listing.Images[0].url_170x135; // Give it a link to the small image for the listing
                        }
                }
            }

            return;
        }
        
        // Button clicked events for Item Details, ratings, and shipping ***************************
        //


        /// <summary>
        /// On load of the quantity selection box, fill the box with the correct amount of options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quantityBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            
            // set page variable for accessing this box later
            quantityBox = box;

            if (listing.quantity > 1)
            {
                for (int i = 0; i < listing.quantity; i++)  // start at 1
                    quantityBox.Items.Add(i + 1);               // Add the quantity options to the dropdown
                quantityBox.SelectedIndex = 0;
            }
            else// hide the quantity options if there's only 1 available, and display text saying as much
            {
                quantityBox.Visibility = Visibility.Collapsed;
                if (listing.quantity == 1)
                    quantityBlock.Text = "1 available";
                else
                {
                    quantityBlock.Foreground = new SolidColorBrush(Colors.Red);
                    quantityBlock.Text = "sold out";
                }
            }
        }

        /// <summary>
        /// Broken up. See the loaded events for the ComboBoxes instead
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task setupVariations()
        {
            if (App.logged_in)
                listing.variations = await DataGET.getVariations(listing.listing_id);
            else
                return;

            for (int i = 0; i < listing.variations.results.Count; i++)              // data bind the variation property objects
                this.DefaultViewModel["variation" + (i + 1).ToString()] = listing.variations.results.ElementAt(i);  

            if (listing.variations.results.Count == 1)                              // only one variation
            {
                variation2_panel.Visibility = Visibility.Collapsed;

                // Fill the ComboBox for variation 1
                VariationsProperty variation1 = listing.variations.results.ElementAt(0);
                var1Box.ItemsSource = variation1.options;
                var1Box.SelectedIndex = 0;
            }
            else if (listing.variations.results.Count < 1)                          // no variations
            {
                variation1_panel.Visibility = Visibility.Collapsed;                 // hide the variations panels since the variations don't exist
                variation2_panel.Visibility = Visibility.Collapsed;
            }
            else if (listing.variations.results.Count == 2)                         // two variations
            {   
                // Fill the ComboBox for variation 1
                VariationsProperty variation1 = listing.variations.results.ElementAt(0);
                var1Box.ItemsSource = variation1.options;
                var1Box.SelectedIndex = 0;

                // Fill the ComboBox for variation 2
                VariationsProperty variation2 = listing.variations.results.ElementAt(1);
                var2Box.ItemsSource = variation2.options;
                var2Box.SelectedIndex = 1;
            }

            return;
        }

        /// <summary>
        /// Save the quantity and variation decisions, and add then add to the cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void addToCart_Button_Click(object sender, RoutedEventArgs e)
        {
            // Set the selected quantity
            if (quantityBox.SelectedIndex >= 0)
                listing.quantity_chosen = quantityBox.SelectedIndex + 1;
            else
                listing.quantity_chosen = 1;

            // Mark the final decisions on the variations
            if(var1Box.SelectedIndex >= 0)          // option 1
            {
                Option option1 = var1Box.SelectedItem as Option;
                listing.variations.results.ElementAt(0).selected_option_id = option1.value_id;

                if(var2Box.SelectedIndex >= 0)      // option 2
                {
                    Option option2 = var2Box.SelectedItem as Option;
                    listing.variations.results.ElementAt(1).selected_option_id = option2.value_id;
                }
            }

            //await CartAccess.addToCart(listing);
            await CartAccess.addToCart_WithVariations(listing);
        }

        private void quantityBlock_Loaded(object sender, RoutedEventArgs e)
        {
            quantityBlock = (TextBlock)sender;
        }

        private async void variation1_panel_Loaded(object sender, RoutedEventArgs e)
        {
            variation1_panel = (Grid)sender;

            if (!App.logged_in)
                return;

            if (listing.variations == null)
                listing.variations = await DataGET.getVariations(listing.listing_id);

            if(listing.variations != null)
                if (listing.variations.results.Count < 1)
                    variation1_panel.Visibility = Visibility.Collapsed;
        }

        private async void variation2_panel_Loaded(object sender, RoutedEventArgs e)
        {
            variation2_panel = (Grid)sender;

            if (!App.logged_in)
                return;

            if (listing.variations == null)
                listing.variations = await DataGET.getVariations(listing.listing_id);

            if(listing.variations != null)
                if (listing.variations.results.Count < 2)
                    variation2_panel.Visibility = Visibility.Collapsed;
        }

        private async void var1Box_Loaded(object sender, RoutedEventArgs e)
        {
            var1Box = (ComboBox)sender;

            if (App.logged_in)
                if(listing.variations == null)
                    listing.variations = await DataGET.getVariations(listing.listing_id);
            else
                return;

            if (listing.variations.results.Count < 1)                          // no variations
            {
                var1Box.Visibility = Visibility.Collapsed;                 // hide the variations panels since the variations don't exist
            }
            else if (listing.variations.results.Count >= 1)                              // only one variation
            {
                this.DefaultViewModel["variation1"] = listing.variations.results.ElementAt(0);             // data bind the variation property objects
                // Fill the ComboBox for variation 1
                VariationsProperty variation1 = listing.variations.results.ElementAt(0);
                var1Box.ItemsSource = variation1.options;
                var1Box.SelectedIndex = 0;
            }

            return;
        }

        private async void var2Box_Loaded(object sender, RoutedEventArgs e)
        {
            var2Box = (ComboBox)sender;

            if (App.logged_in)
                if(listing.variations == null)
                    listing.variations = await DataGET.getVariations(listing.listing_id);
            else
                return;
                                        
            if (listing.variations.results.Count < 2)                          // less than 2 variations
            {
                var2Box.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.DefaultViewModel["variation2"] = listing.variations.results.ElementAt(1);              // data bind the variation property objects
                // Fill the ComboBox for variation 2
                VariationsProperty variation2 = listing.variations.results.ElementAt(1);
                var2Box.ItemsSource = variation2.options;
                var2Box.SelectedIndex = 0;
            }

            return;
        }

        private void detailsBlock_Loaded(object sender, RoutedEventArgs e)
        {
            detailsBlock = (TextBlock)sender;
        }

        /************************* Payment XAML Loading *************************/
        private async void bankTransfer_Loaded(object sender, RoutedEventArgs e)
        {
            bankTransfer = (TextBlock)sender;

            try
            {
                if (listing.Shop.payment_template == null)
                await listing.Shop.getPaymentTemplate();    // Get payment method options

                PaymentTemplate template = listing.Shop.payment_template;

                if (template.allow_bt)      // bank transfer
                {
                    bankTransfer.Height = 20;
                    bankTransfer.Width = 73;
                }
            }
            catch (Exception ex)
            {
                
            }
            
            return;
        }

        private async void check_Loaded(object sender, RoutedEventArgs e)
        {
            check = (Windows.UI.Xaml.Controls.Image)sender;

            try
            {
                if (listing.Shop.payment_template == null)
                    await listing.Shop.getPaymentTemplate();    // Get payment method options

                PaymentTemplate template = listing.Shop.payment_template;

                if (template.allow_check)   // check
                {
                    check.Height = 20;
                    check.Width = 73;
                }
            }
            catch(Exception ex)
            {

            }
            return;
        }

        private async void moneyOrder_Loaded(object sender, RoutedEventArgs e)
        {
            moneyOrder = (TextBlock)sender;

            try
            {
                if (listing.Shop.payment_template == null)
                    await listing.Shop.getPaymentTemplate();    // Get payment method options

                PaymentTemplate template = listing.Shop.payment_template;

                if (template.allow_mo)      // money order
                {
                    moneyOrder.Height = 20;
                    moneyOrder.Width = 70;
                }
            }
            catch(Exception ex)
            {

            }
            return;
        }

        private async void otherPayment_Loaded(object sender, RoutedEventArgs e)
        {
            otherPayment = (TextBlock)sender;

            try
            {
                if (listing.Shop.payment_template == null)
                    await listing.Shop.getPaymentTemplate();    // Get payment method options

                PaymentTemplate template = listing.Shop.payment_template;

                if (template.allow_other)   // other payment method
                {
                    otherPayment.Height = 20;
                    otherPayment.Width = 32;
                }
            }
            catch(Exception ex)
            {

            }
            return;
        }

        private async void payPal_Loaded(object sender, RoutedEventArgs e)
        {
            payPal = (Windows.UI.Xaml.Controls.Image)sender;

            try
            {
                if (listing.Shop.payment_template == null)
                    await listing.Shop.getPaymentTemplate();    // Get payment method options

                PaymentTemplate template = listing.Shop.payment_template;

                if (template.allow_paypal)  // paypal
                {
                    payPal.Height = 20;
                    payPal.Width = 73;
                }
            }
            catch(Exception ex)
            {

            }
            return;
        }

        private async void creditCard_Loaded(object sender, RoutedEventArgs e)
        {
            creditCard = (TextBlock)sender;

            try
            {
                if (listing.Shop.payment_template == null)
                    await listing.Shop.getPaymentTemplate();    // Get payment method options

                PaymentTemplate template = listing.Shop.payment_template;

                if (template.allow_cc)      // credit card
                {
                    creditCard.Height = 20;
                    creditCard.Width = 60;
                }
            }
            catch(Exception ex)
            {

            }
            return;
        }

        private void bigImage_Loaded(object sender, RoutedEventArgs e)
        {
            //bigImage = (Windows.UI.Xaml.Controls.Image)sender;
            bigImage.Source = new BitmapImage(
                                    new Uri(listing.Images[0].url_570xN, UriKind.Absolute)); // the page has already been loaded, so immediately set the big image source
        }

        private void imgListView_Loaded(object sender, RoutedEventArgs e)
        {
            //imgListView = (ListView)sender;
        }

        private void pageTitle_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock pTitle = (TextBlock)sender;
            pTitle.Text = listing.title;
        }

        /// <summary>
        /// Load the related items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void relatedItemsList_Loaded(object sender, RoutedEventArgs e)
        {
            relatedItemsList = (ListView)sender;                            // set up the reference to this listview

            try
            {
                if (sectionItems == null)
                    sectionItems = new ObservableCollection<Listing>();
                if (sectionItems.Count == 0)
                {
                    sectionItems = await DataGET.getListings(listing.Shop.shop_id, listing.Section.shop_section_id, "active", "", new List<Parameter>(), App.defaultAddress.country_id, 0, 6);
                    relatedItemsList.ItemsSource = sectionItems;
                }
            }
            catch (Exception data_e)
            {
                errorMessage = data_e.Message;
            }
        }

        /// <summary>
        /// Go to the page for the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void relatedItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemDetailPage1), relatedItemsList.SelectedItem);
        }

        private void shopPageLink_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShopPage), shop.general_info);
        }

        private void ratingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReviewsPage), shop.general_info);
        }

        
    }
}