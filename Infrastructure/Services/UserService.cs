using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Models.Request;
using ApplicationCore.Models.Response;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel registerRequestModel)
        {
            // first check whether email exists in database
            var dbUser = await _userRepository.GetUserByEmail(registerRequestModel.Email);



            // user exists in database
            if (dbUser != null)
            {
                throw new ConflictException("user already exists, please login");
                //throw new Exception("user already exists, please login");
            }


            // continue with our hashing
            // create a salt and add salt to the user entered password and create user object and save it to db
            //Generate Salt
            var salt = CreateSalt();

            //var saltExistsDb = await _userRepository.SaltExists(salt);

            //if (!saltExistsDb)
            //{
            //    //continue
            //}

            //Hash password
            var hashedPassword = HashPassword(registerRequestModel.Password, salt);
            //Save user Entity in DB
            var user = new User
            {
                FirstName = registerRequestModel.FirstName,
                LastName = registerRequestModel.LastName,
                Email = registerRequestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                DateOfBirth = registerRequestModel.DateOfBirth
            };

            var createdUser = await _userRepository.AddAsync(user);

            var createdUserResponseModel = new UserRegisterResponseModel
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName
            };

            return createdUserResponseModel;
        }

        private string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }



            return Convert.ToBase64String(randomBytes);
        }

        private string HashPassword(string password, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public async Task<LoginResponseModel> ValidateUser(string email, string password)
        {
            // we should go to database and get the record by email

            var dbUser = await _userRepository.GetUserByEmail(email);

            if (dbUser == null)
            {
                return null;
            }

            var hashedPassword = HashPassword(password, dbUser.Salt);
            if (hashedPassword == dbUser.HashedPassword)
            {
                // user entered correct password
                var loginUserResponse = new LoginResponseModel
                {
                    Id = dbUser.Id,
                    FirstName = dbUser.FirstName,
                    LastName = dbUser.LastName,
                    Email = dbUser.Email
                };
                return loginUserResponse;
            }

            return null;
        }

        public async Task<UserRegisterResponseModel> GetUserDetails(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User", id);

            var response = new UserRegisterResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return response;
        }

        public async Task<User> GetUser(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }
    }
}
