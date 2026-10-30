using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Menus;

public class AuditLogMenu
{
    private readonly AuditLogService _auditLogService;

    public AuditLogMenu(AuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("====================================");
            Console.WriteLine("          AUDIT LOG MANAGEMENT");
            Console.WriteLine("====================================");
            Console.WriteLine("1. View All Audit Logs");
            Console.WriteLine("2. Search Audit Log By ID");
            Console.WriteLine("3. View Logs By Entity");
            Console.WriteLine("4. View Logs By Action");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine("====================================");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        ViewAllLogs();
                        break;

                    case "2":
                        SearchLog();
                        break;

                    case "3":
                        ViewLogsByEntity();
                        break;

                    case "4":
                        ViewLogsByAction();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Pause();
            }
        }
    }

    private void ViewAllLogs()
    {
        var logs = _auditLogService.GetAllAuditLogs();

        DisplayLogs(logs);
        Pause();
    }

    private void SearchLog()
    {
        Console.Write("Enter Audit Log ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            Pause();
            return;
        }

        var log = _auditLogService.GetAllAuditLogs()
            .FirstOrDefault(a => a.AuditLogId == id);

        if (log == null)
        {
            Console.WriteLine("No audit log found with this ID.");
        }
        else
        {
            DisplayLog(log);
        }

        Pause();
    }

    private void ViewLogsByEntity()
    {
        Console.Write("Enter Entity Name (Book/Member/Borrowing): ");
        string? entity = Console.ReadLine();

        var logs = _auditLogService.GetAuditLogsByEntity(entity ?? "");

        DisplayLogs(logs);
        Pause();
    }

    private void ViewLogsByAction()
    {
        Console.Write("Enter Action (INSERT/UPDATE/DELETE/BORROW/RETURN): ");
        string? action = Console.ReadLine();

        var logs = _auditLogService.GetAuditLogsByAction(action ?? "");

        DisplayLogs(logs);
        Pause();
    }

    private void DisplayLogs(List<AuditLog> logs)
    {
        if (logs == null || logs.Count == 0)
        {
            Console.WriteLine("\nNo audit logs found.");
            return;
        }

        foreach (var log in logs)
        {
            DisplayLog(log);
            Console.WriteLine("------------------------------------");
        }
    }

    private void DisplayLog(AuditLog log)
    {
        Console.WriteLine($"Audit Log ID: {log.AuditLogId}");
        Console.WriteLine($"Action: {log.Action}");
        Console.WriteLine($"Entity: {log.EntityName}");
        Console.WriteLine($"Entity ID: {log.EntityId}");
        Console.WriteLine($"Description: {log.Description}");
        Console.WriteLine($"Created At: {log.CreatedAt}");
    }

    private void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}