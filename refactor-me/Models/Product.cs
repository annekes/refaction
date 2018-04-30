using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        /* 
         * Updates product with changed details
         **/
        public Product Update(Product newProduct)
        {
            Name = newProduct.Name;
            Description = newProduct.Description;
            Price = newProduct.Price;
            DeliveryPrice = newProduct.DeliveryPrice;

            return this;
        }

        /*
         * Save product to db
         **/
        public void Save()
        {
            var cmdStr = IsNew ?
                $"insert into product (id, name, description, price, deliveryprice) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})" :
                $"update product set name = '{Name}', description = '{Description}', price = {Price}, deliveryprice = {DeliveryPrice} where id = '{Id}'";
            Helpers.ExecuteSQL(cmdStr);
        }

        /*
         * Delete product from db
         **/
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