using AutoMapper;

namespace AsyncAPI
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Entities.Book, Models.Book>()
                .ForMember(dest => dest.Author, // Models.Book.Author
                    opt => opt.MapFrom(src =>
                        $"{src.Author.FirstName} {src.Author.LastName}")); // Entities.Author

            CreateMap<Models.BookForCreation, Entities.Book>();
        }
    }
}