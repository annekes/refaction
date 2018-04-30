using System;
using System.Collections.Generic;

namespace refactor_me.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(Guid.Empty);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions(productId);
        }

        /*
         * Loads list of product options according to id, if no name given it lists all products.
         **/
        private void LoadProductOptions(Guid productId)
        {
            Items = new List<ProductOption>();

            var cmdStr = "select id from productoption";
            cmdStr = productId.Equals(Guid.Empty) ?
                cmdStr :
                $"{cmdStr} where productid = '{productId}'";

            var rdr = Helpers.ExecuteSQL(cmdStr);

            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new ProductOption(id));
            }
        }
    }
}