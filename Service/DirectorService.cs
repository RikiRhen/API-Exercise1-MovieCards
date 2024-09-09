using AutoMapper;
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
    public class DirectorService : IDirectorService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DirectorService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DirectorDto>> GetDirectorsAsync(bool trackChanges = false)
        {
            return _mapper.Map<IEnumerable<DirectorDto>>(await _uow.Directors.GetDirectorsAsync(trackChanges));
        }

        public async Task<DirectorDto> GetDirectorDtoByIdAsync(int id, bool trackChanges)
        {
            var dto = await _uow.Directors.GetDirectorByIdAsync(id, trackChanges);
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            return _mapper.Map<DirectorDto>(dto);
        }
    }
}
