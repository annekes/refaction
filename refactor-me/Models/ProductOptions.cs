using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions(productId.ToString());
        }

        private void LoadProductOptions(string productId)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var where = $"where productid = '{productId}'";
            var cmd = new SqlCommand($"select id from productoption {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new ProductOption(id));
            }
        }
    }
}