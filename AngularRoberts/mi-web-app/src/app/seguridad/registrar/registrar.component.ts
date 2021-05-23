import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SeguridadService } from '../seguridad.service';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.css'],
})
export class RegistrarComponent implements OnInit {
  constructor(private seguridadService: SeguridadService) {}

  ngOnInit(): void {}

  registrarUsuario(form: NgForm): void {
    this.seguridadService.registrarUsuario({
      email: form.value.email,
      password: form.value.password,
      apellido: form.value.apellido,
      nombre: form.value.nombre,
      username: form.value.username,
      usuarioId: '',
      token: ''
    });
  }
}
