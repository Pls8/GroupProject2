

# 🛠 Home Repair Console App

A simple **C# console application** for managing a home repair service.
It uses **Entity Framework Core** for database operations (Code-First approach).

## Features

* Add, list, and delete **customers**
* Add and delete **technicians**
* Create and manage **repair orders**
* Add and remove **parts from orders**
* Complete orders and generate **invoices**
* Prevent deletion if dependent entities exist (e.g., orders linked to customer)

## Requirements

* .NET 7.0 SDK (or latest)
* SQL Server (localdb works fine)
* Visual Studio or VS Code

## Setup

1. Clone the repository
2. Build the solution
3. Run the program:

   ```bash
   dotnet run
   ```
4. The database will be created automatically using EF Core Code-First (`EnsureCreated()`).

## Usage

When running, the console will display a menu:

```
---------------- Home Repair Console ----------------
1. Add Customer
2. Add Technician
3. Create Repair Order
4. Add Part to Order
5. Complete Order
6. List Orders
7. Delete Customer
8. Delete Technician
9. Delete Repair Order
10. Delete Repair Part
11. Delete Order Part
0. Exit
```

Select the number corresponding to the action you want to perform.

---

## Database Schema

**Entities:**

* CustomerClass
* TechnicianClass
* RepairOrderClass
* RepairPartClass
* OrderPartClass
* InvoiceClass

**Relationships:**

* A `Customer` can have multiple `RepairOrders`
* A `Technician` can be assigned to multiple `RepairOrders`
* A `RepairOrder` can contain multiple `RepairParts` via `OrderPart`
* A `RepairOrder` can generate **one invoice**

---

## File Structure

```
HomeRepairConsole/
│
├─ Program.cs             # Main console application
├─ README.md              # Project documentation
│
├─ Context/
│   └─ AppDbContext.cs    # EF Core DbContext
│
├─ Model/
│   ├─ CustomerClass.cs
│   ├─ TechnicianClass.cs
│   ├─ RepairOrderClass.cs
│   ├─ RepairPartClass.cs
│   ├─ OrderPartClass.cs
│   ├─ InvoiceClass.cs
│   └─ OrderStatusEnums.cs
│
├─ Migrations/            # EF Core migrations (if generated)
│
└─ bin/                   # Build output
```

---

## Notes

* Deleting entities is protected:

  * Customer cannot be deleted if they have repair orders
  * Technician cannot be deleted if assigned to orders
  * RepairOrder cannot be deleted if an invoice exists
  * Parts cannot be deleted if used in any order

* Parts cost and service cost are used to generate **invoice totals**.

---


