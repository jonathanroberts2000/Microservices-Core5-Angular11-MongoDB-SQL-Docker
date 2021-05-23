import { Component, EventEmitter, OnInit, Output, OnDestroy } from '@angular/core';
import { SeguridadService } from '../../seguridad/seguridad.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-barra',
  templateUrl: './barra.component.html',
  styleUrls: ['./barra.component.css'],
})
export class BarraComponent implements OnInit, OnDestroy {
  @Output() menuToggle = new EventEmitter<void>();
  estadoUsuario: boolean;
  usuarioSubscription: Subscription;

  constructor(private seguridadSerivicio: SeguridadService) {}

  ngOnInit(): void {
    this.usuarioSubscription = this.seguridadSerivicio.seguridadCambio.subscribe(status => {
      this.estadoUsuario = status;
    });
  }

  ngOnDestroy(): void{
    this.usuarioSubscription.unsubscribe();
  }

  terminarSesion(): void{
    this.seguridadSerivicio.salirSesion();
  }

  onMenuToggleDispatch(): void {
    this.menuToggle.emit();
  }
}
