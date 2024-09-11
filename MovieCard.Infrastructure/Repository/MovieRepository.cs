﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieCard.Contracts;
using MovieCard.Infrastructure.Data;
using MovieCard.Models.Entities;
using MovieCard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MovieCard.Infrastructure.Repository
{
    public class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {

        public MovieRepository(MovieCardContext context): base(context){}

        //OBJECT MARKERAD
        public async Task<IEnumerable<object>> GetMoviesAsync(string? title, string? genre, string? director, string? actor, string? releaseDate, string? sortBy, string? sortOrder, bool trackChanges, bool detailed, IMapper mapper)
        {
            var query = GetAll(trackChanges);

            if (!string.IsNullOrEmpty(title))
            {
                title = title.Trim();
                query = query.Where(m => m.Title == title);
            }
            if (!string.IsNullOrEmpty(genre))
            {
                genre = genre.Trim();
                query = query.Include(m => m.Genres).Where(m => m.Genres.Any(g => g.GenreName == genre));
            }

            if (!string.IsNullOrEmpty(director))
            {
                director = director.Trim().ToLower();
                query = query.Where(m => m.Director.Name.Replace(" ", "").ToLower().Equals(director));
            }

            if (!string.IsNullOrEmpty(actor))
            {
                actor = actor.Trim().ToLower();
                query = query.Include(m => m.Actors).Where(m => m.Actors.Any(a => a.Name.Replace(" ", "").ToLower().Equals(actor)));
            }

            if (!string.IsNullOrEmpty(releaseDate))
            {
                releaseDate = releaseDate.Trim();
                query = query.Where(m => m.ReleaseDate == releaseDate);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isDescending = !string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("desc");

                query = sortBy.ToLower() switch
                {
                    "title" => isDescending ? query.OrderByDescending(m => m.Title) : query.OrderBy(m => m.Title),
                    "rating" => isDescending ? query.OrderByDescending(m => m.Rating) : query.OrderBy(m => m.Rating),
                    "realeasedate" => isDescending ? query.OrderByDescending(m => m.ReleaseDate) : query.OrderBy(m => m.ReleaseDate),
                    _ => query.OrderBy(m => m.Id)
                };
            }

            if (detailed)
            {
                var queryResults = await query
                .Include(m => m.Director)
                .Include(m => m.Director.ContactInfo)
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .ProjectTo<MovieDetailDto>(mapper.ConfigurationProvider)
                .ToListAsync();

                return queryResults;
            }
            else
            {
                var queryResults = await query
                    .Include(m => m.Director)
                    .ProjectTo<MovieDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return queryResults;
            }

            //return await GetAll(trackChanges).ToListAsync();

        }

        public async Task<Movie?> GetMovieByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(m => m.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }
    }
}
