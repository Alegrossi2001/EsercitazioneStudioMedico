using EsercitazioneStudioMedico.Models.Domains;

namespace EsercitazioneStudioMedico.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateAsync(Reservation reservation);

        Task<List<Reservation>> GetAllReservations();
        Task<List<Reservation>> GetReservationByDate(DateTime date);
        Task<List<Reservation>> GetReservationByFiscalCode(string fiscalCode);
        Task<List<Reservation>> GetReservationByTicket(string ticket);
        Task<string> CreateReservation(Reservation reservation);
        Task<bool> DeleteReservation(DateTime date, string fiscalCode, string ticket);


    }
}
