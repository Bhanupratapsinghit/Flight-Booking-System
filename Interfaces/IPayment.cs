using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightProject.Models;

namespace FlightProject.Interfaces
{
    public interface IPayment
    {
        //Task<Payment> ProcessPayment(int bookingId, string paymentMethod,decimal amount);
        Task<Payment> ProcessPayment(int bookingId, string paymentMethod,decimal amount);
    }
}