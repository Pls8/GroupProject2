using GroupProject2.Context;
using GroupProject2.Model;
using Microsoft.EntityFrameworkCore;

namespace GroupProject2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("---------------- Home Repair Console ----------------");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Add Technician");
                Console.WriteLine("3. Create Repair Order");
                Console.WriteLine("4. Add Part to Order");
                Console.WriteLine("5. Complete Order");
                Console.WriteLine("6. List Orders");
                Console.WriteLine("7. Delete Customer");
                Console.WriteLine("8. Delete Technician");
                Console.WriteLine("9. Delete Repair Order");
                Console.WriteLine("10. Delete Repair Part");
                Console.WriteLine("11. Delete Order Part");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1": AddCustomer(); break;
                    case "2": AddTechnician(); break;
                    case "3": CreateOrder(); break;
                    case "4": AddPartToOrder(); break;
                    case "5": CompleteOrder(); break;
                    case "6": ListOrders(); break;

                    case "7": DeleteCustomer(); break;
                    case "8": DeleteTechnician(); break;
                    case "9": DeleteRepairOrder(); break;
                    case "10": DeleteRepairPart(); break;
                    case "11": DeleteOrderPart(); break;

                    case "0": return;

                    default:
                        Console.WriteLine("Invalid option. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }


        static void AddCustomer()
        {
            using var db = new AppDbContext();
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Phone: "); string phone = Console.ReadLine();
            Console.Write("Address: "); string address = Console.ReadLine();

            db.Customers.Add(new CustomerClass { Name = name, Phone = phone, Address = address });
            db.SaveChanges();
            Console.WriteLine("Customer Added"); Console.ReadKey();
        }

        static void AddTechnician()
        {
            using var db = new AppDbContext();
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Phone: "); string phone = Console.ReadLine();
            Console.Write("Specialty: "); string sp = Console.ReadLine();

            db.Technicians.Add(new TechnicianClass { Name = name, Phone = phone, Specialty = sp });
            db.SaveChanges();
            Console.WriteLine("Technician Added"); Console.ReadKey();
        }

        static void CreateOrder()
        {
            using var db = new AppDbContext();
            Console.Write("Customer ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Appliance Type: ");
            string type = Console.ReadLine();

            Console.Write("Problem: ");
            string prob = Console.ReadLine();

            db.RepairOrders.Add(new RepairOrderClass
            {
                CustomerId = id,
                ApplianceType = type,
                ProblemDescription = prob
            });

            db.SaveChanges();
            Console.WriteLine("Order Created"); Console.ReadKey();
        }

        static void AddPartToOrder()
        {
            using var db = new AppDbContext();

            Console.Write("Order ID: ");
            int oid = int.Parse(Console.ReadLine());

            Console.Write("Part Name: ");
            string name = Console.ReadLine();

            Console.Write("Unit Price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Quantity: ");
            int qty = int.Parse(Console.ReadLine());

            var part = new RepairPartClass { PartName = name, UnitPrice = price };
            db.RepairParts.Add(part);
            db.SaveChanges();

            db.OrderParts.Add(new OrderPartClass
            {
                RepairOrderId = oid,
                PartId = part.PartId,
                Quantity = qty
            });

            db.SaveChanges();

            Console.WriteLine("Part Added to Order"); Console.ReadKey();
        }

        static void CompleteOrder()
        {
            using var db = new AppDbContext();

            Console.Write("Order ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Service Cost: ");
            decimal svc = decimal.Parse(Console.ReadLine());

            // Calculate parts cost
            var parts = db.OrderParts.Include(p => p.RepairPart)
                .Where(p => p.RepairOrderId == id).ToList();

            decimal partsTotal = parts.Sum(p => p.RepairPart.UnitPrice * p.Quantity);

            db.Invoices.Add(new InvoiceClass
            {
                RepairOrderId = id,
                ServiceCost = svc,
                PartsCost = partsTotal
            });

            var order = db.RepairOrders.First(o => o.RepairOrderId == id);
            order.Status = OrderStatusEnums.Completed;

            db.SaveChanges();

            Console.WriteLine("Order Completed + Invoice Generated");
            Console.ReadKey();
        }

        static void ListOrders()
        {
            using var db = new AppDbContext();
            var orders = db.RepairOrders.Include(c => c.Customer);

            foreach (var o in orders)
                Console.WriteLine($"{o.RepairOrderId} - {o.ApplianceType} - {o.Status} - {o.Customer.Name}");

            Console.ReadKey();
        }





        //Delete Method
        static void DeleteCustomer()
        {
            using var db = new AppDbContext();

            Console.Write("Enter Customer ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            var customer = db.Customers
                .Where(c => c.CustomerId == id)
                .FirstOrDefault();

            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                Console.ReadKey();
                return;
            }

            // Check if customer has repair orders
            bool hasOrders = db.RepairOrders.Any(r => r.CustomerId == id);

            if (hasOrders)
            {
                Console.WriteLine("Cannot delete customer with existing repair orders.");
                Console.ReadKey();
                return;
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            Console.WriteLine("Customer deleted successfully.");
            Console.ReadKey();
        }




        static void DeleteTechnician()
        {
            using var db = new AppDbContext();

            Console.Write("Enter Technician ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            var tech = db.Technicians.FirstOrDefault(t => t.TechnicianId == id);

            if (tech == null)
            {
                Console.WriteLine("Technician not found.");
                Console.ReadKey();
                return;
            }

            // Check if technician has orders
            bool hasOrders = db.RepairOrders.Any(r => r.TechnicianId == id);

            if (hasOrders)
            {
                Console.WriteLine("Cannot delete technician with existing repair orders.");
                Console.ReadKey();
                return;
            }

            db.Technicians.Remove(tech);
            db.SaveChanges();

            Console.WriteLine("Technician deleted successfully.");
            Console.ReadKey();
        }



        static void DeleteRepairOrder()
        {
            using var db = new AppDbContext();

            Console.Write("Enter Repair Order ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            var order = db.RepairOrders
                .FirstOrDefault(o => o.RepairOrderId == id);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                Console.ReadKey();
                return;
            }

            // Check invoice
            if (db.Invoices.Any(i => i.RepairOrderId == id))
            {
                Console.WriteLine("Cannot delete an order with an existing invoice.");
                Console.ReadKey();
                return;
            }

            db.RepairOrders.Remove(order);
            db.SaveChanges();

            Console.WriteLine("Order deleted successfully.");
            Console.ReadKey();
        }


        public static void DeleteRepairPart()
        {
            using var db = new AppDbContext();

            Console.Write("Enter Part ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            var part = db.RepairParts.FirstOrDefault(p => p.PartId == id);

            if (part == null)
            {
                Console.WriteLine("Part not found.");
                Console.ReadKey();
                return;
            }

            // Check if part is used in orders
            bool isUsed = db.OrderParts.Any(op => op.PartId == id);

            if (isUsed)
            {
                Console.WriteLine("Cannot delete part used in an order.");
                Console.ReadKey();
                return;
            }

            db.RepairParts.Remove(part);
            db.SaveChanges();

            Console.WriteLine("Part deleted successfully.");
            Console.ReadKey();
        }



        public static void DeleteOrderPart()
        {
            using var db = new AppDbContext();

            Console.Write("OrderPart ID: ");
            int id = int.Parse(Console.ReadLine());

            var op = db.OrderParts.FirstOrDefault(o => o.OrderPartId == id);

            if (op == null)
            {
                Console.WriteLine("OrderPart not found.");
                Console.ReadKey();
                return;
            }

            db.OrderParts.Remove(op);
            db.SaveChanges();

            Console.WriteLine("OrderPart deleted successfully.");
            Console.ReadKey();
        }




    }
}
