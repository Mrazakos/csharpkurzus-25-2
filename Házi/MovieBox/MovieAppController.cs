using MovieBox.Core.Records;
using MovieBox.Core.Service;
using MovieBox.Ui;
using System.Threading.Tasks;


namespace MovieBox
{
    public class MovieAppController
    {
        private readonly IMovieService _movieService;
        private readonly IConsoleUIService _ui;

        public MovieAppController(IMovieService movieService, IConsoleUIService ui)
        {
            _movieService = movieService;
            _ui = ui;
        }

        /// <summary>
        /// Starts and runs the main application loop.
        /// </summary>
        public async Task Run()
        {
            await _movieService.InitializeAsync();
            if (_movieService.GetAllMovies().Count() == 0)
                AddInitialData();

            bool keepRunning = true;
            while (keepRunning)
            {
                _ui.Clear();
                string choice = _ui.GetMainMenuChoice();
                _ui.Clear();

                switch (choice)
                {
                    case "1":
                        ListAllMovies();
                        break;
                    case "2":
                        AddNewMovie();
                        break;
                    case "3":
                        SearchForMovies();
                        break;
                    case "4":
                        ShowStatistics();
                        break;
                    case "5":
                        keepRunning = false;
                        break;
                    default:
                        _ui.ShowMessage("Invalid option, please try again.", MessageType.Error);
                        break;
                }

                if (keepRunning)
                {
                    _ui.WaitForUser();
                }
            }

            await _movieService.SaveAsync();
        }

        // --- Menu Action Methods ---

        private void ListAllMovies()
        {
            _ui.ShowMessage("--- All Movies ---", MessageType.Title);
            var movies = _movieService.GetAllMovies();
            _ui.DisplayMovies(movies);
        }

        private void AddNewMovie()
        {
            try
            {
                var movie = _ui.GetNewMovieData();
                _movieService.AddMovie(movie);
                _ui.ShowMessage("\nMovie added successfully!", MessageType.Success);
                _ui.ShowMessage($"You added: {movie.Title} ({movie.ReleaseYear})");
            }
            catch (Exception ex)
            {
                _ui.ShowMessage($"\nFailed to add movie: {ex.Message}", MessageType.Error);
            }
        }

        private void SearchForMovies()
        {
            try
            {
                var criteria = _ui.GetSearchCriteria();
                var movies = _movieService.SearchMovies(criteria);
                _ui.ShowMessage("\n--- Search Results ---", MessageType.Title);
                _ui.DisplayMovies(movies);
            }
            catch (Exception ex)
            {
                _ui.ShowMessage($"\nFailed to search: {ex.Message}", MessageType.Error);
            }
        }

        private void ShowStatistics()
        {
            var stats = _movieService.GetMovieCollectionStats();
            _ui.DisplayStats(stats);
        }

        private void AddInitialData()
        {
            _movieService.AddMovie(new Movie("The Shawshank Redemption", "Frank Darabont", 1994, 9.3));
            _movieService.AddMovie(new Movie("The Godfather", "Francis Ford Coppola", 1972, 9.2));
            _movieService.AddMovie(new Movie("The Dark Knight", "Christopher Nolan", 2008, 9.0));
            _movieService.AddMovie(new Movie("Pulp Fiction", "Quentin Tarantino", 1994, 8.9));
            _movieService.AddMovie(new Movie("Forrest Gump", "Robert Zemeckis", 1994, 8.8));
            _movieService.AddMovie(new Movie("Inception", "Christopher Nolan", 2010, 8.8));
        }
    }
}
