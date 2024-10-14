
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Whiskey_TastingTale_Backend.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next; 
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;   
            _configuration = configuration; 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/User/login"))
            {
                // swagger 요청인 경우 JWT 검증을 하지 않고 다음 미들웨어로 바로 넘김
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split("").Last();
            if (token != null)
            {
                var result = AttachUserToContext(context, token);
                if (result == false) {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("{\"message\": \"Token validation failed.\"}");
                    return; 
                }

            }else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Authorization token is missing.");
                return;
            }

            await _next(context);
        }

        private bool AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                var realToken = token.Replace("Bearer ", ""); 

                tokenHandler.ValidateToken(realToken, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

                context.Items["UserId"] = userId;
                context.Items["UserRole"] = role;

                return true;
            }
            catch
            {
                // 토큰 검증 실패 처리
                return false; 
            }
        }
    }
}
