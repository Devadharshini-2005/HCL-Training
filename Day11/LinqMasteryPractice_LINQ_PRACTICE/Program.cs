using LinqMastery.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;

namespace LinqMastery;


class NotFoundException : Exception
{
    public NotFoundException(string msg) : base(msg)
    {
        
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("LINQ Mastery Practice Project");

        //Exercise01_Filtering();
        //Exercise02_Projection();
        //Exercise03_SelectMany();
        //Exercise04_Sorting();
        Exercise05_ElementOperators();
        //Exercise06_Aggregation();
        //Exercise07_AnyAll();
        //Exercise08_Grouping();
        //Exercise09_SetOperations();
        //Exercise10_InnerJoin();
        //Exercise11_LeftJoin();
        //Exercise12_GroupJoin();
        //Exercise13_Partitioning();
        //Exercise14_QuantifiersAndMembership();
        //Exercise15_StringAndNumericQueries();
        //Exercise16_NestedQueries();
        //Exercise17_CombinedQueries();
        // Exercise18_DictionaryAndLookup();
        //Exercise19_JoinOrdersAndCustomers();
        //Exercise20_Reports();
    }

    // RULE: For every task, implement both styles where supported.
    // Use these labels to keep your work organised:
    // METHOD SYNTAX PRACTICE
    // QUERY SYNTAX PRACTICE

    static void Exercise01_Filtering()
    {
        var products = StoreData.Products;


        // Task 1: Find products priced above 5,000.
        // METHOD SYNTAX PRACTICE
        // TODO: products.Where(...)

        //Console.WriteLine("-----Method Syntax------");

        //var methodSyntax = products.Where(p => p.Price > 5000);

        //foreach (var ms in methodSyntax)
        //{
        //    Console.WriteLine($"Product name : {ms.ProductName}");
        //}

        // QUERY SYNTAX PRACTICE
        // TODO: from p in products where ... select p

        //Console.WriteLine("-----Query Syntax------");

        //var querySyntax = from p in products where p.Price > 5000 select p;

        //foreach (var qs in querySyntax)
        //{
        //    Console.WriteLine($"Product name : {qs.ProductName}");
        //}

        // Task 2: Find Electronics products that are in stock.
        // METHOD SYNTAX PRACTICE
        // TODO: Where with &&

        //Console.WriteLine("-----Method Syntax------");

        //var mcategory = products.Where(p => p.Category == "Electronics" && p.Stock > 0);
        
        //foreach (var c in mcategory)
        //{
        //    Console.WriteLine($"Product name : {c.ProductName}     "+
        //        $"Category : {c.Category}    "+
        //        $"Stock : {c.Stock}");
        //}

        // QUERY SYNTAX PRACTICE
        // TODO: where with &&

        //Console.WriteLine("-----Query Syntax------");

        //var qcategory = from p in products where p.Category == "Electronics" && p.Stock > 0 select p;

        //foreach (var q in mcategory)
        //{
        //    Console.WriteLine($"Product name : {q.ProductName}     " +
        //        $"Category : {q.Category}    " +
        //        $"Stock : {q.Stock}");
        //}

        // Task 3: Find products priced between 1,000 and 20,000.
        // METHOD SYNTAX PRACTICE
        // TODO

        //Console.WriteLine("------Method Syntax-------");

        //var mproduct = products.Where(p => p.Price > 1000 && p.Price < 20000);

        //foreach (var m in mproduct)
        //{
        //    Console.WriteLine($"Product name : {m.ProductName}     " +
        //        $"Category : {m.Category}    " +
        //        $"Stock : {m.Price}");
        //}


        // QUERY SYNTAX PRACTICE
        // TODO

        //Console.WriteLine("------Query Syntax-------");

        //var qproduct = from p in products where p.Price>1000 && p.Price<20000 select p;

        //foreach (var q in qproduct)
        //{
        //    Console.WriteLine($"Product name : {q.ProductName}     " +
        //        $"Category : {q.Category}    " +
        //        $"Stock : {q.Price}");
        //}


        // Task 4: Find products whose names contain "Office".
        // METHOD SYNTAX PRACTICE
        // TODO: Contains()

        //Console.WriteLine("------Method Syntax-------");

        //var mname = products.Where(p => p.ProductName.Contains("Office"));

        //foreach (var m in mname)
        //{
        //    Console.WriteLine($"Product name : {m.ProductName}     " +
        //        $"Category : {m.Category}    " +
        //        $"Stock : {m.Price}");
        //}


        // QUERY SYNTAX PRACTICE
        // TODO

        //Console.WriteLine("------Query Syntax-----");

        //var qname = from p in products where p.ProductName.Contains("Office") select p;

        //foreach (var q in qname)
        //{
        //    Console.WriteLine($"Product name : {q.ProductName}     " +
        //        $"Category : {q.Category}    " +
        //        $"Stock : {q.Price}");
        //}
    }

