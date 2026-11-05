
using CampusPlacementManagement.DTOs.Students;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentResponseDto>> GetAllAsync();

    Task<StudentResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<StudentResponseDto>> GetByCollegeIdAsync(
        int collegeId);

    Task<IEnumerable<StudentResponseDto>> GetPendingAsync(
        int collegeId);

    Task<StudentResponseDto> EnrollAsync(
        CreateStudentRequestDto request,
        string placementOfficerUserId);

    Task<bool> ApproveAsync(
        int id,
        string placementOfficerUserId);

    Task<bool> RejectAsync(
        int id,
        string placementOfficerUserId);
    Task<IEnumerable<StudentResponseDto>> GetPendingStudentsForOfficerAsync(
    string placementOfficerUserId);

}
