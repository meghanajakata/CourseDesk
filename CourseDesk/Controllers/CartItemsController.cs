using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseDesk.Data;
using CourseDesk.Models;
using System.Diagnostics;
using CourseDesk.Repositories;
using CourseDesk.Filters;

namespace CourseDesk.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ICartRepository _cartRepository;
        public CartItemsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }


       

       
    }
}