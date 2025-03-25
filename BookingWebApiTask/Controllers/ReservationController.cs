using AutoMapper;
using BookingWebApiTask.Application.Dtos;
using BookingWebApiTask.Application.Interfaces;
using BookingWebApiTask.Domain.Entities;
using BookingWebApiTask.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookingWebApiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllReservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            var result = await _unitOfWork.Reservation.GetAllAsync(
                        include: x => x.Include(y => y.Trip).Include(y => y.ReservedUser)
                );
            if (result == null || !result.Any())
                return NotFound("No reservations found.");

            var reservationResult = _mapper.Map<IEnumerable<ReservationResult>>(result);
            return Ok(reservationResult);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReservation([FromRoute] int id)
        {
            var result = await _unitOfWork.Reservation.GetAsync(
                                id,
                                include: x=>x.Include(y => y.Trip).Include(y => y.ReservedUser)
                                );

            if (result == null)
                return NotFound($"Reservation with ID {id} not found.");

            var reservationResult = _mapper.Map<ReservationResult>(result);
            return Ok(reservationResult);
        }

        [HttpPost("AddReservation")]
        public async Task<IActionResult> AddReservation([FromForm] ReservationDto reservationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reservedUser = await _unitOfWork.ApplicationUser.GetAsync(reservationDto.ReservedById!);
            if (reservedUser == null)
                return NotFound($"reservation with ID {reservationDto.ReservedById} not found.");

            var trip = await _unitOfWork.Trip.GetAsync((int)reservationDto.TripId!);
            if (trip == null)
                return NotFound($"Trip with ID {reservationDto.TripId} not found.");

            var reservation = _mapper.Map<Reservation>(reservationDto);
            reservation.Trip = trip;

            var result = await _unitOfWork.Reservation.AddAsync(reservation);
            if (result == null)
                return BadRequest("Failed to add reservation.");

            int rows = await _unitOfWork.SaveChangesAsync();
            if (rows > 0)
            {
                return CreatedAtAction(
                    nameof(GetReservation),
                    new { id = reservation.Id },
                    _mapper.Map<ReservationResult>(reservation)
                );
            }
            return BadRequest("Failed to save changes.");
        }

        [HttpPut("UpdateReservation/{id}")]
        public async Task<IActionResult> UpdateReservation([FromRoute] int id, [FromForm] ReservationDto reservationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingReservation = await _unitOfWork.Reservation.GetAsync(id);
            if (existingReservation == null)
                return NotFound($"Reservation with ID {id} not found.");

            var reservedUserId = reservationDto.ReservedById ?? existingReservation.ReservedById;
            var reservedUser = await _unitOfWork.ApplicationUser.GetAsync(reservedUserId);
            if (reservedUser == null)
                return NotFound($"User with ID {reservedUserId} not found.");

            existingReservation.ReservedUser = reservedUser;

            var tripId = existingReservation.TripId;
            if(reservationDto.TripId != null && reservationDto.TripId !=0)
                tripId = (int)reservationDto.TripId;
            var reservedTrip = await _unitOfWork.Trip.GetAsync(tripId);
            if (reservedTrip == null)
                return NotFound($"Trip with ID {reservationDto.TripId} not found.");
            existingReservation.Trip = reservedTrip;

            _mapper.Map(reservationDto, existingReservation);
            
            _unitOfWork.Reservation.Update(existingReservation);

            int rows = await _unitOfWork.SaveChangesAsync();
            if (rows > 0)
                return Ok(new { message = "Update Successfully", data = _mapper.Map<ReservationResult>(existingReservation) });

            return BadRequest("Failed to update reservation.");
        }

        [HttpDelete("DeleteReservation/{id:int}")]
        public async Task<IActionResult> DeleteReservation([FromRoute] int id)
        {
            var reservation = await _unitOfWork.Reservation.GetAsync(id);
            if (reservation == null)
                return NotFound($"Reservation with ID {id} not found.");

            await _unitOfWork.Reservation.DeleteAsync(reservation);
            int rows = await _unitOfWork.SaveChangesAsync();
            if (rows > 0)
                return Ok($"Reservation with ID {id} deleted successfully.");

            return BadRequest("Failed to delete reservation.");
        }
    }
}
