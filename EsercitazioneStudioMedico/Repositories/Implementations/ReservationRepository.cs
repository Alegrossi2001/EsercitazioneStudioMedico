using EsercitazioneStudioMedico.Context;
using EsercitazioneStudioMedico.Models.Domains;
using EsercitazioneStudioMedico.Repositories.Interfaces;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EsercitazioneStudioMedico.Repositories.Implementations
{
    public class ReservationRepository: IReservationRepository
    {
        private readonly ApplicationDBContext context;
        private string filepath;
        public ReservationRepository(ApplicationDBContext dbContext, IWebHostEnvironment environment)
        {
            this.context = dbContext;
            this.filepath = Path.Combine(environment.ContentRootPath, "Data", "FakeDB.json");
        }

        public async Task<Reservation> CreateAsync(Reservation reservation)
        {
            await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();
            return reservation;
        }

        public async Task<string> CreateReservation(Reservation reservation)
        {
            string reservationTicket = reservation.Ticket;

            var reservationsByDate = await GetReservationByDate(reservation.PrenotationDate);
            var existingReservationWithSameFiscalCode = await GetReservationByFiscalCode(reservation.FiscalCode);
            if(reservationsByDate.Count() > 5 || existingReservationWithSameFiscalCode.Count() > 1)
            {
                return null;
            }
            else
            {
                var reservations = await GetAllReservations();
                reservations.Add(reservation);
                var jsonData = JsonSerializer.Serialize(reservations);
                await File.WriteAllTextAsync(filepath, jsonData);
                return reservationTicket;
            }

        }

        public async Task<bool> DeleteReservation(DateTime date, string fiscalCode, string ticket)
        {
            var reservations = await GetAllReservations();
            var reservationToRemove = reservations.FirstOrDefault(r =>
                r.PrenotationDate.Date == date.Date &&
                r.FiscalCode == fiscalCode &&
                r.Ticket == ticket);
            if(reservationToRemove != null)
            {
                reservations.Remove(reservationToRemove);
                var jsonData = JsonSerializer.Serialize(reservations);
                await File.WriteAllTextAsync(filepath, jsonData);
                return true;
            }

            return false;

            

        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            var jsonData = await File.ReadAllTextAsync(filepath);
            List<Reservation> reservations = JsonConvert.DeserializeObject<List<Reservation>>(jsonData);
            reservations = reservations.OrderBy(date => date.CreationDate).ToList();
            return reservations;
        }

        public async Task<List<Reservation>> GetReservationByDate(DateTime date)
        {
            var reservations = await GetAllReservations();
            return reservations.Where(r => r.PrenotationDate.Date == date.Date).ToList();
        }

        public async Task<List<Reservation>> GetReservationByFiscalCode(string fiscalCode)
        {
            var reservations = await GetAllReservations();
            return reservations.Where(r => r.FiscalCode == fiscalCode).ToList();
        }

        public async Task<List<Reservation>> GetReservationByTicket(string ticket)
        {
            var reservations = await GetAllReservations();
            return reservations.Where(r => r.Ticket == ticket).ToList();
        }
    }
}
