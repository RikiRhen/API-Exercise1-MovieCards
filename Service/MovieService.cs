using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using MovieCard.Contracts;
using MovieCard.Shared.DTOs;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesAsync(bool trackChanges = false)
        {
            return _mapper.Map<IEnumerable<MovieDto>>(await _uow.Movies.GetMoviesAsync(trackChanges));
        }

        public async Task<MovieDto> GetMovieDtoByIdAsync(int id, bool trackChanges = false)
        {
            var dto = await _uow.Movies.GetMovieByIdAsync(id, trackChanges);
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            return _mapper.Map<MovieDto>(dto);
        }

    }
}
