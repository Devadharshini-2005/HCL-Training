
using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class MemberService
{
    private readonly MemberRepository _memberRepository;

    
    public MemberService(MemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public void AddMember(Member member)
    {
        ValidateMember(member);

        member.Name = member.Name.Trim();
        member.Email = member.Email.Trim();
        member.PhoneNumber = member.PhoneNumber.Trim();

        bool emailExists = _memberRepository.GetAllMembers()
            .Any(m => string.Equals(
                m.Email,
                member.Email,
                StringComparison.OrdinalIgnoreCase));

        if (emailExists)
        {
            throw new InvalidOperationException(
                "A member with this email already exists.");
        }

        _memberRepository.AddMember(member);
    }

    public List<Member> GetAllMembers()
    {
        return _memberRepository.GetAllMembers();
    }

    public Member GetMemberById(int memberId)
    {
        if (memberId <= 0)
        {
            throw new ArgumentException(
                "Member ID must be greater than zero.");
        }

        var member = _memberRepository.GetMemberById(memberId);

        if (member == null)
        {
            throw new KeyNotFoundException(
                $"Member with ID {memberId} was not found.");
        }

        return member;
    }

    public void UpdateMember(Member member)
    {
        if (member.MemberId <= 0)
        {
            throw new ArgumentException(
                "Member ID must be greater than zero.");
        }

        ValidateMember(member);

        member.Name = member.Name.Trim();
        member.Email = member.Email.Trim();
        member.PhoneNumber = member.PhoneNumber.Trim();

        bool duplicateEmail = _memberRepository.GetAllMembers()
            .Any(m =>
                m.MemberId != member.MemberId &&
                string.Equals(
                    m.Email,
                    member.Email,
                    StringComparison.OrdinalIgnoreCase));

        if (duplicateEmail)
        {
            throw new InvalidOperationException(
                "Another member already uses this email.");
        }

        if (!_memberRepository.UpdateMember(member))
        {
            throw new KeyNotFoundException(
                $"Member with ID {member.MemberId} was not found.");
        }
    }

    public void DeleteMember(int memberId)
    {
        if (memberId <= 0)
        {
            throw new ArgumentException(
                "Member ID must be greater than zero.");
        }

        if (_memberRepository.GetMemberById(memberId) == null)
        {
            throw new KeyNotFoundException(
                $"Member with ID {memberId} was not found.");
        }

        if (!_memberRepository.DeleteMember(memberId))
        {
            throw new InvalidOperationException(
                "Unable to delete the member.");
        }
    }

    private static void ValidateMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);

        if (string.IsNullOrWhiteSpace(member.Name))
        {
            throw new ArgumentException(
                "Member name cannot be empty.");
        }

        if (member.Name.Trim().Length > 100)
        {
            throw new ArgumentException(
                "Member name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(member.Email) ||
            !System.Net.Mail.MailAddress.TryCreate(
                member.Email.Trim(), out var address) ||
            !string.Equals(
                address.Address,
                member.Email.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Please enter a valid email address.");
        }

        if (member.Email.Trim().Length > 150)
        {
            throw new ArgumentException(
                "Email cannot exceed 150 characters.");
        }

        if (string.IsNullOrWhiteSpace(member.PhoneNumber))
        {
            throw new ArgumentException(
                "Phone number cannot be empty.");
        }

        string phone = member.PhoneNumber.Trim();

        if (phone.Length > 15 ||
            phone.Any(c => !char.IsDigit(c) && c != '+' &&
                           c != '-' && c != ' ' && c != '(' && c != ')'))
        {
            throw new ArgumentException(
                "Please enter a valid phone number.");
        }

        if (member.MembershipDate.Date > DateTime.Today)
        {
            throw new ArgumentException(
                "Membership date cannot be in the future.");
        }
    }
}
