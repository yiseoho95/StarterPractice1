﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? userId =>
            Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity != null &&
                                       _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public string Email => _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        public string FullName => _httpContextAccessor.HttpContext?.User.Claims.
                                      FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value + " " +
                                  _httpContextAccessor.HttpContext?.User.Claims.
                                      FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

        public string ProfilePicture => throw new NotImplementedException();

        public string RemoteIpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        public bool IsAdmin => throw new NotImplementedException();

        public bool IsSuperAdmin => throw new NotImplementedException();

        public IEnumerable<string> Roles => throw new NotImplementedException();

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            throw new NotImplementedException();
        }
    }
}