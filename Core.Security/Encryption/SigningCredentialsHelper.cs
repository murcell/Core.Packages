﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Encryption;

/// <summary>
/// JsonWebtokeni imzalayacağımız nesneyi oluşturur
/// </summary>
public static class SigningCredentialsHelper
{
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)=>new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512Signature);
}
