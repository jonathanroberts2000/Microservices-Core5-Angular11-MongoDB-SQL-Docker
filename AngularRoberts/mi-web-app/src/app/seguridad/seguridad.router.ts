import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { SeguridadService } from './seguridad.service';

@Injectable()
export class SeguridadRouter implements CanActivate {
  constructor(
    private seguridadService: SeguridadService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): any {
    if (this.seguridadService.onSesion()) {
      return true;
    } else {
      this.router.navigate(['/login']);
    }
  }
}
