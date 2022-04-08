using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public static class DataAccess
    {
        public static ObservableCollection<User> GetUsers()
        {
            return new ObservableCollection<User>(ShopBozyaEntities.GetContext().Users.Where(u => !u.IsDeleted));
        }

        public static User GetUser(int id)
        {
            return GetUsers().Where(user => user.Id == id).FirstOrDefault();
        }

        public static User GetUser(string login, string password)
        {
            return GetUsers().Where(user => user.Login == login && user.Password == password).FirstOrDefault();
        }

        public static bool TryLogin(string login, string password)
        {
            return GetUser(login, password) != null;
        }

        public static bool RegistartionUser(string login, string password)
        {
            User user = new User
            {
                Login = login,
                Password = password,
                RoleId = GetRole("Клиент").Id
            };

            ShopBozyaEntities.GetContext().Users.Add(user);
            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }



        public static bool CheckLogin(string login)
        {
            return GetUsers().Where(user => user.Login == login).Count() == 0;
        }

        public static bool CheckPassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^])(?=.*[^a-zA-Z0-9])\S{6,16}$");

            return regex.IsMatch(password);
        }



        public static ObservableCollection<Role> GetRoles()
        {
            return new ObservableCollection<Role>(ShopBozyaEntities.GetContext().Roles.Where(r => !r.IsDeleted));
        }

        public static Role GetRole(int id)
        {
            return GetRoles().Where(role => role.Id == id).FirstOrDefault();
        }

        public static Role GetRole(string name)
        {
            return GetRoles().Where(role => role.Name == name).FirstOrDefault();
        }


        public static ObservableCollection<Product> GetProducts()
        {
            return new ObservableCollection<Product>(ShopBozyaEntities.GetContext().Products.Where(p => !p.IsDeleted));
        }

        public static bool SaveProduct(Product product)
        {
            if (GetProducts().Where(p => p.Id == product.Id).Count() == 0)
            {
                product.AddDate = DateTime.Now;
                ShopBozyaEntities.GetContext().Products.Add(product);
            }
            else
                ShopBozyaEntities.GetContext().Products.SingleOrDefault(p => p.Id == product.Id);

            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }

        public static bool SaveProductCountries(int productId, List<Country> countries)
        {
            foreach (var country in countries)
            {
                ProductCountry productCountry = new ProductCountry
                {
                    ProductId = productId,
                    CountryId = country.Id
                };

                if (GetProductCountries().Where(p => p.ProductId == productId && p.CountryId == country.Id).Count() == 0)
                {
                    ShopBozyaEntities.GetContext().ProductCountries.Add(productCountry);
                }
            }
            
            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }

        public static bool RemoveProductCounrtry(int productId, int countryId)
        {
            ShopBozyaEntities.GetContext().ProductCountries.Remove(GetProductCountry(productId, countryId));
            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }

        public static ProductCountry GetProductCountry(int productId, int countryId)
        {
            return GetProductCountries().Where(p => p.ProductId == productId && p.CountryId == countryId).FirstOrDefault();
        }

        public static ObservableCollection<ProductCountry> GetProductCountries()
        {
            return new ObservableCollection<ProductCountry>(ShopBozyaEntities.GetContext().ProductCountries);
        }

        public static List<ProductCountry> GetProductCountries(Product product)
        {
            return GetProductCountries().Where(p => p.ProductId == product.Id).ToList();
        }

        public static bool DeleteProduct(Product product)
        {
            product.IsDeleted = true;
            ShopBozyaEntities.GetContext().Products.SingleOrDefault(p => p.Id == product.Id);
            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }

        public static ObservableCollection<Unit> GetUnits()
        {
            return new ObservableCollection<Unit>(ShopBozyaEntities.GetContext().Units.Where(u => !u.IsDeleted));
        }

        public static ObservableCollection<Country> GetCountries()
        {
            return new ObservableCollection<Country>(ShopBozyaEntities.GetContext().Countries.Where(c => !c.IsDeleted));
        }

        public static Country GetCountry(int id)
        {
            return GetCountries().Where(c => c.Id == id).FirstOrDefault();
        }

        public static bool CheckContent(string name, string description)
        {
            Regex regex = new Regex(@"^[А-Яа-яA-Za-z\s\-]+$");
            bool n = regex.IsMatch(name);
            bool d = regex.IsMatch(description);

            return regex.IsMatch(name) && regex.IsMatch(description);
        }
    }
}
