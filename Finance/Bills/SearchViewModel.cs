using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhpaAll.Bills
{
    public class SearchResultModel
    {
        public int Type { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class SearchViewModel
    {
        private readonly IList<SearchResultModel> _results;
        public SearchViewModel()
        {
            _results = new List<SearchResultModel>();
        }

        public IList<SearchResultModel> Results
        {
            get
            {
                return _results;
            }
        }
    }
}