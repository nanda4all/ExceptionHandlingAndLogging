using System;

namespace MoviesApi.Models
{
    public class InvalidMovieException : Exception
    {
        public InvalidMovieException(string message) : base(message)
        {

        }
    }
}