    static void Exercise02_Projection()
    {
        var products = StoreData.Products;

        // Task 1: Select only product names.
        // METHOD SYNTAX PRACTICE
        // TODO: Select(...)

        //var mSelectname = products.Select(p=>p.ProductName);

        //foreach(var m in mSelectname)
        //{
        //    Console.WriteLine($"Product Name : {m}");
        //}

        // QUERY SYNTAX PRACTICE
        // TODO: select p.ProductName

        //Console.WriteLine("------Query Syntax-------");

        //var qSelectname = from p in products select p.ProductName;

        //foreach (var m in qSelectname)
        //{
        //    Console.WriteLine($"Product Name : {m}");
        //}

        // Task 2: Return ProductName and Price using an anonymous type.
        // METHOD SYNTAX PRACTICE
        // TODO: Select(p => new { ... })

        //Console.WriteLine("-----Method Syntax-----");

        //var mtype = products.Select(p => new {p.ProductName, p.Price});

        //foreach (var m in mtype)
        //{
        //    Console.WriteLine($"Product Name : {m.ProductName}"+
        //        $"Price : {m.Price}+");
        //}

        // QUERY SYNTAX PRACTICE
        // TODO: select new { ... }

        //Console.WriteLine("-----Query Syntax----");

        //var qtype = from p in products select  new { p.ProductName, p.Price };

        //foreach (var q in qtype)
        //{
        //    Console.WriteLine($"Product Name : {q.ProductName}" +
        //        $"Price : {q.Price}+");
        //}

        // Task 3: Return ProductName, Price, and PriceWithTax (18%).
        // METHOD SYNTAX PRACTICE
        // TODO

        //Console.WriteLine("----Method Syntax----");

        //var mtax = products.Select(p => new
        //{
        //    p.ProductName,
        //    p.Price,
        //    tax = (p.Price * (18/100)) + p.Price
        //});

        //foreach (var m in mtax)
        //{
        //    Console.WriteLine($"Product Name : {m.ProductName}          " +
        //        $"Price : {m.Price}      "+
        //        $"Tax : {m.tax}");
        //}

        // QUERY SYNTAX PRACTICE
        // TODO

    //    Console.WriteLine("------Query Syntax----");

    //    var qtax = from p in products
    //               select new
    //               {
    //                   p.ProductName,
    //                   p.Price,
    //                   tax = (p.Price * (18 / 100)) + p.Price
    //               };

    //    foreach (var q in qtax)
    //    {
    //        Console.WriteLine($"Product Name : {q.ProductName}           " +
    //            $"Price : {q.Price}     "+
    //            $"Tax : {q.tax}");
    //    }
    }

    static void Exercise03_SelectMany()
    {
        var products = StoreData.Products;
        var batches = StoreData.TrainingBatches;

        // Task 1: Flatten all training batches into one sequence.
        // METHOD SYNTAX PRACTICE
        // TODO: SelectMany()

        Console.WriteLine("------Method Syntax - SelectMany------");

        var mselectmany = batches.SelectMany(group => group);

        foreach (var m in mselectmany)
        {
            Console.WriteLine(m);
        }

        // QUERY SYNTAX PRACTICE
        // TODO: use two from clauses



        // Task 2: Get every product tag as one flat sequence.
        // METHOD SYNTAX PRACTICE
        // TODO

        Console.WriteLine("----Query Syntax - SelectMany-----");

        var mproduct = products.SelectMany(p => p.Tags);

        foreach (var m in mproduct)
        {
            Console.WriteLine(m);
        }

        // QUERY SYNTAX PRACTICE
        // TODO
    }

