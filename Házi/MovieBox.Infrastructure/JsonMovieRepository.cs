using MovieBox.Core.Records;
using MovieBox.Core.Repository;
using System.Text.Json;

namespace MovieBox.Infrastucture
{
    public class JsonMovieRepository : IMovieRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonMovieRepository()
        {
            _filePath = "movies.json";

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Asynchronously loads the list of movies from "movies.json".
        /// </summary>
        public async Task<IEnumerable<Movie>> LoadMovies()
        {
            if (!File.Exists(_filePath))
            {
                return Enumerable.Empty<Movie>();
            }

            try
            {
                string json = await File.ReadAllTextAsync(_filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return Enumerable.Empty<Movie>();
                }

                // Deserialize the JSON string into a list of Movie records
                var movies = JsonSerializer.Deserialize<List<Movie>>(json, _jsonOptions);
                return movies ?? Enumerable.Empty<Movie>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[JsonRepository] Error: JSON file is corrupt and cannot be read. {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[JsonRepository] Error: Failed to load movies. {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        /// <summary>
        /// Asynchronously overwrites "movies.json" with the current list of movies.
        /// </summary>
        public async Task SaveMovies(IEnumerable<Movie> movies)
        {
            try
            {
                string json = JsonSerializer.Serialize(movies, _jsonOptions);

                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[JsonRepository] Error: Failed to save movies. {ex.Message}");
            }
        }
    }
}
