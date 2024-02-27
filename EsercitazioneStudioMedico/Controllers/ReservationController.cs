using EsercitazioneStudioMedico.Helper;
using EsercitazioneStudioMedico.Models.Domains;
using EsercitazioneStudioMedico.Models.DTO;
using EsercitazioneStudioMedico.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EsercitazioneStudioMedico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationRepository reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        //GET api/reservations
        [HttpGet]
        public async Task<IActionResult> GetReservation()
        {
            var reservations = await reservationRepository.GetAllReservations();
            return Ok(reservations);
        }

        //GET api/reservations
        [HttpGet("bydate")]
        public async Task<IActionResult> GetReservationsByDate([FromQuery]DateTime date)
        {
            var reservations = await reservationRepository.GetReservationByDate(date.Date);
            if (reservations.Any())
            {
                return Ok(reservations);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("byfiscalcode")]
        public async Task<IActionResult> GetReservationsByFiscalCode([FromQuery]string fiscalCode)
        {
            var reservations = await reservationRepository.GetReservationByFiscalCode(fiscalCode);
            if (reservations.Any())
            {
                return Ok(reservations);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("byticket")]
        public async Task<IActionResult> GetReservationByTicket([FromQuery] string ticket)
        {
            var reservations = await reservationRepository.GetReservationByTicket(ticket);
            if (reservations.Any())
            {
                return Ok(reservations);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateReservation([FromQuery]ReservationInfoDTO reservationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                PrenotationDate = reservationDTO.PrenotationDate,
                CreationDate = DateTime.UtcNow,
                FiscalCode = reservationDTO.FiscalCode,
                Ticket = AlphanumericStringGenerator.GenerateRandomString(16)
            };

            await reservationRepository.CreateReservation(reservation);
            return Ok(reservation.Ticket);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteReservation([FromQuery]ReservationDeleteDTO reservationDTO)
        {
            if (await reservationRepository.DeleteReservation(reservationDTO.PrenotationDate, reservationDTO.FiscalCode, reservationDTO.Ticket))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
