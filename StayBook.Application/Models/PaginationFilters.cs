namespace StayBook.Application.Models;

public record PaginationFilters
    (
        int Skip,
        int Take
    );