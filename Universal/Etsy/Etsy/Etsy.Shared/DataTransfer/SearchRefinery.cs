using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Etsy.Model;
using Windows.UI.Text;

namespace Etsy.DataTransfer
{
    public enum SortOptions
    {
        MostRecent,
        Relevancy,      // sort on score, highest score first
        HighestPrice,
        LowestPrice     // sort on price, lowest first
    }

    public class SearchRefinery
    {
        

        /// <summary>
        /// The source of available options for sorting 
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<KeyEnumPair<string, SortOptions, string>> SortingOptions()
        {
            ObservableCollection<KeyEnumPair<string, SortOptions, string>> options = new ObservableCollection<KeyEnumPair<string, SortOptions, string>>();

            options.Add(new KeyEnumPair<string, SortOptions, string>("Most Recent", SortOptions.MostRecent, "Normal"));
            options.Add(new KeyEnumPair<string, SortOptions, string>("Relevancy", SortOptions.Relevancy, "Normal"));
            options.Add(new KeyEnumPair<string, SortOptions, string>("Highest Price", SortOptions.HighestPrice, "Normal"));
            options.Add(new KeyEnumPair<string, SortOptions, string>("Lowest Price", SortOptions.LowestPrice, "Normal"));

            return options;
        }
        
        /// <summary>
        /// Return the parameters necessary for the chosen sort
        /// </summary>
        /// <param name="sort_chosen"></param>
        /// <returns></returns>
        public static List<Parameter> sortingParameters(SortOptions sort_chosen)
        {
            List<Parameter> parameters = new List<Parameter>();

            switch (sort_chosen)
            {
                case SortOptions.MostRecent:
                    parameters.Add(new Parameter("sort_on", "created"));
                    parameters.Add(new Parameter("sort_order", "down"));
                    break;
                case SortOptions.Relevancy:
                    parameters.Add(new Parameter("sort_on", "score"));
                    parameters.Add(new Parameter("sort_order", "down"));
                    break;
                case SortOptions.HighestPrice:
                    parameters.Add(new Parameter("sort_on", "price"));
                    parameters.Add(new Parameter("sort_order", "down"));
                    break;
                case SortOptions.LowestPrice:
                    parameters.Add(new Parameter("sort_on", "price"));
                    parameters.Add(new Parameter("sort_order", "up"));
                    break;
                default:
                    break;
            }

            return parameters;
        }

        /// <summary>
        /// Return the parameters for the given filter values
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Parameter> filterParameters(double minPrice, double maxPrice)
        {
            List<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("min_price", Convert.ToInt32(minPrice).ToString()));            // minimum price
            if(maxPrice < 1000)
                parameters.Add(new Parameter("max_price", Convert.ToInt32(maxPrice).ToString()));        // maximum price if it's less than 1000
            
            return parameters;
        }
    }

    
}
