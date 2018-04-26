using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            var rdr = Helpers.ExecuteSQL($"select * from productoption where id = '{id}'");

            if (rdr.Read())
            {
                IsNew = false;
                Id = Guid.Parse(rdr["Id"].ToString());
                ProductId = Guid.Parse(rdr["ProductId"].ToString());
                Name = rdr["Name"].ToString();
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
            }

        }

        public void Save()
        {
            var cmd = IsNew ?
                $"insert into productoption (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')" :
                $"update productoption set name = '{Name}', description = '{Description}' where id = '{Id}'";
            Helpers.ExecuteSQL(cmd);
        }

        /* 
         * Updates this Product with the Name, Description, Price, and DeliveryPrice of newProduct
         **/
        public ProductOption Update(ProductOption newOption)
        {
            Name = newOption.Name;
            Description = newOption.Description;

            return this;
        }

        public void Delete()
        {
            Helpers.ExecuteSQL($"delete from productoption where id = '{Id}'");
        }

        public void AssignProduct(Guid productId)
        {
            ProductId = productId;
        }
    }
}