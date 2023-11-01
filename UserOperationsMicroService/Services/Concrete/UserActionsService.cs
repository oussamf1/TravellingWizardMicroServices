using UserOperationsMicroService.DTOs;
using Shared.Models;
using UserOperationsMicroService.Services.Interface;
using System.Text.Json;
using Shared.Services.Interface;
using Shared.Repos.Interface;
using Shared.Models;


namespace UserOperationsMicroService.Services.Concrete
{
    public class UserActionsService : IUserActionsService
    {
        private readonly IPasswordComputationService _pwdComputationService;
        private readonly IUserRepo _userRepo;
        private readonly IJwtTokenOpsServicecs _jwtTokenOpsService;
        private readonly IEmailService _emailService;
        private readonly IVacationPlanRepo _vacationPlanRepo;
        public UserActionsService(IVacationPlanRepo vacationPlanRepo, IPasswordComputationService pwdComputationService, IUserRepo userRepo, IJwtTokenOpsServicecs jwtTokenOpsService, IEmailService emailService)
        {
            _pwdComputationService = pwdComputationService;
            _userRepo = userRepo;
            _jwtTokenOpsService = jwtTokenOpsService;
            _emailService = emailService;
            _vacationPlanRepo= vacationPlanRepo;
        }
        public async Task<bool> ConfirmUserEmail(string token)
        {
            var user = await _userRepo.GetByToken(token);

            if (user == null)
            {
                return false; 
            }

            if (user.ConfirmationToken != token)
            {
                return false; 
            }

            user.IsEmailConfirmed = true;

            bool isSaved = await _userRepo.ConfirmUserEmail(user.Id);

            return isSaved;
        }
        public async Task<UserDto> IsLoggedIn(string token)
        {
            var userEmail = _jwtTokenOpsService.VerifyToken(token);

            var user = await _userRepo.GetByEmail(userEmail);

            var userDTO = new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return userDTO;
        }
        public async Task<UserRegistrationResultDTO> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            bool isExistingUser = await _userRepo.GetByEmail(userRegistrationDTO.Email) != null;
            if (isExistingUser)
            {
                var response = new UserRegistrationResultDTO()
                {
                    isRegistered = false,
                    ErrosMessages = new[]
                    {
                        "User already exists"
                    },
                };

                return response;
            }
            else
            {
                string passwordHash = _pwdComputationService.ComputePasswordHash(userRegistrationDTO.Password);

                User user = new User
                {
                    FirstName = userRegistrationDTO.FirstName,
                    LastName = userRegistrationDTO.LastName,
                    Email = userRegistrationDTO.Email,
                    PasswordHash = passwordHash,
                    ConfirmationToken = GenerateConfirmationToken()
                };

                string confirmationLink = $"https://localhost:44366/api/Authentication/ConfirmAccount?token={user.ConfirmationToken}";

                await _emailService.SendConfirmationEmail(user, confirmationLink);

                await _userRepo.Add(user);

                var response = new UserRegistrationResultDTO()
                {
                    isRegistered = true,
                    ErrosMessages = new List<string>(),
                };

                return response;
            }
        }
        public async Task<UserAuthResultDTO> Login(UserAuthenticationDTO userAuth)
        {

            var user = await _userRepo.GetByEmail(userAuth.Email);

            var isAuth = _pwdComputationService.VerifyPassword(userAuth.Password, user.PasswordHash);

            if (isAuth)
            {
                if(user.IsEmailConfirmed)
                {
                    var response = new UserAuthResultDTO()
                    {
                        isAuthenticated = true,

                        Token = _jwtTokenOpsService.GenerateToken(user),

                        ErrorMessages = new List<string>(),
                    };

                    return response;
                }
                else
                {
                    var response = new UserAuthResultDTO()
                    {
                        isAuthenticated = false,

                        Token = null,

                        ErrorMessages = new[]
                 {
                        "Please verify your email first"
                    },
                    };

                    return response;

                }
  

            }
            else
            {
                var response = new UserAuthResultDTO()
                {
                    isAuthenticated = false,

                    Token = null,

                    ErrorMessages = new[]
                    {
                        "Email and password combination is incorrect"
                    },
                };

                return response;

            }
        }
        public async Task<AddVacationPlanResultDTO> AddVacationPlan(VacationPlanDTO plan, int userId)
        {
            try
            {
                string planJson = JsonSerializer.Serialize(plan);

                var vacationPlan = new VacationPlan
                {
                    UserId = userId,
                    planInJson = planJson,
                    SubmissionDate = DateTime.Now,
                };

                await _vacationPlanRepo.Add(vacationPlan);

                var response = new AddVacationPlanResultDTO
                {
                    success = true,
                    Messages = new[]
                    {
                        "Vacation plan have been added succesfully"
                    },
                };

                return response;

            }
            catch (Exception ex)
            {
                var response = new AddVacationPlanResultDTO
                {
                    success = true,
                    Messages = new[]
                 {
                        "Operation failed !"
                    },
                };
                return response;

            }

        }

        private string GenerateConfirmationToken()
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new char[20];

            for (int i = 0; i < 20; i++)
            {
                token[i] = allowedChars[random.Next(allowedChars.Length)];
            }

            return new string(token);
        }

    }
}
