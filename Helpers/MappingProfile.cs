using AutoMapper;
using VotingSystem.API.DataTransferObjects.Request;
using VotingSystem.API.DataTransferObjects.Response;
using VotingSystem.API.Models;

namespace VotingSystem.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Requests
            // Map Dto to Model
            
            CreateMap<SignInDto, SystemUser>();
            CreateMap<CreateVoterDto, Voter>();
            CreateMap<CreateCandidateDto, Candidate>();
            CreateMap<CreateElectionDto, Election>();
            CreateMap<UpdateUserDto, Voter>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateCandidateDto, Candidate>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateElectionDto, Election>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Responses
            // Map Model to Dto

            CreateMap<SystemUser, UserResponseDto>();
            CreateMap<Voter, UserResponseDto>();
            CreateMap<Candidate, CandidateResponseDto>();
            CreateMap<Election, ElectionResponseDto>();

        }
    }
}