using AutoMapper;
using BookingWebApiTask.Application.Dtos;
using BookingWebApiTask.Application.Interfaces;
using BookingWebApiTask.Application.Services;
using BookingWebApiTask.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingWebApiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TripController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet, Route("GetAllTrips")]
        public async Task<IActionResult> GetAllTrips()
        {
            var result = await _unitOfWork.Trip.GetAllAsync();
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetTrip([FromRoute]int id)
        {
            var result = await _unitOfWork.Trip.GetAsync(id);
            if (result == null)
                return NotFound($"This id not exist {id}");
            return Ok(result);
        }

        [HttpPost("AddTrip")]
        public async Task<IActionResult> AddTrip([FromForm]TripDto tripDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(tripDto);

            // save image to wwwroot/images
            tripDto.ImageUrl = await ImageServices.SaveImageAsync(tripDto.ImageFile!, _hostEnvironment.WebRootPath);

            Trip trip = _mapper.Map<Trip>(tripDto);
            var result = await _unitOfWork.Trip.AddAsync(trip);
            if (result != null)
            {
                int rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0)
                    return Ok(result);
            }
            return BadRequest();
        }

        [HttpPut("UpdateTrip/{id}")]
        public async Task<IActionResult> UpdateTrip([FromRoute] int id, [FromForm] TripDto tripDto)
        {
            Trip? oldTrip = await _unitOfWork.Trip.GetAsync(id);
            if (oldTrip == null)
                return NotFound();

            if (tripDto.ImageFile != null)
            {
                ImageServices.DeleteImage(oldTrip.ImageUrl, _hostEnvironment.WebRootPath);
                oldTrip.ImageUrl = await ImageServices.SaveImageAsync(tripDto.ImageFile, _hostEnvironment.WebRootPath);
            }
            _mapper.Map(tripDto, oldTrip);
            
            var result = _unitOfWork.Trip.Update(oldTrip);
            if (result != null)
            {
                int rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0)
                    return Ok(result);
            }

            return BadRequest();
        }


        [HttpDelete("DeleteTrip/{id}")]
        public async Task<IActionResult> DeleteTrip([FromRoute] int id)
        {
            var trip = await _unitOfWork.Trip.GetAsync(id);
            if (trip == null)
                return NotFound();

            ImageServices.DeleteImage(trip.ImageUrl, _hostEnvironment.WebRootPath);

            await _unitOfWork.Trip.DeleteAsync(trip);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
                return Ok();

            return BadRequest();
        }



    }
}
