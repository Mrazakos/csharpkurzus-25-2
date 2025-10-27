using MovieBox.Core.Filter;
using MovieBox.Core.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBox.Core.Service
{
    public interface IMovieFilterService
    {

        /// <summary>
        /// Filters a collection of movies based on the provided criteria.
        /// </summary>
        /// <param name="movies">The collection of movies to filter.</param>
        /// <param name="criteria">The filter criteria to apply.</param>
        /// <returns>A list of movies that match the filter criteria.</returns>
        IEnumerable<Movie> FilterMovies(IEnumerable<Movie> movies, MovieFilterCriteria criteria);
    }

    public class MovieFilterService : IMovieFilterService
    {
        public IEnumerable<Movie> FilterMovies(IEnumerable<Movie> movies, MovieFilterCriteria criteria)
        {
            var query = movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                query = query.Where(m => m.Title.Contains(criteria.Title, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(criteria.DirectorContains))
            {
                query = query.Where(m => m.Director.Contains(criteria.DirectorContains, StringComparison.OrdinalIgnoreCase));
            }
            if (criteria.Year.HasValue)
            {
                query = query.Where(m => m.ReleaseYear == criteria.Year.Value);
            }
            if (criteria.RatingMin.HasValue)
            {
                query = query.Where(m => m.Rating >= criteria.RatingMin.Value);
            }
            if (criteria.RatingMax.HasValue)
            {
                query = query.Where(m => m.Rating <= criteria.RatingMax.Value);
            }

            return criteria.ResultCount.HasValue ? query.Take(criteria.ResultCount.Value).ToList() : query.ToList();
        }
    }
}
