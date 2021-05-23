import { Subject } from 'rxjs';

export class LibrosService {

  librosSubject = new Subject();

  private libros = [
    'Libro de Jonathan',
    'Libro de Aritmetica',
    'El Grafico Revista',
  ];

  agregarLibro(libroNombre: string): void {
    this.libros.push(libroNombre);
    this.librosSubject.next();
  }

  eliminarLibro(libroNombre: string): void{
    this.libros = this.libros.filter(x => x !== libroNombre);
    this.librosSubject.next();
  }

  obtenerLibros(): any {
    return [...this.libros];
  }
}
