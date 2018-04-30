using System;
using System.Collections.Generic;

namespace refactor_me.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts(name);
        }

        /*
         * Loads list of products according to name, if no name given it lists all products.
         **/
        private void LoadProducts(string name)
        {
            Items = new List<Product>();

            var cmdStr = "select id from product";
            if (name != null)
            {
                cmdStr = $"{cmdStr} where lower(name) like '%{name.ToLower()}%'";
            }

            var rdr = Helpers.ExecuteSQL(cmdStr);

            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new Product(id));
            }
        }
    }
}