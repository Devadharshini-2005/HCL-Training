
using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class MemberRepository
{
    private readonly AppDbContext _context;

    public MemberRepository(AppDbContext context)
    {
        _context = context;
    }
    public void AddMember(Member member)
    {
        using var context = new AppDbContext();

        context.Members.Add(member);
        context.SaveChanges();
    }

    public List<Member> GetAllMembers()
    {
        using var context = new AppDbContext();

        return context.Members
            .AsNoTracking()
            .OrderBy(m => m.MemberId)
            .ToList();
    }

    public Member? GetMemberById(int memberId)
    {
        using var context = new AppDbContext();

        return context.Members
            .AsNoTracking()
            .FirstOrDefault(m => m.MemberId == memberId);
    }

    public bool UpdateMember(Member updatedMember)
    {
        using var context = new AppDbContext();

        var existingMember = context.Members
            .FirstOrDefault(m => m.MemberId == updatedMember.MemberId);

        if (existingMember == null)
        {
            return false;
        }

        existingMember.Name = updatedMember.Name;
        existingMember.Email = updatedMember.Email;
        existingMember.PhoneNumber = updatedMember.PhoneNumber;
        existingMember.MembershipDate = updatedMember.MembershipDate;

        context.SaveChanges();

        return true;
    }

    public bool DeleteMember(int memberId)
    {
        using var context = new AppDbContext();

        var member = context.Members
            .FirstOrDefault(m => m.MemberId == memberId);

        if (member == null)
        {
            return false;
        }

        context.Members.Remove(member);
        context.SaveChanges();

        return true;
    }
}
