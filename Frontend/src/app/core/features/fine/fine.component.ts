import { Component } from '@angular/core';
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-fine',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './fine.component.html',
  styleUrl: './fine.component.css'
})
export class FineComponent {
  usuario: string = '';
  motivo: string = '';
  monto: number | null = null;
  denominacion: string = '';
  usuarios: string[] = ['Usuario 1', 'Usuario 2', 'Usuario 3'];

  onSubmit() {
    // Lógica para manejar el envío del formulario
    if (this.usuario && this.motivo && this.monto && this.denominacion) {
      console.log('Multa creada:', { usuario: this.usuario, motivo: this.motivo, monto: this.monto, denominacion: this.denominacion });
      // Aquí podrías enviar la multa al backend o mostrar un mensaje de éxito
    }
  }
}