    static void Exercise04_Sorting()
    {
        var products = StoreData.Products;

        // Task 1: Sort by price ascending.
        // METHOD SYNTAX PRACTICE
        // TODO: OrderBy()

        Console.WriteLine("----Order By---");

        var masecprice = products.OrderBy(p => p.Price);

        foreach (var p in masecprice)
        {
            Console.WriteLine($"Product Name : {p.ProductName}" +
                $"Price : {p.Price}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO: orderby

        // Task 2: Sort by price descending.
        // METHOD SYNTAX PRACTICE
        // TODO: OrderByDescending()

        Console.WriteLine("------OrderByDescending-------");

        var mdecsprice = products.OrderByDescending(p => p.Price);

        foreach (var m in mdecsprice)
        {
            Console.WriteLine($"Product name : {m.ProductName}       " +
                $"Price : {m.Price}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO: orderby ... descending

        // Task 3: Sort by Category ascending, then Price descending.
        // METHOD SYNTAX PRACTICE
        // TODO: OrderBy().ThenByDescending()

        Console.WriteLine("-----ThenByDescending-----");

        var mthenby = products.OrderBy(p => p.Category).ThenByDescending(p => p.Price);

        foreach(var p in mthenby)
        {
            Console.WriteLine($"Product name : {p.ProductName}"+
                $"Price : {p.Price}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO
    }

    static void Exercise05_ElementOperators()
    {
        var products = StoreData.Products;

        // Task 1: Find the first product priced above 10,000.
        // METHOD SYNTAX PRACTICE
        // TODO: First()

        Console.WriteLine("---Method Synatx - Frist");

        var mfirst = products.First(p => p.Price > 10000);

        Console.WriteLine($"Product name : {mfirst.ProductName}     "+
            $"Price : {mfirst.Price}");

        // QUERY SYNTAX PRACTICE
        // TODO: query + First()

        // Task 2: Search for ProductId 999 safely.
        // METHOD SYNTAX PRACTICE
        // TODO: FirstOrDefault()

        Console.WriteLine("---Method Synatx - FristOrDefault----");

        try
        {
            var mfirstordefault = products.FirstOrDefault(p => p.ProductId == 999);

            if(mfirstordefault == null)
            {
                throw new NotFoundException("Record is not Found!!");
            }

            Console.WriteLine($"Product Id : {mfirstordefault.ProductId}   " + $"Product name : {mfirst.ProductName}     " +
                $",  Price : {mfirst.Price}");
        }
        catch(NotFoundException msg)
        {
            Console.WriteLine(msg.Message);
        }


        // QUERY SYNTAX PRACTICE
        // TODO: query + FirstOrDefault()

        // Task 3: Find ProductId 3 using Single().
        // METHOD SYNTAX PRACTICE
        // TODO: Single()

        Console.WriteLine("---Method Syntax - Single------");

        var msingle = products.Single(p => p.ProductId == 3);

        Console.WriteLine( $"Product Id : {msingle.ProductId}"+ $"Product name : {msingle.ProductName}     " +
            $",  Price : {msingle.Price}");

        // QUERY SYNTAX PRACTICE
        // TODO: query + Single()
    }

    static void Exercise06_Aggregation()
    {
        var products = StoreData.Products;

        // Task 1: Count all products.
        // METHOD SYNTAX PRACTICE
        // TODO: Count()

        Console.WriteLine("----Method Synatx - Count----");

        var mcount = products.Count();

        Console.WriteLine(mcount);


        // QUERY SYNTAX PRACTICE
        // TODO: query + Count()


        // Task 2: Calculate total stock.
        // METHOD SYNTAX PRACTICE
        // TODO: Sum()

        Console.WriteLine("----Method Synatx - Sum----");

        var msum = products.Sum(p => p.Stock);

        Console.WriteLine(msum);


        // QUERY SYNTAX PRACTICE
        // TODO: query + Sum()

        // Task 3: Find average, minimum and maximum price.
        // METHOD SYNTAX PRACTICE
        // TODO

        Console.WriteLine("----Method Synatx - Average----");

        var mavg = products.Average(p => p.Price);

        Console.WriteLine(mavg);


        Console.WriteLine("----Method Synatx - Minimun----");

        var mmin = products.Min(p => p.Price);

        Console.WriteLine(mmin);

        Console.WriteLine("----Method Synatx - Maximum----");

        var mmax = products.Max(p => p.Price);

        Console.WriteLine(mmax);

        // QUERY SYNTAX PRACTICE
        // TODO

        // Task 4: Calculate total inventory value (Price * Stock).
        // METHOD SYNTAX PRACTICE
        // TODO

        Console.WriteLine("----Method Synatx - Inventory value----");

        var mivalue = products.Sum(p => p.Price * p.Stock);

        Console.WriteLine(mivalue);

        // QUERY SYNTAX PRACTICE
        // TODO
    }

    static void Exercise07_AnyAll()
    {
        var products = StoreData.Products;

        // Task 1: Check whether any product costs more than 50,000.
        // METHOD SYNTAX PRACTICE
        // TODO: Any()

        Console.WriteLine("------Method Syntax - Any()");

        var many = products.Any(p => p.Price > 50000);

        Console.WriteLine(many);


        // QUERY SYNTAX PRACTICE
        // TODO: query + Any()

        // Task 2: Check whether all products have non-negative stock.
        // METHOD SYNTAX PRACTICE
        // TODO: All()

        Console.WriteLine("------Method Syntax - All()");

        var mall = products.All(p => p.Stock < 0);

        Console.WriteLine(mall);

        // QUERY SYNTAX PRACTICE
        // TODO: query + All()
    }

    static void Exercise08_Grouping()
    {
        var products = StoreData.Products;

        // Task 1: Group products by Category.
        // METHOD SYNTAX PRACTICE     
        // TODO: GroupBy()

        Console.WriteLine("----Method Syntax - GroupBy()----");

        var mgroupby = products.GroupBy(p => p.Category);

        foreach (var m in mgroupby)
        {
            foreach(var o in m)
            {
                Console.WriteLine($"Product name : {o.ProductName}     " +
                    $"Category : {o.Category}");
            }
            Console.WriteLine();
        }

        // QUERY SYNTAX PRACTICE
        // TODO: group ... by ...

        // Task 2: For each category, calculate product count,
        // total stock, and average price.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupBy + Select

        Console.WriteLine("----Method Syntax - Groupby + Select-----");

        var mgselect = products.GroupBy(p => p.Category).Select(p => new
        {
            count = p.Count(),
            total = p.Sum(o => o.Stock),
            average = p.Average(o => o.Price)
        });

        foreach(var s in mgselect)
        {
                Console.WriteLine($"Count : {s.count}    " +
                    $"Total : {s.total}    " +
                    $"Average : {s.average}");
       
        }

        // QUERY SYNTAX PRACTICE
        // TODO: group ... by ... into ...
    }

    static void Exercise09_SetOperations()
    {
        var a = StoreData.ListA;
        var b = StoreData.ListB;

        // Task 1: Remove duplicates.
        // METHOD SYNTAX PRACTICE
        // TODO: Distinct()

        Console.WriteLine("-----Method Syntax - Distinct()-----");

        var mdistinct1 = a.Distinct();

        foreach(var m in mdistinct1)
        {
            Console.WriteLine(m); 
        }

        var mdistinct2 = b.Distinct();

        foreach (var m in mdistinct2)
        {
            Console.WriteLine(m);
        }

        // QUERY SYNTAX PRACTICE
        // TODO: query + Distinct()

        // Task 2: Find union, intersection and A-only values.
        // METHOD SYNTAX PRACTICE
        // TODO: Union(), Intersect(), Except()

        Console.WriteLine("----Method Syntax - Union----");

        var munion = a.Union(b);

        foreach(var u in munion)
        {
            Console.WriteLine(u);
        }

        Console.WriteLine("----Method Syntax - Intersect----");

        var mintersect = a.Intersect(b);

        foreach (var i in mintersect)
        {
            Console.WriteLine(i);
        }

        Console.WriteLine("----Method Syntax - Except----");

        var mexcept = a.Except(b);

        foreach (var e in mexcept)
        {
            Console.WriteLine(e);
        }

        // QUERY SYNTAX PRACTICE
        // TODO: query results + set operators
    }

    static void Exercise10_InnerJoin()
    {
        var employees = StoreData.Employees;
        var departments = StoreData.Departments;

        // Task: Display EmployeeName and DepartmentName for matching records.
        // METHOD SYNTAX PRACTICE
        // TODO: Join()

        Console.WriteLine("----Method Syntax - Join----");

        var mjoin = employees.Join(
            departments,
            emp => emp.DepartmentId,
            dept => dept.DepartmentId,
            (emp,dept) => new
            {
                employeeName = emp.EmployeeName,
                departmentName = dept.DepartmentName
            }
            );

        foreach (var j in mjoin)
        {
            Console.WriteLine($"Employee Name : {j.employeeName}" +
                   $"Department Name : {j.departmentName}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO: join ... on ... equals ...
    }

    static void Exercise11_LeftJoin()
    {
        var employees = StoreData.Employees;
        var departments = StoreData.Departments;

        // Task: Return every employee, displaying "Unassigned" if
        // no matching department exists.
        // METHOD SYNTAX PRACTICE
        // TODO: LeftJoin() on .NET 10, or GroupJoin + SelectMany

        Console.WriteLine("----Method Syntax - LeftJoin----");

        var mleftjoin = employees.LeftJoin(
            departments,
            emp => emp.DepartmentId,
            dept => dept.DepartmentId,
            (emp, dept) => new
            {
                employeeName = emp.EmployeeName,
                departmentName = dept == null ? "Unsigned" : dept.DepartmentName
            }
            );

        foreach(var j in mleftjoin)
        {
            Console.WriteLine($"Employee Name : {j.employeeName}" +
                   $"Department Name : {j.departmentName}");
        }


        // QUERY SYNTAX PRACTICE
        // TODO: join ... into ... then DefaultIfEmpty()
    }

    static void Exercise12_GroupJoin()
    {
        var employees = StoreData.Employees;
        var departments = StoreData.Departments;

        // Task: Return each department with its employee count.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupJoin()

        Console.WriteLine("-----Method Syntax - Group Join-----");

        var mgroupjoin = departments.GroupJoin(
            employees,
            dept => dept.DepartmentId,
            e => e.DepartmentId,
            (dept, matches) => new { departmentName = dept.DepartmentName, count = matches.Count() }
            );


        foreach (var j in mgroupjoin)
        {
            Console.WriteLine($"Employee Name : {j.departmentName}" +
                   $"Department Name : {j.count}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO: join ... into ...
    }

    static void Exercise13_Partitioning()
    {
        var products = StoreData.Products.OrderBy(p => p.ProductId);

        // Task 1: Get first three products.
        // METHOD SYNTAX PRACTICE
        // TODO: Take(3)

        Console.WriteLine("----Method Syntax - Take----");

        var mfirstthree = products.Take(3);

        foreach (var f in mfirstthree)
        {
            Console.WriteLine($"Product Name : {f.ProductName}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO: query + Take(3)

        // Task 2: Skip first three products.
        // METHOD SYNTAX PRACTICE
        // TODO: Skip(3)

        Console.WriteLine("---Method Syntax - Skip----");

        var mskipthree = products.Skip(3);

        foreach (var f in mskipthree)
        {
            Console.WriteLine($"Product Name : {f.ProductName}");
        }


        // QUERY SYNTAX PRACTICE
        // TODO: query + Skip(3)
    }

    static void Exercise14_QuantifiersAndMembership()
    {

        var products = StoreData.Products;

        List<int> requestedIds = new() { 1, 3, 5 };

        // Task: Find products whose IDs occur in requestedIds.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + Contains

        Console.WriteLine("----Method Query - Where + Contains----");

        var mwhere = products.Where( p => requestedIds.Contains(p.ProductId));

        foreach (var m in mwhere)
        {
            Console.WriteLine($"Product Id : {m.ProductId}   " +
                $"ProductName : {m.ProductName}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO: where + Contains
    }

    static void Exercise15_StringAndNumericQueries()
    {
        var products = StoreData.Products;

        // Task 1: Find names starting with "O".
        // METHOD SYNTAX PRACTICE
        // TODO: StartsWith()

        Console.WriteLine("-----Method Syntax - StartsWith-----");

        var mstartswith = products.Where(p => p.ProductName.StartsWith("O"));
        foreach (var m in mstartswith)
        {
            Console.WriteLine($"Product Name : {m.ProductName}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO

        // Task 2: Find products with even ProductIds.
        // METHOD SYNTAX PRACTICE
        // TODO: modulo operator %

        Console.WriteLine("-----Method Syntax - Even Product Id-----");

        var mmodulo = products.Where(p => p.ProductId % 2==0);
        foreach (var m in mmodulo)
        {
            Console.WriteLine($"Product Id : {m.ProductId}" 
                + $"Product Name : {m.ProductName}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO
    }

    static void Exercise16_NestedQueries()
    {
        var products = StoreData.Products;
        var orders = StoreData.Orders;

        // Task 1: Find products priced above average.
        // METHOD SYNTAX PRACTICE
        // TODO: Average + Where


        Console.WriteLine("----Method Syntax - Average + Where----");

        var avgPrice = products.Average(p => p.Price);
        var mavg = products.Where(p => p.Price > avgPrice);

        foreach (var m in mavg)
        {
            Console.WriteLine($"Product Name : {m.ProductName}" +
                $"Price : {m.Price}");
        }

        // QUERY SYNTAX PRACTICE
        // TODO

        // Task 2: Find customers with at least one order.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + Any

        Console.WriteLine("----Method Syntax - Any + Where----");

        

        // QUERY SYNTAX PRACTICE
        // TODO
    }

    static void Exercise17_CombinedQueries()
    {
        var products = StoreData.Products;

        // Task: Find in-stock Electronics products, sort by descending
        // price, and return only product names.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + OrderByDescending + Select

        Console.WriteLine("---Method Syntax - Where, OrderByDescending, Select---");

        var mwhere = products.Where(p => p.Category == "Electronics").OrderByDescending(p => p.Price).Select(p => p.ProductName);

        foreach (var m in mwhere) 
        {
            Console.WriteLine($"Product Name : {m}");
        }



        // QUERY SYNTAX PRACTICE
        // TODO: from + where + orderby + select
    }

    static void Exercise18_DictionaryAndLookup()
    {
        var products = StoreData.Products;

        // Task 1: Create a dictionary keyed by ProductId.
        // METHOD SYNTAX PRACTICE
        // TODO: ToDictionary()

        Console.WriteLine("---Method Syntax - ToDictionary---");

        var mtodict = products.ToDictionary( p => p.ProductId , p => p);

        foreach (var m in mtodict)
        {
            Console.WriteLine($"Key : {m.Key} " +
                $"Value : {m.Value}");
        }


        // QUERY SYNTAX PRACTICE
        // TODO: query + ToDictionary()

        // Task 2: Create a lookup by Category.
        // METHOD SYNTAX PRACTICE
        // TODO: ToLookup()

        Console.WriteLine("---Method Query - ToLooup---");

        var mtolookup = products.ToLookup(p => p.Category);

        foreach (var m in mtolookup)
        {
            Console.WriteLine($"Product Id : {m.Key}");

            foreach (var i in m)
            {
                Console.WriteLine($"Product name : {i.ProductName}");
            }
        }

        // QUERY SYNTAX PRACTICE
        // TODO: query + ToLookup()
    }

    static void Exercise19_JoinOrdersAndCustomers()
    {
        var orders = StoreData.Orders;
        var customers = StoreData.Customers;

        // Task: Show OrderId, CustomerName, OrderDate and Status.
        // METHOD SYNTAX PRACTICE
        // TODO: Join + Select

        Console.WriteLine("---Method syntax - Join, Select---");

        var mjoin = customers.Join(
            orders,
            cust => cust.CustomerId,
            order => order.CustomerId,
            (cust, order) => new
            {
                orderId = order.OrderId,
                customerName = cust.CustomerName,
                orderDate = order.OrderDate,
                status = order.Status
            }
            ).Select(p => p);


        foreach (var m in mjoin)
        {
            Console.WriteLine($"OrderID : {m.orderId}    " +
                $"Customer name : {m.customerName}      " +
                $"Order Date : {m.orderDate}    " +
                $"Stauts : {m.status}");

        }


        // QUERY SYNTAX PRACTICE
        // TODO: join + select
    }

    static void Exercise20_Reports()
    {
        var products = StoreData.Products;
        var orders = StoreData.Orders;

        // Task 1: Count orders by Status.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupBy + Count

        Console.WriteLine("---Method Syntax - GroupBy, Count---");

        var mgroupby = orders.GroupBy(p => p.Status).Count();

        Console.WriteLine($"Count : {mgroupby}");

        // QUERY SYNTAX PRACTICE
        // TODO: group by + Count

        // Task 2: Create a category report with product count,
        // average price, total stock and inventory value.
        // METHOD SYNTAX PRACTICE
        // TODO



        // QUERY SYNTAX PRACTICE
        // TODO
    }
}
