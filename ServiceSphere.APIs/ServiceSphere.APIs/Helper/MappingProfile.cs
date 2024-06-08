using AutoMapper;
using Newtonsoft.Json.Linq;
using ServiceSphere.APIs.DTOs;
using ServiceSphere.core.Entities;
using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Posting;
using ServiceSphere.core.Entities.Services;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Entities.Users.Freelancer;

namespace ServiceSphere.APIs.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


            CreateMap<AddressDto, Address>().ReverseMap();
            //CreateMap<ProjectPostingDto,ProjectPosting>().ReverseMap().
            //    ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories))
            //    .ForMember(dest => dest.ProjectSubCategories, opt => opt.MapFrom(src => src.ProjectSubCategories))
            //    ; // Map SubCategories directly;
            CreateMap<SubCategory, SubCategoryDto>().ReverseMap();
            //  CreateMap<ProjectSubCategoryDto, ProjectSubCategory>().ReverseMap().ForMember(p => p.SubCategory, c => c.MapFrom(s => s.SubCategory.Name));
            //  .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            //.ForMember(p=>p.Category,c=>c.MapFrom(s=>s.Category.Name))


            // ProjectPostingDto to ProjectPosting
            CreateMap<ProjectPostingDto, ProjectPosting>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId)) // Map CategoryId directly
                .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.UserId)) // Map UserId to userID
                 ; // Map ProjectSubCategories directly
            CreateMap<ProjectPosting, ProjectPostingDto>();
            CreateMap<ProjectSubCategory, ProjectSubCategoryDto>().ReverseMap();
            CreateMap<ServicePosting, GetServicePostingByIdDto>().ReverseMap();
            CreateMap<ProposalDto, Proposal>().ReverseMap();
            CreateMap<UpdateProposalDto, Proposal>().ReverseMap();
            CreateMap<ServicePosting, ServicePostingDto>().ReverseMap();
            CreateMap<ProjectPosting,GetProjectByIdDto>().ReverseMap();

            CreateMap<ServiceDto, Service>().ReverseMap() ;

            CreateMap<Service, ServiceToReturnDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<FreelancerProfileDto, Freelancer>()
          .ForMember(dest => dest.Categories, opt => opt.Ignore())
           .ForMember(dest => dest.SubCategories, opt => opt.Ignore())// Ignore Categories during AutoMapper mapping
           .ForMember(dest => dest.experienceLevel,
         opt => opt.MapFrom(src => Enum.Parse(typeof(ExperienceLevel), src.experienceLevel)));
            CreateMap<Freelancer, FreelancerProfileToReturnDto>();
            CreateMap<ClientProfileDto, Client>();
            CreateMap<Client, ClientProfileToReturnDto>();

            CreateMap<Freelancer, FreelancerDto>();

            //Contract
            CreateMap<ContractDto, Contract>();

            //Post contract
            CreateMap<PostContract, PostContractDto>();
         

        }



    }

    
}
