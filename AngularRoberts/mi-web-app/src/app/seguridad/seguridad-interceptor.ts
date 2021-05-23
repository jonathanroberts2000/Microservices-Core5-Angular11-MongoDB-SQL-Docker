import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SeguridadService } from './seguridad.service';

@Injectable()
export class SeguridadInterceptor implements HttpInterceptor {
  constructor(private seguridadService: SeguridadService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): any {
    const tokenSeguridad = this.seguridadService.obtenerToken();
    const request = req.clone({
      headers: req.headers.set('Authorization', 'Bearer ' + tokenSeguridad),
    });

    return next.handle(request);
  }
}
