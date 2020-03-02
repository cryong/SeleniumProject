using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumProject.Data
{
    public enum Type
    {
        Time,
        Material
    }

    public class TimeAndMaterial
    {
        private Type type = Type.Material;
        private string code;
        private string description;
        private string price;
        public Type Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Price
        {
            get
            {
                return price;
            }
            set
            {
                // only positive value and valid numeric values are allowed
                decimal n;
                if (!decimal.TryParse(value, out n) || value.StartsWith("-"))
                {
                    price = "0";
                }
                else
                {
                    // round to 2 d.p
                    price = Math.Round(n, 2).ToString("F");
                }
            }
        }

        public TimeAndMaterial()
        {

        }

        public TimeAndMaterial(string code, string description, string price)
        {
            this.Code = code;
            this.Description = description;
            this.Price = price;
        }

        public TimeAndMaterial(Type type, string code, string description, string price)
        {
            this.Type = type;
            this.Code = code;
            this.Description = description;
            this.Price = price;
        }
    }

}
