import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  httpClient = inject(HttpClient)

  constructor() { }


  bookList(pageIndex: number, pageSize: number, searchText: string | null, sortColumn: string, sortOrder: number, authorId: number | null, publisherId: number | null, genreId: number | null, languageId: number | null): any {
    return this.httpClient.get('/api/books/booklist?' + 'pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&sortOrder=' + sortOrder + '&searchText=' + searchText + '&sortColumn=' + sortColumn + '&authorId=' + authorId + '&publisherId=' + publisherId + '&genreId=' + genreId + '&languageId=' + languageId);
  }

  deleteBook(id: number) {
    return this.httpClient.delete('/api/books?bookId=' + id,)
  }

  getBook(id: number) {
    return this.httpClient.get('/api/books?bookId=' + id);
  }

  saveBook(data: any) {
    return this.httpClient.post('/api/books/savebook', data)
  }


  getBookCopies(id: number) {
    return this.httpClient.get('/api/books/bookcopies?bookId=' + id);
  }

  downloadFile(id: number) {
    return this.httpClient.get(`/api/books/download/${id}`, {
      responseType: 'json'
    });
  }

  getBookDetail(id: number) {
    return this.httpClient.get('/api/books/bookdetail?bookId=' + id);
  }

}
