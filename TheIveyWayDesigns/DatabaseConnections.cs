using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.SqlServerCompact;
using System.Data.SqlServerCe;
using System.IO;
using System.Data;
using TheIveyWayDesigns.Models;

namespace TheIveyWayDesigns
{
    public class DatabaseConnections
    {
        public SqlCeConnection InitializeDatabase()
        {
            string connectionString = CreateDatabase();
            SqlCeConnection conn = new SqlCeConnection(connectionString);
            conn.Open();

            //CreateTable(conn);

            return conn;
        }

        public IEnumerable<CustomerModel> GetCustomers()
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select * from customers";
                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);
                
                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(c => new CustomerModel()
            {
                Address = c["Address"].ToString(),
                City = c["City"].ToString(),
                CustomerId = Convert.ToInt32(c["CustomerId"].ToString()),
                Name = c["Name"].ToString(),
                PhoneNumber = c["PhoneNumber"].ToString(),
                State = c["state"].ToString(),
                ZipCode = c["ZipCode"].ToString()
            }).ToList();
        }

        public void AddCustomer(CustomerModel customerModel)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "insert into customers (name, address, city, state, zipcode, phonenumber) values (@Name, @Address, @City, @State, @ZipCode, @PhoneNumber)";

                comm.Parameters.AddWithValue("@Name", customerModel.Name);
                comm.Parameters.AddWithValue("@Address", customerModel.Address);
                comm.Parameters.AddWithValue("@City", customerModel.City);
                comm.Parameters.AddWithValue("@State", customerModel.State);
                comm.Parameters.AddWithValue("@ZipCode", customerModel.ZipCode);
                comm.Parameters.AddWithValue("@PhoneNumber", customerModel.PhoneNumber);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        public void UpdateCustomer(CustomerModel customerModel)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "UPDATE Customers SET Name = @Name, Address = @Address, " +
                    "City = @City, State = @State, ZipCode = @ZipCode, PhoneNumber = @PhoneNumber " +
                    "Where CustomerId = @CustomerId";

                comm.Parameters.AddWithValue("@Name", customerModel.Name);
                comm.Parameters.AddWithValue("@Address", customerModel.Address);
                comm.Parameters.AddWithValue("@City", customerModel.City);
                comm.Parameters.AddWithValue("@State", customerModel.State);
                comm.Parameters.AddWithValue("@ZipCode", customerModel.ZipCode);
                comm.Parameters.AddWithValue("@PhoneNumber", customerModel.PhoneNumber);
                comm.Parameters.AddWithValue("@CustomerId", customerModel.CustomerId);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        public void AddOrder(List<AddOrderModel> addOrderModel, int customerId)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            int orderId = 0;

            double orderTotal = addOrderModel.Select(a => a.LineTotal).Sum();

            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "insert into Orders (CustomerId, OrderDate, Shipped, OrderTotal) values (@CustomerId, @OrderDate, @Shipped, @OrderTotal)";

                comm.Parameters.AddWithValue("@CustomerId", customerId);
                comm.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                comm.Parameters.AddWithValue("@Shipped", false);
                comm.Parameters.AddWithValue("@OrderTotal", orderTotal);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.CommandText = "SELECT @@IDENTITY";
                orderId = Convert.ToInt32(comm.ExecuteScalar());



                comm.Connection.Close();
            }

            foreach (var order in addOrderModel)
            {
                using (SqlCeCommand comm = new SqlCeCommand())
                {
                    comm.Connection = new SqlCeConnection(connectionString);
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = "insert into OrderDetails (OrderId, Description, Quantity, Price, LineTotal) " +
                        "values (@OrderId, @Description, @Quantity, @Price, @LineTotal)";

                    comm.Parameters.AddWithValue("@OrderId", orderId);
                    comm.Parameters.AddWithValue("@Description", order.Description);
                    comm.Parameters.AddWithValue("@Quantity", order.Quantity);
                    comm.Parameters.AddWithValue("@Price", order.Price);
                    comm.Parameters.AddWithValue("@LineTotal", order.LineTotal);

                    comm.Connection.Open();
                    comm.ExecuteNonQuery();
                    comm.Connection.Close();

                    
                }

                
            }
            
        }

        public IEnumerable<OrdersModel> GetOrdersForCustomer(int customerId)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select c.Name, c.CustomerId, o.orderdate, o.shipped, o.OrderId, " +
                    "o.OrderTotal from Customers as c inner join Orders as o on c.CustomerId = o.CustomerId where c.CustomerId = @CustomerId " +
                    "and o.Shipped = 0";

                comm.Parameters.AddWithValue("@CustomerId", customerId);

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }            

            return dt.AsEnumerable().Select(o => new OrdersModel()
            {
                Customer = o["Name"].ToString(),
                CustomerId = Convert.ToInt32(o["CustomerId"].ToString()),
                OrderDate = Convert.ToDateTime(o["OrderDate"].ToString()),
                OrderId = Convert.ToInt32(o["OrderId"].ToString()),
                Shipped = Convert.ToBoolean(o["Shipped"].ToString()),
                OrderTotal = Convert.ToDouble(o["OrderTotal"].ToString())
            }).ToList();
        }

        public IEnumerable<OrdersModel> GetAllCurrentOrders()
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select c.Name, c.CustomerId, o.orderdate, o.shipped, o.OrderId, " +
                    "o.OrderTotal from Customers as c inner join Orders as o on c.CustomerId = o.CustomerId where o.Shipped = 0";

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(o => new OrdersModel()
            {
                Customer = o["Name"].ToString(),
                CustomerId = Convert.ToInt32(o["CustomerId"].ToString()),
                OrderDate = Convert.ToDateTime(o["OrderDate"].ToString()),
                OrderId = Convert.ToInt32(o["OrderId"].ToString()),
                Shipped = Convert.ToBoolean(o["Shipped"].ToString()),
                OrderTotal = Convert.ToDouble(o["OrderTotal"].ToString())
            }).ToList();
        }

        public IEnumerable<OrderDetailsModel> GetOrderDetails(int orderId)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select Description, Quantity, Price, LIneTotal, OrderId, OrderDetailsId " +
                    " from OrderDetails where OrderId = @OrderId";

                comm.Parameters.AddWithValue("@OrderId", orderId);

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(o => new OrderDetailsModel()
            {
                OrderId = Convert.ToInt32(o["OrderId"].ToString()),
                OrderDetailsId = Convert.ToInt32(o["OrderDetailsId"].ToString()),
                Description = o["Description"].ToString(),
                Quantity = Convert.ToInt32(o["Quantity"].ToString()),
                Price = Convert.ToDouble(o["Price"].ToString()),
                LineTotal = Convert.ToDouble(o["LineTotal"].ToString())
            }).ToList();
        }

        public IEnumerable<VendorsModel> GetVendors()
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select VendorId, VendorName, Address, City, State, ZipCode, Phone, Website from Vendors";

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(v => new VendorsModel()
            {
                Address = v["Address"].ToString(),
                City = v["City"].ToString(),
                PhoneNumber = v["Phone"].ToString(),
                State = v["State"].ToString(),
                VendorId = Convert.ToInt32(v["VendorId"].ToString()),
                VendorName = v["VendorName"].ToString(),
                Website = v["Website"].ToString(),
                ZipCode = v["ZipCode"].ToString()
            }).ToList();
        }

        public VendorsModel GetVendorById(int vendorId)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select VendorId, VendorName, Address, City, State, ZipCode, Phone, Website from Vendors where VendorId = @VendorId";

                comm.Parameters.AddWithValue("@VendorId", vendorId);

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            DataRow row = dt.Rows[0];

            return new VendorsModel()
            {
                Address = row["Address"].ToString(),
                City = row["City"].ToString(),
                PhoneNumber = row["Phone"].ToString(),
                State = row["State"].ToString(),
                VendorId = Convert.ToInt32(row["VendorId"].ToString()),
                VendorName = row["VendorName"].ToString(),
                Website = row["Website"].ToString(),
                ZipCode = row["ZipCode"].ToString()
            };
        }

        public void UpdateVendor(VendorsModel vendorModel)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "UPDATE Vendors SET VendorName = @VendorName, Address = @Address, " +
                    "City = @City, State = @State, ZipCode = @ZipCode, Phone = @Phone, WebSite = @Website " +
                    "Where VendorId = @VendorId";

                comm.Parameters.AddWithValue("@VendorId", vendorModel.VendorId);
                comm.Parameters.AddWithValue("@VendorName", vendorModel.VendorName);
                comm.Parameters.AddWithValue("@Address", vendorModel.Address);
                comm.Parameters.AddWithValue("@City", vendorModel.City);
                comm.Parameters.AddWithValue("@State", vendorModel.State);
                comm.Parameters.AddWithValue("@ZipCode", vendorModel.ZipCode);
                comm.Parameters.AddWithValue("@Phone", vendorModel.PhoneNumber);
                comm.Parameters.AddWithValue("@Website", vendorModel.Website);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        public IEnumerable<VendorProductsModel> GetVendorProducts(int vendorId)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select VendorProductsId, VendorId, Description, Price" +
                    " from VendorProducts where VendorId = @VendorId";

                comm.Parameters.AddWithValue("@VendorId", vendorId);

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(o => new VendorProductsModel()
            {
                VendorId = Convert.ToInt32(o["VendorId"].ToString()),
                VendorProductsId = Convert.ToInt32(o["VendorProductsId"].ToString()),
                Description = o["Description"].ToString(),
                Price = Convert.ToDouble(o["Price"].ToString())
            }).ToList();
        }

        public void AddVendor(VendorsModel vendorModel)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();

            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "insert into Vendors (VendorName, Address, City, State, ZipCode, Phone, Website) values (@VendorName, @Address, @City, @State, @ZipCode, @Phone, @WebSite)";

                comm.Parameters.AddWithValue("@VendorName", vendorModel.VendorName);
                comm.Parameters.AddWithValue("@Address", vendorModel.Address);
                comm.Parameters.AddWithValue("@City", vendorModel.City);
                comm.Parameters.AddWithValue("@State", vendorModel.State);
                comm.Parameters.AddWithValue("@ZipCode", vendorModel.ZipCode);
                comm.Parameters.AddWithValue("@Phone", vendorModel.PhoneNumber);
                comm.Parameters.AddWithValue("@Website", vendorModel.Website);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        public void AddVendorProduct(VendorProductsModel vendorProductsModel)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();

           

            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "insert into VendorProducts (VendorId, Description, Price) values (@VendorId, @Description, @Price)";

                comm.Parameters.AddWithValue("@VendorId", vendorProductsModel.VendorId);
                comm.Parameters.AddWithValue("@Description", vendorProductsModel.Description);
                comm.Parameters.AddWithValue("@Price", vendorProductsModel.Price);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        public IEnumerable<PackingSlipModel> GetPackingListInfo(int orderId)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select c.Name, c.Address, c.City, c.State, c.ZipCode, c.PhoneNumber, " +
	            "o.OrderId, o.OrderDate, o.OrderTotal, od.OrderDetailsId, od.Description, od.Quantity, " + 
	            "od.Price, od.LineTotal from customers as c inner join orders as o on c.customerid = o.customerid inner join " +
                "orderdetails as od on o.orderid = od.orderid where o.OrderId = @OrderId";

                comm.Parameters.AddWithValue("@OrderId", orderId);

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(ps => new PackingSlipModel()
            {
                Address = ps["Address"].ToString(),
                City = ps["City"].ToString(),
                Name = ps["Name"].ToString(),
                Description = ps["Description"].ToString(),
                LineNumber = Convert.ToInt32(ps["OrderDetailsId"].ToString()),
                LineTotal = Convert.ToDouble(ps["LineTotal"].ToString()),
                OrderDate = Convert.ToDateTime(ps["OrderDate"].ToString()).ToShortDateString(),
                OrderNumber = ps["OrderId"].ToString(),
                OrderTotal = ps["OrderTotal"].ToString(),
                PhoneNumber = ps["PhoneNumber"].ToString(),
                Price = Convert.ToDouble(ps["Price"].ToString()),
                Quantity = Convert.ToInt32(ps["Quantity"].ToString()),
                State = ps["State"].ToString(),
                ZipCode = ps["ZipCode"].ToString(),
            }).ToList();
        }

        public IEnumerable<InventoryModel> GetInventory()
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select InventoryId, Description, Size, Quantity from Inventory";

                 SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(o => new InventoryModel()
            {
                InventoryId = Convert.ToInt32(o["InventoryId"].ToString()),
                Size = o["Size"].ToString(),
                Description = o["Description"].ToString(),
                Quantity = Convert.ToDouble(o["Quantity"].ToString())
            }).ToList();
        }

        public IEnumerable<InventoryModel> GetInventoryForDropdowns()
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "select InventoryId, Description, Size, Quantity from Inventory";

                SqlCeDataAdapter da = new SqlCeDataAdapter(comm);

                da.Fill(dt);
            }

            return dt.AsEnumerable().Select(o => new InventoryModel()
            {
                InventoryId = Convert.ToInt32(o["InventoryId"].ToString()),
                Description = o["Size"].ToString() + ' ' + o["Description"].ToString()
            }).ToList();
        }

        public void AddInventory(InventoryModel inventoryModel)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();

            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "insert into Inventory (Description, Size, Quantity) values (@Description, @Size, @Quantity)";

                comm.Parameters.AddWithValue("@Description", inventoryModel.Description);
                comm.Parameters.AddWithValue("@Size", inventoryModel.Size);
                comm.Parameters.AddWithValue("@Quantity", inventoryModel.Quantity);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        public void UpdateInventory(InventoryModel inventoryModel)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "UPDATE Inventory SET Description = @Description, Size = @Size, " +
                    "Quantity = @Quantity Where InventoryId = @InventoryId";

                comm.Parameters.AddWithValue("@InventoryId", inventoryModel.InventoryId);
                comm.Parameters.AddWithValue("@Description", inventoryModel.Description);
                comm.Parameters.AddWithValue("@Size", inventoryModel.Size);
                comm.Parameters.AddWithValue("@Quantity", inventoryModel.Quantity);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        public void ShipOrder(int orderId)
        {
            string connectionString = CreateDatabase();
            DataTable dt = new DataTable();

            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = new SqlCeConnection(connectionString);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "update orders set shipped = 1 where orderid = @OrderId";

                comm.Parameters.AddWithValue("@OrderId", orderId);

                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }

        private string CreateDatabase()
        {
            string dbPath = String.Format("{0}IveyWayDesigns.sdf", @"C:\IveyWayDesigns\");

            string connectionString = String.Format("DataSource=\"{0}\";Max Database Size=3000;", dbPath);

            if (!File.Exists(dbPath))
            {
                //File.Delete(dbPath);

                SqlCeEngine en = new SqlCeEngine(connectionString);
                en.CreateDatabase();
                en.Dispose();
            }

            return connectionString;
        }

        private void CreateTable(SqlCeConnection conn)
        {
            using (SqlCeCommand comm = new SqlCeCommand())
            {
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = "CREATE TABLE Orders ([Id] [int] IDENTITY(1,1) PRIMARY KEY, [Name] [nvarchar](110) NOT NULL, [Geometry] [varbinary](429) NOT NULL)";
                comm.ExecuteNonQuery();
            }
        }
    }
}
