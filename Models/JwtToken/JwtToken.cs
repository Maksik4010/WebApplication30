using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication30.Models.JwtToken
{
    public sealed class JwtToken
    {
        // Fields
        private JwtSecurityToken token;

        // Properties
        public DateTime ValidTo => token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(token);

        // Constructor
        public JwtToken(JwtSecurityToken token)
        {
            this.token = token;
        }
        
    }
}
