export interface BookSearchFilter {
    authorId: number | null,
    publisherId: number | null,
    genreId: number | null,
    languageId: number | null,
    searchText: string,
    sortColumn: string,
    isSortAscending: boolean
}