namespace EsercitazioneStudioMedico.Models.Domains
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public DateTime PrenotationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string? FiscalCode { get; set; }
        public string? Ticket { get; set; }

    }
}
