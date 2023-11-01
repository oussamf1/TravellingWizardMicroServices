using UserOperationsMicroService.DTOs;

namespace UserOperationsMicroService.Services.Interface
{
    public interface IUserActionsService
    {
        public Task<UserRegistrationResultDTO> RegisterUser(UserRegistrationDTO user);

        public Task<UserAuthResultDTO> Login(UserAuthenticationDTO userAuth);
        public Task<bool> ConfirmUserEmail(string token);
        public Task<AddVacationPlanResultDTO> AddVacationPlan(VacationPlanDTO plan, int userId);
        public Task<UserDto> IsLoggedIn(string token);
    }
}
