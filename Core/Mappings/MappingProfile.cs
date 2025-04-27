using AutoMapper;
using SmartCoins.Core.DTOs;
using SmartCoins.Core.Entities;

namespace SmartCoins.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            //CreateMap<User, UserResponse>()
                //.ForMember(dest => dest.Token, opt => opt.Ignore());

            // Expense mappings
            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category != null
                                                   ? src.Category.Name
                                                   : string.Empty));

            // Category mappings
            //CreateMap<ExpenseCategory, CategoryDto>();

            // Budget mappings
            /*CreateMap<Budget, BudgetDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.Name));*/

            // Tag mappings
            //CreateMap<Tag, TagDto>();

            // SavingsGoal mappings
            //CreateMap<SavingsGoal, SavingsGoalDto>();
        }
    }
}