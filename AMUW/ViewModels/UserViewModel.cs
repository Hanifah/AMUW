﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMUW.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
    }

    public class BaseUserViewModel
    {
        public string UserRole { get; set; }
    }
}