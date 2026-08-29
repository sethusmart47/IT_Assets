using AutoMapper;
using IT_asserts.Dtos.AssetAssignment;
using IT_asserts.Dtos.Employee;
using IT_asserts.entity;
using IT_asserts_Claim.Models;

namespace IT_asserts.Mapping
{
    public class AssetAssignmentProfile : Profile
    {
        public AssetAssignmentProfile()
        {
            // Employee → EmployeeDto
            CreateMap<Employee, EmployeeDto>();

            // AssetAssignment → AssignmentResponseDto
            CreateMap<AssetAssignment, AssignmentResponseDto>()
                .ForMember(d => d.AssetTag, opt => opt.MapFrom(s => s.Asset.AssetTag))
                .ForMember(d => d.SerialNumber, opt => opt.MapFrom(s => s.Asset.SerialNumber))
                .ForMember(d => d.EmployeeName, opt => opt.MapFrom(s => s.Employee.EmployeeName))
                .ForMember(d => d.EmployeeEmail, opt => opt.MapFrom(s => s.Employee.Email));

            // AssetAssignment → ReturnResponseDto
            CreateMap<AssetAssignment, ReturnResponseDto>()
                .ForMember(d => d.AssignmentId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.AssetTag, opt => opt.MapFrom(s => s.Asset.AssetTag))
                .ForMember(d => d.SerialNumber, opt => opt.MapFrom(s => s.Asset.SerialNumber))
                .ForMember(d => d.NewStatus, opt => opt.MapFrom(s => s.Asset.Status.ToString()))
                .ForMember(d => d.ConditionName, opt => opt.MapFrom(s =>
                    s.ConditionAtReturn == 1 ? "Good" :
                    s.ConditionAtReturn == 2 ? "Damaged" :
                    s.ConditionAtReturn == 3 ? "Lost" : "Unknown"))
                .ForMember(d => d.ReturnedDate, opt => opt.MapFrom(s => s.ReturnedDate ?? DateTime.UtcNow));

            // AssetAssignment → EmployeeAssignmentDto (for employee detail page)
            CreateMap<AssetAssignment, EmployeeAssignmentDto>()
                .ForMember(d => d.AssignmentId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.AssetTag, opt => opt.MapFrom(s => s.Asset.AssetTag))
                .ForMember(d => d.SerialNumber, opt => opt.MapFrom(s => s.Asset.SerialNumber))
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Asset.Category))
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Asset.Brand))
                .ForMember(d => d.Model, opt => opt.MapFrom(s => s.Asset.Model))
                .ForMember(d => d.Configuration, opt => opt.MapFrom(s => s.Asset.Configuration))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Asset.Status.ToString()))
                .ForMember(d => d.ConditionAtReturnName, opt => opt.MapFrom(s =>
                    s.ConditionAtReturn == 1 ? "Good" :
                    s.ConditionAtReturn == 2 ? "Damaged" :
                    s.ConditionAtReturn == 3 ? "Lost" : null));
        }
    }
}
