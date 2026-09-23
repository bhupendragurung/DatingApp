using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.Auth;
   public sealed record UserDto(Guid Id, string UserName, string KnownAs);
