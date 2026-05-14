using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public TokenResultDto GenerateToken(UserDto user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserGuid", user.Id.ToString()) // ← این را اضافه کنید
    };

        if (user.Roles != null)
        {
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Id.ToString()));
                if (!string.IsNullOrWhiteSpace(role.Code))
                    claims.Add(new Claim("ROLECODE", role.Code));

            }
        }

        var tokenExpiryHours = _jwtSettings.ExpiryHours;
        var expireDate = DateTime.UtcNow.AddHours(tokenExpiryHours);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expireDate,
            signingCredentials: creds
        );

        return new TokenResultDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireDate = expireDate
        };
    }

    // 🔹 تولید RefreshToken
    public TokenResultDto GenerateRefreshToken(UserDto user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var tokenHandler = new JwtSecurityTokenHandler();

        // 🔹 claims مشابه AccessToken، فقط می‌توان محدودتر کرد
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserGuid", user.Id.ToString())
    };

        if (user.Roles != null)
        {
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Id.ToString()));
                if (!string.IsNullOrWhiteSpace(role.Code))
                    claims.Add(new Claim("ROLECODE", role.Code));
            }
        }

        var expireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expireDate,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new TokenResultDto
        {
            Token = tokenHandler.WriteToken(token),
            ExpireDate = expireDate
        };
    }



    // 🔹 اعتبارسنجی RefreshToken (در صورت نیاز JWT)
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false // برای refreshToken معمولاً خودکار expire بررسی می‌شود
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }


}

