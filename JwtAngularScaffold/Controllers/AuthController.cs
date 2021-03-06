﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JwtAngularScaffold.Contracts;
using JwtAngularScaffold.Identity;
using JwtAngularScaffold.Models;
using JwtAngularScaffold.Models.Entities;
using JwtAngularScaffold.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAngularScaffold.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _repo;

        public AuthController(ApplicationDbContext context, IUserRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET api/values
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            User user = _repo.Authenticate(model);
            if (user == null)
            {
                return Unauthorized();
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretdevlin@12345"));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001", // This parameter is a simple string representing the name of the web server that issues the token
                audience: "https://localhost:5001", // This parameter is a string value representing valid recipients
                claims: new
                    List<Claim> // This is list of user roles, for example, the user can be an admin, manager or author 
                    {
                        new Claim("", ""), // TODO: Add claims
                    },
                expires: DateTime.UtcNow
                    .AddDays(1), // DateTime object that represents the date and time after which the token expires
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new {Token = tokenString}); // Just an object with token to be returned
        }
    }
}