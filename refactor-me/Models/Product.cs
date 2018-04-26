using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id)
        {
            IsNew = true;

            //Confirm that the Product exists.
            var rdr = Helpers.ExecuteSQL($"select * from product where id = '{id}'");
            if (rdr.Read())
            {
                IsNew = false;
                Id = Guid.Parse(rdr["Id"].ToString());
                Name = rdr["Name"].ToString();
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                Price = decimal.Parse(rdr["Price"].ToString());
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
            }

        }

        public void Save()
        {
            var cmdText = IsNew ?
                $"insert into product (id, name, description, price, deliveryprice) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})" :
                $"update product set name = '{Name}', description = '{Description}', price = {Price}, deliveryprice = {DeliveryPrice} where id = '{Id}'";
            Helpers.ExecuteSQL(cmdText);
        }

        /* 
         * Updates this Product with the Name, Description, Price, and DeliveryPrice of newProduct
         **/
        public Product Update(Product newProduct)
        {
            Name = newProduct.Name;
            Description = newProduct.Description;
            Price = newProduct.Price;
            DeliveryPrice = newProduct.DeliveryPrice;

            return this;
        }
        
        public List<ProductOption> Options()
        {
            List<ProductOption> productOptions = new List<ProductOption>();
            var rdr = Helpers.ExecuteSQL($"select id from productoption where productid = '{Id}'");

            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                productOptions.Add(new ProductOption(id));
            }
            return productOptions;
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
            {
                option.Delete();
            }

            Helpers.ExecuteSQL($"delete from product where id = '{Id}'");
        }
    }
}